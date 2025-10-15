using MediatR;
using SolarLab.EBoard.Notifications.Application.Abstractions.Email;
using SolarLab.EBoard.Notifications.Domain.Entities;
using SolarLab.EBoard.Notifications.Domain.ValueObjects;

namespace SolarLab.EBoard.Notifications.Application.CQRS.Notifications.Send;

public class SendNotificationHandler : IRequestHandler<SendNotificationCommand>
{
    private readonly IEmailSender _emailSender;

    public SendNotificationHandler(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public async Task Handle(SendNotificationCommand request, CancellationToken cancellationToken)
    {
        var to = request.To
            .Select(to => new EmailAddress(to))
            .ToList();
        
        var notification = Notification.Create(to, request.Subject, request.Content);
        await _emailSender.SendAsync(notification, cancellationToken);
    }
}