using LanguagePlatform.Domain.Common;

namespace LanguagePlatform.Domain.Entities;

public class UserProgress : BaseEntity
{
    public Guid UserId { get; set; }
    public int TotalScore { get; set; } = 0;
    public int CurrentStreak { get; set; } = 0;
    public int LongestStreak { get; set; } = 0;
    public DateTime? LastActivityDate { get; set; }
    public int WordsLearned { get; set; } = 0;
    public int GrammarCompleted { get; set; } = 0;
    public int ListeningCompleted { get; set; } = 0;
    public int QuizzesCompleted { get; set; } = 0;

    // Navigation
    public User User { get; set; } = null!;
}
