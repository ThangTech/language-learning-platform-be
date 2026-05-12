using LanguagePlatform.Domain.Common;

namespace LanguagePlatform.Domain.Entities;

public class DictationSet : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Level { get; set; } = string.Empty;

    public string Topic { get; set; } = string.Empty;

    public Guid? LessonId { get; set; }

    public ListeningLesson? Lesson { get; set; }

    public ICollection<DictationSentence> Sentences { get; set; } = new List<DictationSentence>();
}
