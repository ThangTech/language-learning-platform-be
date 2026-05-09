using System.Security.Claims;
using FluentValidation;
using LanguagePlatform.Application.DTOs.Common;
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
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? level = null,
        [FromQuery] string? search = null)
    {
        var result = await _grammarService.GetTopicsAsync(page, pageSize, level, search);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _grammarService.GetTopicByIdAsync(id);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGrammarTopicRequest request)
    {
        var ketQua = await _createValidator.ValidateAsync(request);
        if (!ketQua.IsValid)
        {
            var danhSachLoi = ketQua.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(ApiResponse<object>.Fail(danhSachLoi[0], danhSachLoi));
        }

        var result = await _grammarService.CreateTopicAsync(request);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGrammarTopicRequest request)
    {
        var ketQua = await _updateValidator.ValidateAsync(request);
        if (!ketQua.IsValid)
        {
            var danhSachLoi = ketQua.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(ApiResponse<object>.Fail(danhSachLoi[0], danhSachLoi));
        }

        var result = await _grammarService.UpdateTopicAsync(id, request);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _grammarService.DeleteTopicAsync(id);
        return Ok(result);
    }
}

[ApiController]
[Route("api/user-grammar")]
[Authorize]
public class UserGrammarController : ControllerBase
{
    private readonly IGrammarService _grammarService;

    public UserGrammarController(IGrammarService grammarService)
    {
        _grammarService = grammarService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProgress()
    {
        Guid userId = GetUserId();
        var result = await _grammarService.GetUserGrammarProgressAsync(userId);
        return Ok(result);
    }

    [HttpPost("{topicId:guid}/complete")]
    public async Task<IActionResult> MarkCompleted(Guid topicId)
    {
        Guid userId = GetUserId();
        var result = await _grammarService.MarkTopicCompletedAsync(userId, topicId);
        return Ok(result);
    }

    private Guid GetUserId()
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(userId!);
    }
}
