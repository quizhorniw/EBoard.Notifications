using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SolarLab.EBoard.Notifications.Application.CQRS.Notifications.Send;
using JsonException = System.Text.Json.JsonException;

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

                try
                {
                    var command = JsonConvert.DeserializeObject<SendNotificationCommand>(result.Message.Value);
                    if (command != null)
                    {
                        await _mediator.Send(command, stoppingToken);
                    }
                }
                catch (JsonException) 
                {
                    _logger.LogError("Failed to deserialize message: {Value}", result.Message.Value);
                }
            }
        }
        catch (OperationCanceledException e)
        {
            _logger.LogError(e, "Error while consuming notification");
        }
        finally
        {
            consumer.Close();
        }
    }
}