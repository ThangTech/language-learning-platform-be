namespace LanguagePlatform.Application.DTOs.Listening;

public class ListeningLessonDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string AudioUrl { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
    public int Duration { get; set; }
    public string? TranscriptJson { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateListeningLessonRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string AudioUrl { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
    public int Duration { get; set; }
    public string? TranscriptJson { get; set; }
}

public class UpdateListeningLessonRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string AudioUrl { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
    public int Duration { get; set; }
    public string? TranscriptJson { get; set; }
}

public class DictationSetDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
    public List<DictationSentenceDto> Sentences { get; set; } = new();
}

public class DictationSentenceDto
{
    public Guid Id { get; set; }
    public string Sentence { get; set; } = string.Empty;
    public string AudioTitle { get; set; } = string.Empty;
    public string? AudioUrl { get; set; }
    public string Hint { get; set; } = string.Empty;
    public int Duration { get; set; }
    public int OrderIndex { get; set; }
}

public class CreateDictationSetRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
    public List<CreateDictationSentenceRequest> Sentences { get; set; } = new();
}

public class CreateDictationSentenceRequest
{
    public string Sentence { get; set; } = string.Empty;
    public string AudioTitle { get; set; } = string.Empty;
    public string? AudioUrl { get; set; }
    public string Hint { get; set; } = string.Empty;
    public int Duration { get; set; }
    public int OrderIndex { get; set; }
}

public class ListeningResultDto
{
    public Guid Id { get; set; }
    public Guid LessonId { get; set; }
    public int Score { get; set; }
    public DateTime CompletedAt { get; set; }
}

public class SubmitListeningResultRequest
{
    public Guid LessonId { get; set; }
    public int Score { get; set; }
}
