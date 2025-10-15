using SolarLab.EBoard.Notifications.Domain.Entities;

namespace SolarLab.EBoard.Notifications.Application.Abstractions.Email;

public interface IEmailSender
{
    Task SendAsync(Notification notification, CancellationToken cancellationToken = default);
}