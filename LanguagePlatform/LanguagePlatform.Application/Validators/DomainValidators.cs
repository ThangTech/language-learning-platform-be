using FluentValidation;
using LanguagePlatform.Application.DTOs.Grammar;
using LanguagePlatform.Application.DTOs.Listening;
using LanguagePlatform.Application.DTOs.Quiz;

namespace LanguagePlatform.Application.Validators;

// ── Grammar Validators ────────────────────────────────────────────────────────

public class CreateGrammarTopicRequestValidator : AbstractValidator<CreateGrammarTopicRequest>
{
    private static readonly string[] ValidLevels = ["Beginner", "Intermediate", "Advanced"];

    public CreateGrammarTopicRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.")
            .MinimumLength(20).WithMessage("Content must be at least 20 characters.");

        RuleFor(x => x.Level)
            .NotEmpty().WithMessage("Level is required.")
            .Must(l => ValidLevels.Contains(l)).WithMessage("Level must be Beginner, Intermediate, or Advanced.");
    }
}

public class UpdateGrammarTopicRequestValidator : AbstractValidator<UpdateGrammarTopicRequest>
{
    private static readonly string[] ValidLevels = ["Beginner", "Intermediate", "Advanced"];

    public UpdateGrammarTopicRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.")
            .MinimumLength(20).WithMessage("Content must be at least 20 characters.");

        RuleFor(x => x.Level)
            .NotEmpty().WithMessage("Level is required.")
            .Must(l => ValidLevels.Contains(l)).WithMessage("Level must be Beginner, Intermediate, or Advanced.");
    }
}

// ── Listening Validators ──────────────────────────────────────────────────────

public class CreateListeningLessonRequestValidator : AbstractValidator<CreateListeningLessonRequest>
{
    private static readonly string[] ValidLevels = ["Beginner", "Intermediate", "Advanced"];

    public CreateListeningLessonRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.AudioUrl)
            .NotEmpty().WithMessage("Audio URL is required.")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("Audio URL must be a valid URL.");

        RuleFor(x => x.Level)
            .NotEmpty().WithMessage("Level is required.")
            .Must(l => ValidLevels.Contains(l)).WithMessage("Level must be Beginner, Intermediate, or Advanced.");

        RuleFor(x => x.Duration)
            .GreaterThan(0).WithMessage("Duration must be greater than 0 seconds.");
    }
}

public class SubmitListeningResultRequestValidator : AbstractValidator<SubmitListeningResultRequest>
{
    public SubmitListeningResultRequestValidator()
    {
        RuleFor(x => x.LessonId)
            .NotEmpty().WithMessage("Lesson ID is required.");

        RuleFor(x => x.Score)
            .InclusiveBetween(0, 100).WithMessage("Score must be between 0 and 100.");
    }
}

// ── Quiz Validators ───────────────────────────────────────────────────────────

public class SubmitQuizRequestValidator : AbstractValidator<SubmitQuizRequest>
{
    public SubmitQuizRequestValidator()
    {
        RuleFor(x => x.QuizId)
            .NotEmpty().WithMessage("Quiz ID is required.");

        RuleFor(x => x.Answers)
            .NotEmpty().WithMessage("At least one answer is required.")
            .Must(a => a.Count > 0).WithMessage("Answers list cannot be empty.");

        RuleForEach(x => x.Answers).ChildRules(answer =>
        {
            answer.RuleFor(a => a.QuestionId)
                .NotEmpty().WithMessage("Question ID is required.");
            answer.RuleFor(a => a.Answer)
                .NotEmpty().WithMessage("Answer is required.");
        });
    }
}
