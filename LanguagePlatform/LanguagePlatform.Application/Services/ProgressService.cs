using AutoMapper;
using LanguagePlatform.Application.DTOs.Common;
using LanguagePlatform.Application.DTOs.Progress;
using LanguagePlatform.Application.Interfaces;
using LanguagePlatform.Domain.Entities;
using LanguagePlatform.Domain.Interfaces;

namespace LanguagePlatform.Application.Services;

public class ProgressService : IProgressService
{
    private readonly IProgressRepository _progressRepo;
    private readonly IMapper _mapper;

    public ProgressService(IProgressRepository progressRepo, IMapper mapper)
    {
        _progressRepo = progressRepo;
        _mapper = mapper;
    }

    public async Task<ApiResponse<UserProgressDto>> GetStatsAsync(Guid userId)
    {
        var progress = await _progressRepo.GetByUserIdAsync(userId);
        if (progress == null)
        {
            progress = await CreateDefaultProgressAsync(userId);
        }
        return ApiResponse<UserProgressDto>.Ok(_mapper.Map<UserProgressDto>(progress));
    }

    public async Task<ApiResponse<StreakDto>> GetStreakAsync(Guid userId)
    {
        var progress = await _progressRepo.GetByUserIdAsync(userId);
        if (progress == null) progress = await CreateDefaultProgressAsync(userId);
        return ApiResponse<StreakDto>.Ok(_mapper.Map<StreakDto>(progress));
    }

    public async Task<ApiResponse<bool>> UpdateStreakAsync(Guid userId)
    {
        var progress = await _progressRepo.GetByUserIdAsync(userId);
        if (progress == null) progress = await CreateDefaultProgressAsync(userId);

        var today = DateTime.UtcNow.Date;
        var lastActivity = progress.LastActivityDate?.Date;

        if (lastActivity == today) return ApiResponse<bool>.Ok(true, "Streak đã được cập nhật hôm nay.");
        if (lastActivity == today.AddDays(-1))
            progress.CurrentStreak++;
        else
            progress.CurrentStreak = 1;

        if (progress.CurrentStreak > progress.LongestStreak)
            progress.LongestStreak = progress.CurrentStreak;

        progress.LastActivityDate = DateTime.UtcNow;
        _progressRepo.Update(progress);
        await _progressRepo.SaveChangesAsync();
        return ApiResponse<bool>.Ok(true, "Cập nhật streak thành công.");
    }

    public async Task<ApiResponse<IEnumerable<LeaderboardEntryDto>>> GetLeaderboardAsync(int top = 10)
    {
        var items = await _progressRepo.GetTopLeaderboardAsync(top);
        var result = items.Select((p, i) => new LeaderboardEntryDto
        {
            Rank = i + 1,
            UserId = p.UserId,
            FullName = p.User?.FullName ?? "Unknown",
            AvatarUrl = p.User?.AvatarUrl,
            TotalScore = p.TotalScore,
            CurrentStreak = p.CurrentStreak
        });
        return ApiResponse<IEnumerable<LeaderboardEntryDto>>.Ok(result);
    }

    public async Task<ApiResponse<bool>> AddScoreAsync(Guid userId, int score)
    {
        var progress = await _progressRepo.GetByUserIdAsync(userId);
        if (progress == null)
        {
            progress = await CreateDefaultProgressAsync(userId);
        }

        progress.TotalScore += score;
        _progressRepo.Update(progress);
        await _progressRepo.SaveChangesAsync();

        return ApiResponse<bool>.Ok(true, $"Cộng {score} điểm thành công.");
    }

    public async Task<ApiResponse<bool>> RecordCompletionAsync(Guid userId, string activityType, int score = 0)
    {
        var progress = await _progressRepo.GetByUserIdAsync(userId);
        if (progress == null)
        {
            progress = await CreateDefaultProgressAsync(userId);
        }

        AddCompletionCount(progress, activityType);

        if (score > 0)
        {
            progress.TotalScore += score;
        }

        UpdateActivityStreak(progress);

        _progressRepo.Update(progress);
        await _progressRepo.SaveChangesAsync();

        return ApiResponse<bool>.Ok(true, "Đã ghi nhận tiến độ học tập.");
    }

    private static void AddCompletionCount(UserProgress progress, string activityType)
    {
        var type = activityType.ToLowerInvariant();

        if (type == "word")
        {
            progress.WordsLearned++;
            return;
        }

        if (type == "grammar")
        {
            progress.GrammarCompleted++;
            return;
        }

        if (type == "listening")
        {
            progress.ListeningCompleted++;
            return;
        }

        if (type == "quiz")
        {
            progress.QuizzesCompleted++;
        }
    }

    private static void UpdateActivityStreak(UserProgress progress)
    {
        var today = DateTime.UtcNow.Date;
        var lastActivity = progress.LastActivityDate?.Date;

        if (lastActivity != today)
        {
            if (lastActivity == today.AddDays(-1))
            {
                progress.CurrentStreak++;
            }
            else
            {
                progress.CurrentStreak = 1;
            }
        }

        if (progress.CurrentStreak > progress.LongestStreak)
        {
            progress.LongestStreak = progress.CurrentStreak;
        }

        progress.LastActivityDate = DateTime.UtcNow;
    }

    private async Task<UserProgress> CreateDefaultProgressAsync(Guid userId)
    {
        var progress = new UserProgress { UserId = userId };
        await _progressRepo.AddAsync(progress);
        await _progressRepo.SaveChangesAsync();
        return progress;
    }
}
