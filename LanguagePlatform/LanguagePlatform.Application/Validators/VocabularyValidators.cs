using FluentValidation;
using LanguagePlatform.Application.DTOs.Vocabulary;

namespace LanguagePlatform.Application.Validators;

public class CreateWordRequestValidator : AbstractValidator<CreateWordRequest>
{
    private static readonly string[] ValidLevels = ["Beginner", "Intermediate", "Advanced"];

    public CreateWordRequestValidator()
    {
        RuleFor(x => x.Term)
            .NotEmpty().WithMessage("Term is required.")
            .MaximumLength(100).WithMessage("Term must not exceed 100 characters.");

        RuleFor(x => x.Definition)
            .NotEmpty().WithMessage("Definition is required.")
            .MaximumLength(500).WithMessage("Definition must not exceed 500 characters.");

        RuleFor(x => x.Level)
            .NotEmpty().WithMessage("Level is required.")
            .Must(l => ValidLevels.Contains(l)).WithMessage("Level must be Beginner, Intermediate, or Advanced.");

        RuleFor(x => x.Topic)
            .NotEmpty().WithMessage("Topic is required.")
            .MaximumLength(100).WithMessage("Topic must not exceed 100 characters.");

        RuleFor(x => x.ExampleSentence)
            .MaximumLength(500).WithMessage("Example sentence must not exceed 500 characters.")
            .When(x => x.ExampleSentence != null);

        RuleFor(x => x.Pronunciation)
            .MaximumLength(100).WithMessage("Pronunciation must not exceed 100 characters.")
            .When(x => x.Pronunciation != null);
    }
}

public class UpdateWordRequestValidator : AbstractValidator<UpdateWordRequest>
{
    private static readonly string[] ValidLevels = ["Beginner", "Intermediate", "Advanced"];

    public UpdateWordRequestValidator()
    {
        RuleFor(x => x.Term)
            .NotEmpty().WithMessage("Term is required.")
            .MaximumLength(100).WithMessage("Term must not exceed 100 characters.");

        RuleFor(x => x.Definition)
            .NotEmpty().WithMessage("Definition is required.")
            .MaximumLength(500).WithMessage("Definition must not exceed 500 characters.");

        RuleFor(x => x.Level)
            .NotEmpty().WithMessage("Level is required.")
            .Must(l => ValidLevels.Contains(l)).WithMessage("Level must be Beginner, Intermediate, or Advanced.");

        RuleFor(x => x.Topic)
            .NotEmpty().WithMessage("Topic is required.")
            .MaximumLength(100).WithMessage("Topic must not exceed 100 characters.");
    }
}
