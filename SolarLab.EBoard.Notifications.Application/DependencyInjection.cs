using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace SolarLab.EBoard.Notifications.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config => config
            .RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}