using SolarLab.EBoard.Notifications.Application;
using SolarLab.EBoard.Notifications.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var host = builder.Build();

host.Run();