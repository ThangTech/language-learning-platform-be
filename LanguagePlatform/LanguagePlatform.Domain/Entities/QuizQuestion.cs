using LanguagePlatform.Domain.Common;
using LanguagePlatform.Domain.Enums;

namespace LanguagePlatform.Domain.Entities;

public class QuizQuestion : BaseEntity
{
    public Guid QuizId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public QuestionType Type { get; set; } = QuestionType.MultipleChoice;
    public List<string> Options { get; set; } = new();
    public string CorrectAnswer { get; set; } = string.Empty;
    public string? Explanation { get; set; }
    public string? AudioUrl { get; set; }

    // Navigation
    public Quiz Quiz { get; set; } = null!;
}
