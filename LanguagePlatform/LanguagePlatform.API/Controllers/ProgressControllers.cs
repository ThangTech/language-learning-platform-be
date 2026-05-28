using System.Security.Claims;
using LanguagePlatform.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanguagePlatform.API.Controllers;

// Controller trả về thống kê học tập của người dùng (số từ đã học, bài đã làm...)
[ApiController]
[Route("api/stats")]
[Authorize]
public class StatsController : ApiControllerBase
{
    private readonly IProgressService _progressService;

    public StatsController(IProgressService progressService)
    {
        _progressService = progressService;
    }

    // Lấy thống kê tổng quan: số từ đã học, số bài listening hoàn thành, v.v.
    [HttpGet]
    public async Task<IActionResult> GetStats()
    {
        Guid userId = GetUserId();
        var result = await _progressService.GetStatsAsync(userId);
        return HandleResult(result);
    }
}

// Controller quản lý chuỗi ngày học liên tiếp (streak)
[ApiController]
[Route("api/streaks")]
[Authorize]
public class StreaksController : ApiControllerBase
{
    private readonly IProgressService _progressService;

    public StreaksController(IProgressService progressService)
    {
        _progressService = progressService;
    }

    // Lấy thông tin streak hiện tại (đã học bao nhiêu ngày liên tiếp)
    [HttpGet]
    public async Task<IActionResult> GetStreak()
    {
        Guid userId = GetUserId();
        var result = await _progressService.GetStreakAsync(userId);
        return HandleResult(result);
    }

    // Cập nhật streak khi người dùng học xong trong ngày
    [HttpPost("update")]
    public async Task<IActionResult> UpdateStreak()
    {
        Guid userId = GetUserId();
        var result = await _progressService.UpdateStreakAsync(userId);
        return HandleResult(result);
    }
}

// Controller bảng xếp hạng - ai học nhiều nhất
[ApiController]
[Route("api/leaderboard")]
public class LeaderboardController : ApiControllerBase
{
    private readonly IProgressService _progressService;

    public LeaderboardController(IProgressService progressService)
    {
        _progressService = progressService;
    }

    // Lấy top N người dùng có điểm cao nhất
    [HttpGet]
    public async Task<IActionResult> GetLeaderboard([FromQuery] int top = 10)
    {
        var result = await _progressService.GetLeaderboardAsync(top);
        return HandleResult(result);
    }
}
