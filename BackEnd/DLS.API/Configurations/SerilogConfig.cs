using Serilog;
using Serilog.Events;

namespace DLS.API.Configurations;

public static class SerilogConfig
{
    internal static IHostBuilder ConfigureSerilog(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseSerilog((context, services, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentName()
                .Enrich.WithThreadId()
                .WriteTo.Console();

            if (context.HostingEnvironment.IsProduction())
            {
                loggerConfiguration.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
            }
        });
    }
}
