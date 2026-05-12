using LanguagePlatform.Domain.Common;
using LanguagePlatform.Domain.Enums;

namespace LanguagePlatform.Domain.Entities;

public class Quiz : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    /// <summary>Optional FK to ListeningLesson. null = standalone quiz.</summary>
    public Guid? LessonId { get; set; }

    public QuizDifficulty Difficulty { get; set; } = QuizDifficulty.Medium;

    public QuizType Type { get; set; } = QuizType.MultipleChoice;

    /// <summary>Duration in minutes</summary>
    public int DurationMinutes { get; set; }

    // Navigation
    public ListeningLesson? Lesson { get; set; }
    public ICollection<QuizQuestion> Questions { get; set; } = new List<QuizQuestion>();
}
