using Shared.Common.Models;

namespace ListeningService.Models;

/// <summary>
/// Câu hỏi quiz thuộc một bài học.
/// Hỗ trợ: MULTIPLE_CHOICE | MAIN_IDEA | INFERENCE | FILL_IN_BLANK | DICTATION
/// </summary>
public class QuizList : BaseEntity
{
    public Guid QuizId { get; set; }

    public string QuestionText { get; set; } = string.Empty;

    /// <summary>
    /// Loại câu hỏi:
    /// "MULTIPLE_CHOICE" | "MAIN_IDEA" | "INFERENCE" | "FILL_IN_BLANK" | "DICTATION"
    /// </summary>
    public string QuestionType { get; set; } = "MULTIPLE_CHOICE";

    /// <summary>
    /// Câu có chỗ trống, VD: "The meeting is at ___ o'clock".
    /// Chỉ dùng khi QuestionType = FILL_IN_BLANK.
    /// </summary>
    public string? BlankText { get; set; }

    /// <summary>Đáp án đúng cho dạng điền từ.</summary>
    public string? ExpectedAnswer { get; set; }

    // ── Navigation ──────────────────────────────────────────────────────────
    public Quiz Quiz { get; set; } = null!;
}
