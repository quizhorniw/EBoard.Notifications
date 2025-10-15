using MediatR;

namespace SolarLab.EBoard.Notifications.Application.CQRS.Notifications.Send;

public sealed record SendNotificationCommand(List<string> To, string Subject, string Content) : IRequest;