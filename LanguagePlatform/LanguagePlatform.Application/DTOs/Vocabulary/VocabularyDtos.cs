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

    public string Topic { get; set; } = string.Empty;

    public string Category => Topic;

    public DateTime CreatedAt { get; set; }

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
}
