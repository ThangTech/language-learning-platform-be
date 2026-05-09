using FluentValidation;
using LanguagePlatform.Application.DTOs.Vocabulary;

namespace LanguagePlatform.Application.Validators;

public class CreateWordRequestValidator : AbstractValidator<CreateWordRequest>
{
    private static readonly string[] ValidLevels = ["Beginner", "Intermediate", "Advanced"];

    public CreateWordRequestValidator()
    {
        RuleFor(x => x.Term)
            .NotEmpty().WithMessage("Từ vựng không được để trống.")
            .MaximumLength(100).WithMessage("Từ vựng không được vượt quá 100 ký tự.");

        RuleFor(x => x.Definition)
            .NotEmpty().WithMessage("Định nghĩa không được để trống.")
            .MaximumLength(500).WithMessage("Định nghĩa không được vượt quá 500 ký tự.");

        RuleFor(x => x.Level)
            .NotEmpty().WithMessage("Cấp độ không được để trống.")
            .Must(l => ValidLevels.Contains(l)).WithMessage("Cấp độ phải là Beginner, Intermediate hoặc Advanced.");

        RuleFor(x => x.Topic)
            .NotEmpty().WithMessage("Chủ đề không được để trống.")
            .MaximumLength(100).WithMessage("Chủ đề không được vượt quá 100 ký tự.");

        RuleFor(x => x.ExampleSentence)
            .MaximumLength(500).WithMessage("Câu ví dụ không được vượt quá 500 ký tự.")
            .When(x => x.ExampleSentence != null);

        RuleFor(x => x.Pronunciation)
            .MaximumLength(100).WithMessage("Phiên âm không được vượt quá 100 ký tự.")
            .When(x => x.Pronunciation != null);
    }
}

public class UpdateWordRequestValidator : AbstractValidator<UpdateWordRequest>
{
    private static readonly string[] ValidLevels = ["Beginner", "Intermediate", "Advanced"];

    public UpdateWordRequestValidator()
    {
        RuleFor(x => x.Term)
            .NotEmpty().WithMessage("Từ vựng không được để trống.")
            .MaximumLength(100).WithMessage("Từ vựng không được vượt quá 100 ký tự.");

        RuleFor(x => x.Definition)
            .NotEmpty().WithMessage("Định nghĩa không được để trống.")
            .MaximumLength(500).WithMessage("Định nghĩa không được vượt quá 500 ký tự.");

        RuleFor(x => x.Level)
            .NotEmpty().WithMessage("Cấp độ không được để trống.")
            .Must(l => ValidLevels.Contains(l)).WithMessage("Cấp độ phải là Beginner, Intermediate hoặc Advanced.");

        RuleFor(x => x.Topic)
            .NotEmpty().WithMessage("Chủ đề không được để trống.")
            .MaximumLength(100).WithMessage("Chủ đề không được vượt quá 100 ký tự.");
    }
}
