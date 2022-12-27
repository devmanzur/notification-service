using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using OrganizationNotificationPlugin.Brokers;
using OrganizationNotificationPlugin.Exceptions;
using OrganizationNotificationPlugin.Utils;

namespace OrganizationNotificationPlugin.Configurations;

public static class ConfigurationExtensions
{
    /// <summary>
    ///  Injects a REST API based broker to communicate with notification service
    /// </summary>
    /// <param name="services"></param>
    /// <param name="serverUrl">Server Address of the notification service</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">Thrown when server url is null</exception>
    public static IServiceCollection AddNotificationPluginWithSyncBroker(this IServiceCollection services,
        string? serverUrl)
    {
        if (string.IsNullOrEmpty(serverUrl) || string.IsNullOrWhiteSpace(serverUrl))
        {
            throw new ArgumentNullException(serverUrl,
                "server url not defined within the provided configuration body");
        }
        
        AddBasicConfigurations(services);
        
        services.AddScoped<INotificationBroker, HttpNotificationBroker>();
        services.AddHttpClient(PluginConstants.ClientName, c =>
            c.BaseAddress = new Uri(serverUrl));

        return services;
    }

    /// <summary>
    ///  Injects a RabbitMQ based broker to communicate with notification service
    /// </summary>
    /// <param name="services"></param>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">Thrown when server url is null</exception>
    public static IServiceCollection AddNotificationPluginWithAsyncBroker(this IServiceCollection services,string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString) || string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentNullException(connectionString,
                "connection string not defined within the provided configuration body");
        }
        
        AddBasicConfigurations(services);

        // easyNetQ
        var bus = RabbitHutch.CreateBus(connectionString,
            x => x.EnableSystemTextJson(AppJsonUtils.GetSerializerOptions()));
        services.AddSingleton(bus);
        services.AddScoped<INotificationBroker, MessageQueueNotificationBroker>();

        return services;
    }

    /// <summary>
    /// Injects the basic files required for the plugin to work
    /// </summary>
    /// <param name="services"></param>
    /// <exception cref="InvalidActionException"></exception>
    private static void AddBasicConfigurations(IServiceCollection services)
    {
        if (services.Any(x => x.ServiceType == typeof(INotificationPlugin)))
        {
            throw new InvalidActionException("Multiple communication channels injected for sending messages",
                $"Implementation for Service type {typeof(INotificationBroker)} already injected");
        }

        services.AddScoped<INotificationPlugin, EmailNotificationPlugin>();
        services.AddScoped<INotificationPlugin, PushNotificationPlugin>();
    }
}