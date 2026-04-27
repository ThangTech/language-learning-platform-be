using FluentValidation;
using LearningProgressService.DTOs.Requests;
namespace LearningProgressService.Validators;
public class UpdateStreakRequestValidator : AbstractValidator<UpdateStreakRequest>
{
    public UpdateStreakRequestValidator() { }
}
