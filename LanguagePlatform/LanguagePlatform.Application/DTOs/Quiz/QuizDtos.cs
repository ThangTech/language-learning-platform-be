namespace LanguagePlatform.Application.DTOs.Quiz;

public class QuizDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public Guid? LessonId { get; set; }
    public List<QuizQuestionDto> Questions { get; set; } = new();
}

public class QuizQuestionDto
{
    public Guid Id { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string QuestionType { get; set; } = string.Empty;
    public string? OptionsJson { get; set; }
    public string? BlankText { get; set; }
    public int OrderIndex { get; set; }
}

public class CreateQuizRequest
{
    public string Title { get; set; } = string.Empty;
    public Guid? LessonId { get; set; }
    public List<CreateQuizQuestionRequest> Questions { get; set; } = new();
}

public class CreateQuizQuestionRequest
{
    public string QuestionText { get; set; } = string.Empty;
    public string QuestionType { get; set; } = "MultipleChoice";
    public string? OptionsJson { get; set; }
    public string? BlankText { get; set; }
    public string? ExpectedAnswer { get; set; }
    public int OrderIndex { get; set; }
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
    public List<QuizAnswerResultDto> AnswerResults { get; set; } = new();
}

public class QuizAnswerResultDto
{
    public Guid QuestionId { get; set; }
    public string UserAnswer { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
}
