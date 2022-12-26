using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrganizationNotificationPlugin.Brokers;

namespace OrganizationNotificationPlugin.Configurations;

public static class ConfigurationExtensions
{
    private const string ServerUrl = "ServerUrl";

    public static IServiceCollection AddNotificationPluginWithSyncBroker(this IServiceCollection services,
        IConfiguration configuration, string configKey)
    {
        services.AddScoped<INotificationBroker, HttpNotificationBroker>();
        services.AddHttpClient<HttpNotificationBroker>(c =>
            c.BaseAddress = new Uri(configuration.GetSection(configKey)[ServerUrl] ??
                                    throw new ArgumentNullException(ServerUrl,
                                        "ServerUrl not defined within the provided configuration body")));
        services.AddScoped<INotificationPlugin, EmailNotificationPlugin>();
        services.AddScoped<INotificationPlugin, PushNotificationPlugin>();

        return services;
    }
}