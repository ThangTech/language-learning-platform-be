using System.Security.Claims;
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
    public ListeningController(IListeningService listeningService) => _listeningService = listeningService;

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
        [FromQuery] string? level = null, [FromQuery] string? search = null)
        => Ok(await _listeningService.GetLessonsAsync(page, pageSize, level, search));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
        => Ok(await _listeningService.GetLessonByIdAsync(id));

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateListeningLessonRequest request)
        => Ok(await _listeningService.CreateLessonAsync(request));

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateListeningLessonRequest request)
        => Ok(await _listeningService.UpdateLessonAsync(id, request));

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
        => Ok(await _listeningService.DeleteLessonAsync(id));

    [Authorize]
    [HttpPost("results")]
    public async Task<IActionResult> SubmitResult([FromBody] SubmitListeningResultRequest request)
        => Ok(await _listeningService.SubmitResultAsync(GetUserId(), request));

    [Authorize]
    [HttpGet("results/my")]
    public async Task<IActionResult> GetMyResults()
        => Ok(await _listeningService.GetUserResultsAsync(GetUserId()));

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}

[ApiController]
[Route("api/dictation")]
public class DictationController : ControllerBase
{
    private readonly IListeningService _listeningService;
    public DictationController(IListeningService listeningService) => _listeningService = listeningService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _listeningService.GetDictationSetsAsync());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
        => Ok(await _listeningService.GetDictationSetByIdAsync(id));

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDictationSetRequest request)
        => Ok(await _listeningService.CreateDictationSetAsync(request));
}
