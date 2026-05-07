using LanguagePlatform.Domain.Common;
using LanguagePlatform.Domain.Enums;

namespace LanguagePlatform.Domain.Entities;

public class QuizQuestion : BaseEntity
{
    public Guid QuizId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public QuestionType QuestionType { get; set; } = QuestionType.MultipleChoice;
    /// <summary>JSON array of options for MultipleChoice questions.</summary>
    public string? OptionsJson { get; set; }
    /// <summary>Sentence with blank for FillInBlank type.</summary>
    public string? BlankText { get; set; }
    /// <summary>Expected answer for FillInBlank / Dictation types.</summary>
    public string? ExpectedAnswer { get; set; }
    public int OrderIndex { get; set; }

    // Navigation
    public Quiz Quiz { get; set; } = null!;
}
