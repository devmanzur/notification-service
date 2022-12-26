using EasyNetQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrganizationNotificationPlugin.Brokers;
using OrganizationNotificationPlugin.Exceptions;
using OrganizationNotificationPlugin.Utils;

namespace OrganizationNotificationPlugin.Configurations;

public static class ConfigurationExtensions
{
    private const string ServerUrl = "ServerUrl";
    private const string QueueConnectionStringKey = "ConnectionString";

    public static IServiceCollection AddNotificationPluginWithSyncBroker(this IServiceCollection services,
        IConfiguration configuration, string configKey)
    {
        if (services.Any(x => x.ServiceType == typeof(INotificationPlugin)))
        {
            throw new InvalidActionException("Multiple communication channels injected for sending messages",
                $"Implementation for Service type {typeof(INotificationBroker)} already injected");
        }
        
        services.AddScoped<INotificationBroker, HttpNotificationBroker>();
        services.AddHttpClient(PluginConstants.ClientName, c =>
            c.BaseAddress = new Uri(configuration.GetSection(configKey)[ServerUrl] ??
                                    throw new ArgumentNullException(ServerUrl,
                                        "ServerUrl not defined within the provided configuration body")));
        services.AddScoped<INotificationPlugin, EmailNotificationPlugin>();
        services.AddScoped<INotificationPlugin, PushNotificationPlugin>();

        return services;
    }

    public static IServiceCollection AddNotificationPluginWithAsyncBroker(this IServiceCollection services,
        IConfiguration configuration, string configKey)
    {
        if (services.Any(x => x.ServiceType == typeof(INotificationPlugin)))
        {
            throw new InvalidActionException("Multiple communication channels injected for sending messages",
                $"Implementation for Service type {typeof(INotificationBroker)} already injected");
        }

        // easyNetQ
        var bus = RabbitHutch.CreateBus(configuration.GetSection(configKey)[QueueConnectionStringKey] ??
                                        throw new ArgumentNullException(QueueConnectionStringKey,
                                            "QueueConnectionString not defined within the provided configuration body"));
        services.AddSingleton(bus);

        services.AddScoped<INotificationPlugin, EmailNotificationPlugin>();
        services.AddScoped<INotificationPlugin, PushNotificationPlugin>();
        services.AddScoped<INotificationBroker, MessageQueueNotificationBroker>();
        return services;
    }
}