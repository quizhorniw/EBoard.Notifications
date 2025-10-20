using Microsoft.Extensions.DependencyInjection;
using SolarLab.EBoard.Notifications.Application.Abstractions.Email;
using SolarLab.EBoard.Notifications.Infrastructure.Email;
using SolarLab.EBoard.Notifications.Infrastructure.Messaging;

namespace SolarLab.EBoard.Notifications.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddOptions<SmtpSettings>().Configure(opts =>
        {
            opts.Host = Environment.GetEnvironmentVariable("SMTP_HOST");
            opts.Port = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT")!);
            opts.UseSsl = bool.Parse(Environment.GetEnvironmentVariable("SMTP_USE_SSL")!);
            opts.FromName = Environment.GetEnvironmentVariable("SMTP_FROM_NAME");
            opts.FromAddress = Environment.GetEnvironmentVariable("SMTP_FROM_ADDRESS");
            opts.Username = Environment.GetEnvironmentVariable("SMTP_USERNAME");
            opts.Password = Environment.GetEnvironmentVariable("SMTP_PASSWORD");
        });
        
        services.AddSingleton<IEmailSender, EmailSender>();

        services.AddHostedService<KafkaNotificationConsumer>();
        
        return services;
    }
}