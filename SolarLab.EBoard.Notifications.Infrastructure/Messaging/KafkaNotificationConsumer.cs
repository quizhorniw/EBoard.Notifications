using System.Text.Json;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SolarLab.EBoard.Notifications.Application.CQRS.Notifications.Send;

namespace SolarLab.EBoard.Notifications.Infrastructure.Messaging;

public class KafkaNotificationConsumer : BackgroundService
{
    private readonly ILogger<KafkaNotificationConsumer> _logger;
    private readonly IMediator _mediator;

    public KafkaNotificationConsumer(IMediator mediator, ILogger<KafkaNotificationConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "notifications-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        
        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe("notifications");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = consumer.Consume(stoppingToken);
                if (result == null)
                {
                    continue;
                }

                var command = JsonSerializer.Deserialize<SendNotificationCommand>(result.Message.Value);
                if (command != null)
                {
                    await _mediator.Send(command, stoppingToken);
                }
            }
        }
        catch (OperationCanceledException e)
        {
            _logger.LogError(e, "Error while consuming notification through Kafka");
        }
        finally
        {
            consumer.Close();
        }
    }
}