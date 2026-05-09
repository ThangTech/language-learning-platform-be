using System.Security.Claims;
using FluentValidation;
using LanguagePlatform.Application.DTOs.Common;
using LanguagePlatform.Application.DTOs.Listening;
using LanguagePlatform.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanguagePlatform.API.Controllers;

[ApiController]
[Route("api/listening")]
public class ListeningController : ControllerBase
{
    private readonly IListeningService _listeningService;
    private readonly IValidator<CreateListeningLessonRequest> _createLessonValidator;
    private readonly IValidator<SubmitListeningResultRequest> _submitResultValidator;

    public ListeningController(
        IListeningService listeningService,
        IValidator<CreateListeningLessonRequest> createLessonValidator,
        IValidator<SubmitListeningResultRequest> submitResultValidator)
    {
        _listeningService = listeningService;
        _createLessonValidator = createLessonValidator;
        _submitResultValidator = submitResultValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? level = null,
        [FromQuery] string? search = null)
    {
        var result = await _listeningService.GetLessonsAsync(page, pageSize, level, search);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _listeningService.GetLessonByIdAsync(id);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateListeningLessonRequest request)
    {
        var validationResult = await _createLessonValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(ApiResponse<object>.Fail(errors[0], errors));
        }

        var result = await _listeningService.CreateLessonAsync(request);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateListeningLessonRequest request)
    {
        var result = await _listeningService.UpdateLessonAsync(id, request);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _listeningService.DeleteLessonAsync(id);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("results")]
    public async Task<IActionResult> SubmitResult([FromBody] SubmitListeningResultRequest request)
    {
        var validationResult = await _submitResultValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(ApiResponse<object>.Fail(errors[0], errors));
        }

        Guid userId = GetUserId();
        var result = await _listeningService.SubmitResultAsync(userId, request);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("results/my")]
    public async Task<IActionResult> GetMyResults()
    {
        Guid userId = GetUserId();
        var result = await _listeningService.GetUserResultsAsync(userId);
        return Ok(result);
    }

    private Guid GetUserId()
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(userId!);
    }
}

[ApiController]
[Route("api/dictation")]
public class DictationController : ControllerBase
{
    private readonly IListeningService _listeningService;

    public DictationController(IListeningService listeningService)
    {
        _listeningService = listeningService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _listeningService.GetDictationSetsAsync();
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _listeningService.GetDictationSetByIdAsync(id);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDictationSetRequest request)
    {
        var result = await _listeningService.CreateDictationSetAsync(request);
        return Ok(result);
    }
}
