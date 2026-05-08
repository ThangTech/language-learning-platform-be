using System.Security.Claims;
using LanguagePlatform.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanguagePlatform.API.Controllers;

[ApiController]
[Route("api/stats")]
[Authorize]
public class StatsController : ControllerBase
{
    private readonly IProgressService _progressService;
    public StatsController(IProgressService progressService) => _progressService = progressService;

    [HttpGet]
    public async Task<IActionResult> GetStats()
        => Ok(await _progressService.GetStatsAsync(GetUserId()));

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}

[ApiController]
[Route("api/streaks")]
[Authorize]
public class StreaksController : ControllerBase
{
    private readonly IProgressService _progressService;
    public StreaksController(IProgressService progressService) => _progressService = progressService;

    [HttpGet]
    public async Task<IActionResult> GetStreak()
        => Ok(await _progressService.GetStreakAsync(GetUserId()));

    [HttpPost("update")]
    public async Task<IActionResult> UpdateStreak()
        => Ok(await _progressService.UpdateStreakAsync(GetUserId()));

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}

[ApiController]
[Route("api/leaderboard")]
public class LeaderboardController : ControllerBase
{
    private readonly IProgressService _progressService;
    public LeaderboardController(IProgressService progressService) => _progressService = progressService;

    [HttpGet]
    public async Task<IActionResult> GetLeaderboard([FromQuery] int top = 10)
        => Ok(await _progressService.GetLeaderboardAsync(top));
}
