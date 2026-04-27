using FluentValidation;
using GrammarService.DTOs.Requests;
namespace GrammarService.Validators;
public class CreateGrammarTopicRequestValidator : AbstractValidator<CreateGrammarTopicRequest>
{
    public CreateGrammarTopicRequestValidator() { }
}
