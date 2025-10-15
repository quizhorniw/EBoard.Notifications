using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SolarLab.EBoard.Notifications.Application.Abstractions.Email;
using SolarLab.EBoard.Notifications.Infrastructure.Email;

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

        services.AddSingleton(smtpSettings);
        services.AddSingleton<IEmailSender, EmailSender>();
        
        return services;
    }
}