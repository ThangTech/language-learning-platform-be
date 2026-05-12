namespace LanguagePlatform.Application.DTOs.Progress;

public class UserProgressDto
{
    public Guid UserId { get; set; }

    public int TotalScore { get; set; }

    public string ScoreText
    {
        get
        {
            return $"{TotalScore} điểm";
        }
    }

    public int CurrentStreak { get; set; }

    public string CurrentStreakText
    {
        get
        {
            return $"{CurrentStreak} ngày";
        }
    }

    public int LongestStreak { get; set; }

    public string LongestStreakText
    {
        get
        {
            return $"{LongestStreak} ngày";
        }
    }

    public DateTime? LastActivityDate { get; set; }

    public int WordsLearned { get; set; }

    public int GrammarCompleted { get; set; }

    public int ListeningCompleted { get; set; }

    public int QuizzesCompleted { get; set; }

    public int TotalCompleted
    {
        get
        {
            return GrammarCompleted + ListeningCompleted + QuizzesCompleted;
        }
    }
}

public class LeaderboardEntryDto
{
    public Guid UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string? AvatarUrl { get; set; }

    public int TotalScore { get; set; }

    public string ScoreText
    {
        get
        {
            return $"{TotalScore} điểm";
        }
    }

    public int CurrentStreak { get; set; }

    public string StreakText
    {
        get
        {
            return $"{CurrentStreak} ngày";
        }
    }

    public int Rank { get; set; }

    public string RankSuffix
    {
        get
        {
            if (Rank == 1)
            {
                return "st";
            }

            if (Rank == 2)
            {
                return "nd";
            }

            if (Rank == 3)
            {
                return "rd";
            }

            return "th";
        }
    }
}

public class StreakDto
{
    public int CurrentStreak { get; set; }

    public string CurrentStreakText
    {
        get
        {
            return $"{CurrentStreak} ngày";
        }
    }

    public int LongestStreak { get; set; }

    public string LongestStreakText
    {
        get
        {
            return $"{LongestStreak} ngày";
        }
    }

    public DateTime? LastActivityDate { get; set; }

    public bool HasStudiedToday
    {
        get
        {
            return LastActivityDate?.Date == DateTime.UtcNow.Date;
        }
    }
}
