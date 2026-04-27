using FluentValidation;
using VocabularyService.DTOs.Requests;
namespace VocabularyService.Validators;
public class CreateWordRequestValidator : AbstractValidator<CreateWordRequest>
{
    public CreateWordRequestValidator() { }
}
