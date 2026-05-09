using System.Security.Claims;
using FluentValidation;
using LanguagePlatform.Application.DTOs.Common;
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
    {
        var result = await _quizService.GetQuizzesAsync();
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _quizService.GetQuizByIdAsync(id);
        return Ok(result);
    }

    [HttpGet("by-lesson/{lessonId:guid}")]
    public async Task<IActionResult> GetByLesson(Guid lessonId)
    {
        var result = await _quizService.GetQuizzesByLessonAsync(lessonId);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateQuizRequest request)
    {
        var result = await _quizService.CreateQuizAsync(request);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateQuizRequest request)
    {
        var result = await _quizService.UpdateQuizAsync(id, request);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _quizService.DeleteQuizAsync(id);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("submit")]
    public async Task<IActionResult> Submit([FromBody] SubmitQuizRequest request)
    {
        // Kiểm tra dữ liệu: phải có quiz ID và ít nhất 1 câu trả lời
        var ketQua = await _submitValidator.ValidateAsync(request);
        if (!ketQua.IsValid)
        {
            var danhSachLoi = ketQua.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(ApiResponse<object>.Fail(danhSachLoi[0], danhSachLoi));
        }

        Guid userId = GetUserId();
        var result = await _quizService.SubmitQuizAsync(userId, request);
        return Ok(result);
    }

    private Guid GetUserId()
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(userId!);
    }
}
