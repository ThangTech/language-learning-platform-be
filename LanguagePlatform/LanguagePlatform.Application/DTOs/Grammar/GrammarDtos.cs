namespace LanguagePlatform.Application.DTOs.Grammar;

public class GrammarTopicDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Explanation { get; set; }
    public string? Examples { get; set; }
    public string Level { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateGrammarTopicRequest
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Explanation { get; set; }
    public string? Examples { get; set; }
    public string Level { get; set; } = "Beginner";
}

public class UpdateGrammarTopicRequest
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Explanation { get; set; }
    public string? Examples { get; set; }
    public string Level { get; set; } = string.Empty;
}

public class UserGrammarDto
{
    public Guid Id { get; set; }
    public Guid TopicId { get; set; }
    public GrammarTopicDto Topic { get; set; } = null!;
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
}
