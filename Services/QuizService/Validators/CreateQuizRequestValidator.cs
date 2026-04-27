using FluentValidation;
using QuizService.DTOs.Requests;
namespace QuizService.Validators;
public class CreateQuizRequestValidator : AbstractValidator<CreateQuizRequest>
{
    public CreateQuizRequestValidator() { }
}
