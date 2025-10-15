using FluentValidation;

namespace SolarLab.EBoard.Notifications.Application.CQRS.Notifications.Send;

internal class SendNotificationValidator : AbstractValidator<SendNotificationCommand>
{
    public SendNotificationValidator()
    {
        RuleFor(c => c.To).NotEmpty();
        RuleFor(c => c.Subject).NotEmpty().MaximumLength(100);
        RuleFor(c => c.Content).NotEmpty();
    }
}