using Serilog;

namespace OrganizationNotificationService.Configurations;

public static class ConfigureSerilog
{
    /// <summary>
    /// Injects serilog
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IHostBuilder AddSerilog(this IHostBuilder builder, IConfiguration configuration)
    {
        return builder.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration
                .MinimumLevel.Warning()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithThreadId()
                .WriteTo.Console()
                .ReadFrom.Configuration(context.Configuration);
        });
    }
}