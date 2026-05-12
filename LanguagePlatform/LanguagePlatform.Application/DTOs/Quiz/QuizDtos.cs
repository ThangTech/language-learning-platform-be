namespace LanguagePlatform.Application.DTOs.Quiz;

public class QuizDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public Guid? LessonId { get; set; }

    public string Difficulty { get; set; } = string.Empty;

    public string DifficultyColor { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public string TypeIcon { get; set; } = string.Empty;

    public string Duration { get; set; } = string.Empty;

    public int DurationMinutes { get; set; }

    public int TotalQuestions => Questions.Count;

    public List<QuizQuestionDto> Questions { get; set; } = new();
}

public class QuizQuestionDto
{
    public Guid Id { get; set; }

    public string QuestionText { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public List<string> Options { get; set; } = new();

    public string CorrectAnswer { get; set; } = string.Empty;

    public string? Explanation { get; set; }

    public string? AudioUrl { get; set; }
}

public class CreateQuizRequest
{
    public string Title { get; set; } = string.Empty;

    public Guid? LessonId { get; set; }

    public string Difficulty { get; set; } = "Medium";

    public string Type { get; set; } = "MultipleChoice";

    public int DurationMinutes { get; set; } = 10;

    public List<CreateQuizQuestionRequest> Questions { get; set; } = new();
}

public class CreateQuizQuestionRequest
{
    public string QuestionText { get; set; } = string.Empty;

    public string Type { get; set; } = "MultipleChoice";

    public List<string> Options { get; set; } = new();

    public string CorrectAnswer { get; set; } = string.Empty;

    public string? Explanation { get; set; }

    public string? AudioUrl { get; set; }
}

public class UpdateQuizRequest
{
    public string Title { get; set; } = string.Empty;

    public string Difficulty { get; set; } = "Medium";

    public string Type { get; set; } = "MultipleChoice";

    public int DurationMinutes { get; set; } = 10;
}

public class SubmitQuizRequest
{
    public Guid QuizId { get; set; }

    public List<QuizAnswerItem> Answers { get; set; } = new();
}

public class QuizAnswerItem
{
    public Guid QuestionId { get; set; }

    public string Answer { get; set; } = string.Empty;
}

public class QuizResultDto
{
    public Guid QuizId { get; set; }

    public int TotalQuestions { get; set; }

    public int CorrectAnswers { get; set; }

    public int Score { get; set; }

    public List<QuizAnswerResultDto> Answers { get; set; } = new();
}

public class QuizAnswerResultDto
{
    public Guid QuestionId { get; set; }

    public bool IsCorrect { get; set; }

    public string CorrectAnswer { get; set; } = string.Empty;

    public string? Explanation { get; set; }
}
