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

public class CreateLessonRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string AudioUrl { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
    public int Duration { get; set; }
    public string? TranscriptJson { get; set; }
}

public class UpdateLessonRequest
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
    public string Level { get; set; } = string.Empty;
    public Guid? LessonId { get; set; }
    public List<DictationSentenceDto> Sentences { get; set; } = new();
}

public class DictationSentenceDto
{
    public Guid Id { get; set; }
    public string Sentence { get; set; } = string.Empty;
    public string AudioTitle { get; set; } = string.Empty;
    public string? Hint { get; set; }
    public int Duration { get; set; }
    public int OrderIndex { get; set; }
}

public class ListeningResultDto
{
    public Guid Id { get; set; }
    public Guid LessonId { get; set; }
    public int Score { get; set; }
    public int TimeTaken { get; set; }
    public int ListenCount { get; set; }
    public DateTime CompletedAt { get; set; }
}

public class SubmitListeningResultRequest
{
    public Guid LessonId { get; set; }
    public int Score { get; set; }
    public int TimeTaken { get; set; }
    public int ListenCount { get; set; }
}
