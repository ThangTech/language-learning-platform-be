using LanguagePlatform.Domain.Common;
using LanguagePlatform.Domain.Enums;

namespace LanguagePlatform.Domain.Entities;

public class Word : AuditableEntity
{
    public string Term { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
    public string? Pronunciation { get; set; }
    public string? ExampleSentence { get; set; }
    public string? ImageUrl { get; set; }
    public WordLevel Level { get; set; } = WordLevel.Beginner;
    public string Topic { get; set; } = string.Empty;

    // Navigation
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    public ICollection<Flashcard> Flashcards { get; set; } = new List<Flashcard>();
}
