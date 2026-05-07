using LanguagePlatform.Domain.Common;

namespace LanguagePlatform.Domain.Entities;

public class ListeningLesson : AuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string AudioUrl { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
    public int Duration { get; set; } // seconds
    public string? TranscriptJson { get; set; } // JSON array of transcript lines

    // Navigation
    public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    public ICollection<DictationSet> DictationSets { get; set; } = new List<DictationSet>();
    public ICollection<ListeningResult> Results { get; set; } = new List<ListeningResult>();
}
