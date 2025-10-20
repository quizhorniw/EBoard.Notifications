using MassTransit;
using MediatR;
using SolarLab.EBoard.Notifications.Application.CQRS.Notifications.Send;

namespace SolarLab.EBoard.Notifications.Infrastructure.Messaging;

public class KafkaNotificationConsumer : IConsumer<SendNotificationCommand>
{
    private readonly IMediator _mediator;

    public KafkaNotificationConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<SendNotificationCommand> context)
    {
        await _mediator.Send(context.Message, context.CancellationToken);
    }
}