using System.Security.Claims;
using FluentValidation;
using LanguagePlatform.API.Helpers;
using LanguagePlatform.Application.DTOs.Quiz;
using LanguagePlatform.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanguagePlatform.API.Controllers;

[ApiController]
[Route("api/quizzes")]
public class QuizzesController : ControllerBase
{
    private readonly IQuizService _quizService;
    private readonly IValidator<SubmitQuizRequest> _submitValidator;

    public QuizzesController(
        IQuizService quizService,
        IValidator<SubmitQuizRequest> submitValidator)
    {
        _quizService = quizService;
        _submitValidator = submitValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _quizService.GetQuizzesAsync());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
        => Ok(await _quizService.GetQuizByIdAsync(id));

    [HttpGet("by-lesson/{lessonId:guid}")]
    public async Task<IActionResult> GetByLesson(Guid lessonId)
        => Ok(await _quizService.GetQuizzesByLessonAsync(lessonId));

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateQuizRequest request)
        => Ok(await _quizService.CreateQuizAsync(request));

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateQuizRequest request)
        => Ok(await _quizService.UpdateQuizAsync(id, request));

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
        => Ok(await _quizService.DeleteQuizAsync(id));

    [Authorize]
    [HttpPost("submit")]
    public async Task<IActionResult> Submit([FromBody] SubmitQuizRequest request)
    {
        var invalid = await ValidationHelper.ValidateAsync<SubmitQuizRequest, object>(_submitValidator, request);
        if (invalid != null) return BadRequest(invalid);

        return Ok(await _quizService.SubmitQuizAsync(GetUserId(), request));
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
