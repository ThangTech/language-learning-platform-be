using System.Security.Claims;
using FluentValidation;
using LanguagePlatform.API.Helpers;
using LanguagePlatform.Application.DTOs.Vocabulary;
using LanguagePlatform.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanguagePlatform.API.Controllers;

[ApiController]
[Route("api/words")]
public class WordsController : ControllerBase
{
    private readonly IVocabularyService _vocabService;
    private readonly IValidator<CreateWordRequest> _createValidator;
    private readonly IValidator<UpdateWordRequest> _updateValidator;

    public WordsController(
        IVocabularyService vocabService,
        IValidator<CreateWordRequest> createValidator,
        IValidator<UpdateWordRequest> updateValidator)
    {
        _vocabService = vocabService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
        [FromQuery] string? level = null, [FromQuery] string? search = null)
        => Ok(await _vocabService.GetWordsAsync(page, pageSize, level, search));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
        => Ok(await _vocabService.GetWordByIdAsync(id));

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWordRequest request)
    {
        var invalid = await ValidationHelper.ValidateAsync<CreateWordRequest, object>(_createValidator, request);
        if (invalid != null) return BadRequest(invalid);

        return Ok(await _vocabService.CreateWordAsync(request));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateWordRequest request)
    {
        var invalid = await ValidationHelper.ValidateAsync<UpdateWordRequest, object>(_updateValidator, request);
        if (invalid != null) return BadRequest(invalid);

        return Ok(await _vocabService.UpdateWordAsync(id, request));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
        => Ok(await _vocabService.DeleteWordAsync(id));
}

[ApiController]
[Route("api/favorites")]
[Authorize]
public class FavoritesController : ControllerBase
{
    private readonly IVocabularyService _vocabService;
    public FavoritesController(IVocabularyService vocabService) => _vocabService = vocabService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _vocabService.GetFavoritesAsync(GetUserId()));

    [HttpPost("{wordId:guid}")]
    public async Task<IActionResult> Add(Guid wordId)
        => Ok(await _vocabService.AddFavoriteAsync(GetUserId(), wordId));

    [HttpDelete("{wordId:guid}")]
    public async Task<IActionResult> Remove(Guid wordId)
        => Ok(await _vocabService.RemoveFavoriteAsync(GetUserId(), wordId));

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}

[ApiController]
[Route("api/flashcards")]
[Authorize]
public class FlashcardsController : ControllerBase
{
    private readonly IVocabularyService _vocabService;
    public FlashcardsController(IVocabularyService vocabService) => _vocabService = vocabService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _vocabService.GetFlashcardsAsync(GetUserId()));

    [HttpPost("{wordId:guid}")]
    public async Task<IActionResult> Add(Guid wordId)
        => Ok(await _vocabService.AddToFlashcardAsync(GetUserId(), wordId));

    [HttpPut("{wordId:guid}/learned")]
    public async Task<IActionResult> MarkLearned(Guid wordId)
        => Ok(await _vocabService.MarkFlashcardLearnedAsync(GetUserId(), wordId));

    [HttpDelete("{wordId:guid}")]
    public async Task<IActionResult> Remove(Guid wordId)
        => Ok(await _vocabService.RemoveFlashcardAsync(GetUserId(), wordId));

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
