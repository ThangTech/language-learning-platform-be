namespace LanguagePlatform.Application.DTOs.Progress;

public class UserProgressDto
{
    public Guid UserId { get; set; }
    public int TotalScore { get; set; }
    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public DateTime? LastActivityDate { get; set; }
    public int WordsLearned { get; set; }
    public int GrammarCompleted { get; set; }
    public int ListeningCompleted { get; set; }
    public int QuizzesCompleted { get; set; }
}

public class LeaderboardEntryDto
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int TotalScore { get; set; }
    public int CurrentStreak { get; set; }
    public int Rank { get; set; }
}

public class StreakDto
{
    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public DateTime? LastActivityDate { get; set; }
}
