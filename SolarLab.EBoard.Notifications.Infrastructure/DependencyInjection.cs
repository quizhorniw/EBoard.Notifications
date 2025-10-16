using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SolarLab.EBoard.Notifications.Application.Abstractions.Email;
using SolarLab.EBoard.Notifications.Infrastructure.Email;
using SolarLab.EBoard.Notifications.Infrastructure.Messaging;

namespace SolarLab.EBoard.Notifications.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var smtpSettings = configuration.GetSection("Smtp").Get<SmtpSettings>();
        if (smtpSettings == null)
        {
            throw new InvalidOperationException("No SMTP configuration found");
        }
        services.AddOptions<SmtpSettings>().Configure(opts =>
        {
            opts.Host = smtpSettings.Host;
            opts.Port = smtpSettings.Port;
            opts.UseSsl = smtpSettings.UseSsl;
            opts.FromName = smtpSettings.FromName;
            opts.FromAddress = smtpSettings.FromAddress;
            opts.Username = smtpSettings.Username;
            opts.Password = smtpSettings.Password;
        });
        
        services.AddSingleton<IEmailSender, EmailSender>();

        services.AddHostedService<KafkaNotificationConsumer>();
        
        return services;
    }
}