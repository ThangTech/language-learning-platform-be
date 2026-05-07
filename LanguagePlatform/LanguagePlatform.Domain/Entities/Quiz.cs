using LanguagePlatform.Domain.Common;

namespace LanguagePlatform.Domain.Entities;

public class Quiz : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    /// <summary>Optional FK to ListeningLesson. null = standalone quiz.</summary>
    public Guid? LessonId { get; set; }

    // Navigation
    public ListeningLesson? Lesson { get; set; }
    public ICollection<QuizQuestion> Questions { get; set; } = new List<QuizQuestion>();
}
