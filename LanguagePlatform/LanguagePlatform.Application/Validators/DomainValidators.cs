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
            .NotEmpty().WithMessage("Tiêu đề không được để trống.")
            .MaximumLength(200).WithMessage("Tiêu đề không được vượt quá 200 ký tự.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Nội dung không được để trống.")
            .MinimumLength(20).WithMessage("Nội dung phải có ít nhất 20 ký tự.");

        RuleFor(x => x.Level)
            .NotEmpty().WithMessage("Cấp độ không được để trống.")
            .Must(l => ValidLevels.Contains(l)).WithMessage("Cấp độ phải là Beginner, Intermediate hoặc Advanced.");
    }
}

public class UpdateGrammarTopicRequestValidator : AbstractValidator<UpdateGrammarTopicRequest>
{
    private static readonly string[] ValidLevels = ["Beginner", "Intermediate", "Advanced"];

    public UpdateGrammarTopicRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Tiêu đề không được để trống.")
            .MaximumLength(200).WithMessage("Tiêu đề không được vượt quá 200 ký tự.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Nội dung không được để trống.")
            .MinimumLength(20).WithMessage("Nội dung phải có ít nhất 20 ký tự.");

        RuleFor(x => x.Level)
            .NotEmpty().WithMessage("Cấp độ không được để trống.")
            .Must(l => ValidLevels.Contains(l)).WithMessage("Cấp độ phải là Beginner, Intermediate hoặc Advanced.");
    }
}

// ── Listening Validators ──────────────────────────────────────────────────────

public class CreateListeningLessonRequestValidator : AbstractValidator<CreateListeningLessonRequest>
{
    private static readonly string[] ValidLevels = ["Beginner", "Intermediate", "Advanced"];

    public CreateListeningLessonRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Tiêu đề bài nghe không được để trống.")
            .MaximumLength(200).WithMessage("Tiêu đề không được vượt quá 200 ký tự.");

        RuleFor(x => x.AudioUrl)
            .NotEmpty().WithMessage("Đường dẫn audio không được để trống.")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("Đường dẫn audio không hợp lệ.");

        RuleFor(x => x.Level)
            .NotEmpty().WithMessage("Cấp độ không được để trống.")
            .Must(l => ValidLevels.Contains(l)).WithMessage("Cấp độ phải là Beginner, Intermediate hoặc Advanced.");

        RuleFor(x => x.Duration)
            .GreaterThan(0).WithMessage("Thời lượng phải lớn hơn 0 giây.");
    }
}

public class SubmitListeningResultRequestValidator : AbstractValidator<SubmitListeningResultRequest>
{
    public SubmitListeningResultRequestValidator()
    {
        RuleFor(x => x.LessonId)
            .NotEmpty().WithMessage("Mã bài nghe không được để trống.");

        RuleFor(x => x.Score)
            .InclusiveBetween(0, 100).WithMessage("Điểm số phải nằm trong khoảng từ 0 đến 100.");
    }
}

// ── Quiz Validators ───────────────────────────────────────────────────────────

public class SubmitQuizRequestValidator : AbstractValidator<SubmitQuizRequest>
{
    public SubmitQuizRequestValidator()
    {
        RuleFor(x => x.QuizId)
            .NotEmpty().WithMessage("Mã bài kiểm tra không được để trống.");

        RuleFor(x => x.Answers)
            .NotEmpty().WithMessage("Phải có ít nhất một câu trả lời.")
            .Must(a => a.Count > 0).WithMessage("Danh sách câu trả lời không được rỗng.");

        RuleForEach(x => x.Answers).ChildRules(answer =>
        {
            answer.RuleFor(a => a.QuestionId)
                .NotEmpty().WithMessage("Mã câu hỏi không được để trống.");
            answer.RuleFor(a => a.Answer)
                .NotEmpty().WithMessage("Câu trả lời không được để trống.");
        });
    }
}
