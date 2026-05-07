using LanguagePlatform.Domain.Common;

namespace LanguagePlatform.Domain.Entities;

public class ListeningResult : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid LessonId { get; set; }
    public int Score { get; set; }
    public int TimeTaken { get; set; } // seconds
    public int ListenCount { get; set; }
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ListeningLesson Lesson { get; set; } = null!;
}
