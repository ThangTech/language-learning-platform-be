using LanguagePlatform.Domain.Common;

namespace LanguagePlatform.Domain.Entities;

public class QuizResult : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid QuizId { get; set; }
    public int Score { get; set; }         // % đúng (0-100)
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public User User { get; set; } = null!;
    public Quiz Quiz { get; set; } = null!;
}
