using System.Security.Claims;
using FluentValidation;
using LanguagePlatform.Application.DTOs.Common;
using LanguagePlatform.Application.DTOs.Vocabulary;
using LanguagePlatform.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanguagePlatform.API.Controllers;

[ApiController]
[Route("api/words")]
public class WordsController : ApiControllerBase
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
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? level = null,
        [FromQuery] string? search = null)
    {
        var result = await _vocabService.GetWordsAsync(page, pageSize, level, search);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _vocabService.GetWordByIdAsync(id);
        return HandleResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWordRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            var errorResponse = ApiErrorResponse.Fail(errors[0], errors);
            return BadRequest(errorResponse);
        }

        var result = await _vocabService.CreateWordAsync(request);
        return HandleResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateWordRequest request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            var errorResponse = ApiErrorResponse.Fail(errors[0], errors);
            return BadRequest(errorResponse);
        }

        var result = await _vocabService.UpdateWordAsync(id, request);
        return HandleResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _vocabService.DeleteWordAsync(id);
        return HandleResult(result);
    }
}

[ApiController]
[Route("api/favorites")]
[Authorize]
public class FavoritesController : ApiControllerBase
{
    private readonly IVocabularyService _vocabService;

    public FavoritesController(IVocabularyService vocabService)
    {
        _vocabService = vocabService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        Guid userId = GetUserId();
        var result = await _vocabService.GetFavoritesAsync(userId);
        return HandleResult(result);
    }

    [HttpPost("{wordId:guid}")]
    public async Task<IActionResult> Add(Guid wordId)
    {
        Guid userId = GetUserId();
        var result = await _vocabService.AddFavoriteAsync(userId, wordId);
        return HandleResult(result);
    }

    [HttpDelete("{wordId:guid}")]
    public async Task<IActionResult> Remove(Guid wordId)
    {
        Guid userId = GetUserId();
        var result = await _vocabService.RemoveFavoriteAsync(userId, wordId);
        return HandleResult(result);
    }
}

[ApiController]
[Route("api/flashcards")]
[Authorize]
public class FlashcardsController : ApiControllerBase
{
    private readonly IVocabularyService _vocabService;

    public FlashcardsController(IVocabularyService vocabService)
    {
        _vocabService = vocabService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        Guid userId = GetUserId();
        var result = await _vocabService.GetFlashcardsAsync(userId);
        return HandleResult(result);
    }

    [HttpGet("review")]
    public async Task<IActionResult> GetReviewable()
    {
        Guid userId = GetUserId();
        var result = await _vocabService.GetReviewableFlashcardsAsync(userId);
        return HandleResult(result);
    }

    [HttpPost("{wordId:guid}")]
    public async Task<IActionResult> Add(Guid wordId)
    {
        Guid userId = GetUserId();
        var result = await _vocabService.AddToFlashcardAsync(userId, wordId);
        return HandleResult(result);
    }

    [HttpPut("{wordId:guid}/learned")]
    public async Task<IActionResult> MarkLearned(Guid wordId)
    {
        Guid userId = GetUserId();
        var result = await _vocabService.MarkFlashcardLearnedAsync(userId, wordId);
        return HandleResult(result);
    }

    [HttpPut("{wordId:guid}/reviewed")]
    public async Task<IActionResult> MarkReviewed(Guid wordId)
    {
        Guid userId = GetUserId();
        var result = await _vocabService.MarkFlashcardAsReviewedAsync(userId, wordId);
        return HandleResult(result);
    }

    [HttpDelete("{wordId:guid}")]
    public async Task<IActionResult> Remove(Guid wordId)
    {
        Guid userId = GetUserId();
        var result = await _vocabService.RemoveFlashcardAsync(userId, wordId);
        return HandleResult(result);
    }
}
