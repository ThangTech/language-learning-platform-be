using Shared.Common.Models;

namespace ListeningService.Models;

/// <summary>
/// Bộ quiz gắn với một bài học nghe (có thể null nếu quiz độc lập).
/// </summary>
public class Quiz : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// FK tuỳ chọn tới ListeningLesson.
    /// null = quiz độc lập, không gắn bài nghe.
    /// </summary>
    public Guid? LessonId { get; set; }

    // ── Navigation ──────────────────────────────────────────────────────────
    public ListeningLesson? Lesson { get; set; }
    public ICollection<QuizList> Questions { get; set; } = new List<QuizList>();
}
