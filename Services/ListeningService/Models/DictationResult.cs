using Shared.Common.Models;

namespace ListeningService.Models;

/// <summary>
/// Kết quả chép chính tả (dictation) của người dùng.
/// </summary>
public class DictationResult : BaseEntity
{
    public Guid UserId { get; set; }

    public Guid LessonId { get; set; }

    /// <summary>Đoạn văn người dùng tự chép.</summary>
    public string UserTranscript { get; set; } = string.Empty;

    /// <summary>% từ chính xác so với Transcript gốc (0.0 – 100.0).</summary>
    public double Accuracy { get; set; }

    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

    // ── Navigation ──────────────────────────────────────────────────────────
    public ListeningLesson Lesson { get; set; } = null!;
}
