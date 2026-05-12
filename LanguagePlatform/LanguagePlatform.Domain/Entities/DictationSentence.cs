using LanguagePlatform.Domain.Common;

namespace LanguagePlatform.Domain.Entities;

public class DictationSentence : BaseEntity
{
    public Guid DictationSetId { get; set; }

    public string Sentence { get; set; } = string.Empty;

    public string AudioTitle { get; set; } = string.Empty;

    public string? AudioUrl { get; set; }

    public string Hint { get; set; } = string.Empty;

    public int Duration { get; set; }

    public int OrderIndex { get; set; }

    public DictationSet DictationSet { get; set; } = null!;
}
