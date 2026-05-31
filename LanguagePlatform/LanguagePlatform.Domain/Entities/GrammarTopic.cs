using LanguagePlatform.Domain.Common;
using LanguagePlatform.Domain.Enums;

namespace LanguagePlatform.Domain.Entities;

public class GrammarTopic : AuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Explanation { get; set; }
    public string? Examples { get; set; }
    public GrammarLevel Level { get; set; } = GrammarLevel.Beginner;
    public string? YouTubeUrl { get; set; }

    // Navigation
    public ICollection<UserGrammar> UserGrammars { get; set; } = new List<UserGrammar>();
}
