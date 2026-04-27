using FluentValidation;
using NotificationService.DTOs.Requests;
namespace NotificationService.Validators;
public class CreateNotificationRequestValidator : AbstractValidator<CreateNotificationRequest>
{
    public CreateNotificationRequestValidator() { }
}
