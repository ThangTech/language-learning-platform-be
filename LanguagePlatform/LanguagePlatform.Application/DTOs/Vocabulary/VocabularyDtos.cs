namespace LanguagePlatform.Application.DTOs.Vocabulary;

public class WordDto
{
    public Guid Id { get; set; }

    public string Term { get; set; } = string.Empty;

    public string Word => Term;

    public string Definition { get; set; } = string.Empty;

    public string? Pronunciation { get; set; }

    public string? ExampleSentence { get; set; }

    public string Example => ExampleSentence ?? string.Empty;

    public string? ImageUrl { get; set; }

    public string Level { get; set; } = string.Empty;

    public string LevelLabel => Level;

    public string LevelColor => GetLevelColor();

    public List<WordLevelDto> Levels => new()
    {
        new WordLevelDto
        {
            Label = GetShortLevel(),
            BgColor = GetLevelBgColor(),
            TextColor = GetLevelTextColor()
        }
    };

    public string Topic { get; set; } = string.Empty;

    public string Category => Topic;

    public DateTime CreatedAt { get; set; }

    private string GetShortLevel()
    {
        return Level switch
        {
            "Beginner" => "A1",
            "Intermediate" => "B1",
            "Advanced" => "B2",
            _ => Level
        };
    }

    private string GetLevelColor()
    {
        return Level switch
        {
            "Beginner" => "bg-primary/10 text-primary",
            "Intermediate" => "bg-tertiary/10 text-tertiary",
            "Advanced" => "bg-error/10 text-error",
            _ => "bg-outline text-outline"
        };
    }

    private string GetLevelBgColor()
    {
        return Level switch
        {
            "Beginner" => "bg-primary-fixed",
            "Intermediate" => "bg-secondary-fixed",
            "Advanced" => "bg-tertiary-fixed",
            _ => "bg-surface-container"
        };
    }

    private string GetLevelTextColor()
    {
        return Level switch
        {
            "Beginner" => "text-on-primary-fixed",
            "Intermediate" => "text-on-secondary-fixed",
            "Advanced" => "text-on-tertiary-fixed",
            _ => "text-on-surface"
        };
    }
}

public class WordLevelDto
{
    public string Label { get; set; } = string.Empty;

    public string BgColor { get; set; } = string.Empty;

    public string TextColor { get; set; } = string.Empty;
}

public class CreateWordRequest
{
    public string Term { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
    public string? Pronunciation { get; set; }
    public string? ExampleSentence { get; set; }
    public string? ImageUrl { get; set; }
    public string Level { get; set; } = "Beginner";
    public string Topic { get; set; } = string.Empty;
}

public class UpdateWordRequest
{
    public string Term { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
    public string? Pronunciation { get; set; }
    public string? ExampleSentence { get; set; }
    public string? ImageUrl { get; set; }
    public string Level { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
}

public class FavoriteDto
{
    public Guid Id { get; set; }
    public Guid WordId { get; set; }
    public WordDto Word { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}

public class FlashcardDto
{
    public Guid Id { get; set; }
    public Guid WordId { get; set; }
    public WordDto Word { get; set; } = null!;
    public bool IsLearned { get; set; }
    public int ReviewCount { get; set; }
    public DateTime? NextReviewAt { get; set; }
    public DateTime? LearnedAt { get; set; }
}
