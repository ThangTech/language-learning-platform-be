using Shared.Common.Models;

namespace ListeningService.Models;

/// <summary>
/// Bài học nghe tiếng Anh.
/// </summary>
public class ListeningLesson : AuditableEntity
{
    public string Title { get; set; } = string.Empty;

    /// <summary>URL file audio (CDN / Azure Blob / …)</summary>
    public string AudioUrl { get; set; } = string.Empty;

    /// <summary>Transcript gốc để chấm dictation.</summary>
    public string? Transcript { get; set; }

    /// <summary>"Beginner" | "Intermediate" | "Advanced"</summary>
    public string? Level { get; set; }

    /// <summary>1 = thông báo ngắn | 2 = hội thoại | 3 = độc thoại</summary>
    public int Part { get; set; }

    /// <summary>Thời lượng tính bằng giây.</summary>
    public int? Duration { get; set; }

    // ── Navigation ──────────────────────────────────────────────────────────
    public ICollection<ListeningResult> Results { get; set; } = new List<ListeningResult>();
    public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    public ICollection<DictationResult> DictationResults { get; set; } = new List<DictationResult>();
}
