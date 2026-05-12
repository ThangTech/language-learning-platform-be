namespace LanguagePlatform.Application.DTOs.Listening;

public class ListeningLessonDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string AudioUrl { get; set; } = string.Empty;

    public string Level { get; set; } = string.Empty;

    public string LevelColor => GetLevelColor();

    public string Topic { get; set; } = string.Empty;

    public string TopicIcon => GetTopicIcon();

    public int Duration { get; set; }

    public int TotalDuration => Duration;

    public string DurationText => FormatDuration();

    public string AudioTitle => Title;

    public string? TranscriptJson { get; set; }

    public DateTime CreatedAt { get; set; }

    private string FormatDuration()
    {
        var minutes = Duration / 60;
        var seconds = Duration % 60;

        return $"{minutes}:{seconds:00}";
    }

    private string GetLevelColor()
    {
        if (Level == "A1" || Level == "A2")
        {
            return "bg-primary-fixed text-on-primary-fixed";
        }

        return "bg-secondary-fixed text-on-secondary-container";
    }

    private string GetTopicIcon()
    {
        return Topic switch
        {
            "Travel" => "flight",
            "Shopping" => "shopping_bag",
            "Study" => "school",
            "Education" => "menu_book",
            _ => "headphones"
        };
    }
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

    public string LevelColor => GetLevelColor();

    public string Topic { get; set; } = string.Empty;

    public int TotalExercises => Sentences.Count;

    public List<DictationSentenceDto> Sentences { get; set; } = new();

    private string GetLevelColor()
    {
        if (Level == "A1" || Level == "A2")
        {
            return "bg-primary-fixed text-on-primary-fixed";
        }

        return "bg-secondary-fixed text-on-secondary-container";
    }
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
