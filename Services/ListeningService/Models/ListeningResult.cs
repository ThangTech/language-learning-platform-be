using Shared.Common.Models;

namespace ListeningService.Models;

/// <summary>
/// Kết quả làm bài nghe (quiz) của người dùng.
/// </summary>
public class ListeningResult : BaseEntity
{
    public Guid UserId { get; set; }

    public Guid LessonId { get; set; }

    /// <summary>Điểm (0 – 100 hoặc số câu đúng tuỳ quy ước).</summary>
    public int Score { get; set; }

    /// <summary>Thời gian làm bài tính bằng giây.</summary>
    public int TimeTaken { get; set; }

    /// <summary>Số lần người dùng nhấn Play.</summary>
    public int ListenCount { get; set; }

    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

    // ── Navigation ──────────────────────────────────────────────────────────
    public ListeningLesson Lesson { get; set; } = null!;
}
