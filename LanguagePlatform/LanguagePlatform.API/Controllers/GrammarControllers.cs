using System.Security.Claims;
using FluentValidation;
using LanguagePlatform.API.Helpers;
using LanguagePlatform.Application.DTOs.Grammar;
using LanguagePlatform.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanguagePlatform.API.Controllers;

[ApiController]
[Route("api/grammar")]
public class GrammarController : ControllerBase
{
    private readonly IGrammarService _grammarService;
    private readonly IValidator<CreateGrammarTopicRequest> _createValidator;
    private readonly IValidator<UpdateGrammarTopicRequest> _updateValidator;

    public GrammarController(
        IGrammarService grammarService,
        IValidator<CreateGrammarTopicRequest> createValidator,
        IValidator<UpdateGrammarTopicRequest> updateValidator)
    {
        _grammarService = grammarService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
        [FromQuery] string? level = null, [FromQuery] string? search = null)
        => Ok(await _grammarService.GetTopicsAsync(page, pageSize, level, search));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
        => Ok(await _grammarService.GetTopicByIdAsync(id));

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGrammarTopicRequest request)
    {
        var invalid = await ValidationHelper.ValidateAsync<CreateGrammarTopicRequest, object>(_createValidator, request);
        if (invalid != null) return BadRequest(invalid);

        return Ok(await _grammarService.CreateTopicAsync(request));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGrammarTopicRequest request)
    {
        var invalid = await ValidationHelper.ValidateAsync<UpdateGrammarTopicRequest, object>(_updateValidator, request);
        if (invalid != null) return BadRequest(invalid);

        return Ok(await _grammarService.UpdateTopicAsync(id, request));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
        => Ok(await _grammarService.DeleteTopicAsync(id));
}

[ApiController]
[Route("api/user-grammar")]
[Authorize]
public class UserGrammarController : ControllerBase
{
    private readonly IGrammarService _grammarService;
    public UserGrammarController(IGrammarService grammarService) => _grammarService = grammarService;

    [HttpGet]
    public async Task<IActionResult> GetProgress()
        => Ok(await _grammarService.GetUserGrammarProgressAsync(GetUserId()));

    [HttpPost("{topicId:guid}/complete")]
    public async Task<IActionResult> MarkCompleted(Guid topicId)
        => Ok(await _grammarService.MarkTopicCompletedAsync(GetUserId(), topicId));

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
