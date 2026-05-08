using LanguagePlatform.Domain.Common;

namespace LanguagePlatform.Domain.Entities;

public class DictationSentence : BaseEntity
{
    public Guid DictationSetId { get; set; }
    public string Sentence { get; set; } = string.Empty;
    public string? AudioUrl { get; set; }
    public int OrderIndex { get; set; }

    // Navigation
    public DictationSet DictationSet { get; set; } = null!;
}
