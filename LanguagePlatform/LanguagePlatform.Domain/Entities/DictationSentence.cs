using LanguagePlatform.Domain.Common;

namespace LanguagePlatform.Domain.Entities;

public class DictationSentence : BaseEntity
{
    public Guid DictationSetId { get; set; }
    public string Sentence { get; set; } = string.Empty;
    public string AudioTitle { get; set; } = string.Empty;
    public string? Hint { get; set; }
    public int Duration { get; set; } // seconds
    public int OrderIndex { get; set; }

    // Navigation
    public DictationSet DictationSet { get; set; } = null!;
}
