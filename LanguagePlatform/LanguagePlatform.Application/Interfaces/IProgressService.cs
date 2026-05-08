using LanguagePlatform.Application.DTOs.Common;
using LanguagePlatform.Application.DTOs.Progress;

namespace LanguagePlatform.Application.Interfaces;

public interface IProgressService
{
    Task<ApiResponse<UserProgressDto>> GetStatsAsync(Guid userId);
    Task<ApiResponse<StreakDto>> GetStreakAsync(Guid userId);
    Task<ApiResponse<bool>> UpdateStreakAsync(Guid userId);
    Task<ApiResponse<IEnumerable<LeaderboardEntryDto>>> GetLeaderboardAsync(int top = 10);
    Task<ApiResponse<bool>> AddScoreAsync(Guid userId, int score);
}
