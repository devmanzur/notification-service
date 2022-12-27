using System.Collections.Concurrent;
using OrganizationNotificationPlugin.Exceptions;
using OrganizationNotificationPlugin.Models;
using OrganizationNotificationPlugin.Utils;

namespace OrganizationNotificationPlugin;

/// <summary>
/// The aggregate plugin to combine and use multiple plugins at once
/// </summary>
/// <example>
//      var aggregatePlugin = AggregateNotificationPluginProvider.Create()
//     .WithEmailPlugin(new EmailNotificationPlugin(broker))
//     .WithPushNotificationPlugin(new PushNotificationPlugin(broker))
//     .Build();
// aggregatePlugin.SendNotificationsAsync(_notification);
/// </example>
public class AggregateNotificationPlugin
{
    private readonly Dictionary<NotificationType, INotificationPlugin> _notificationPlugins;


    public AggregateNotificationPlugin(Dictionary<NotificationType, INotificationPlugin> notificationPlugins)
    {
        _notificationPlugins = notificationPlugins;
    }


    public async Task<List<Guid>> SendNotificationsAsync(List<AppNotification> notifications)
    {
        if (ContainsUnsupportedType(notifications))
        {
            throw new InvalidActionException(
                "Notifications contains unsupported notification type, Please make sure you have configured required plugins",
                "Attempted to send a notification type without configuring plugin for that type of notification");
        }

        var responses = new ConcurrentBag<Guid>();

        await notifications.ParallelForEachAsync(async notification =>
        {
            _notificationPlugins.TryGetValue(notification.NotificationType, out var plugin);
            var response = await plugin!.SendNotificationAsync(notification);
            responses.Add(response);
        });

        return responses.ToList();
    }

    private bool ContainsUnsupportedType(IEnumerable<AppNotification> notifications)
    {
        return notifications.Select(x => x.NotificationType).Any(x => !_notificationPlugins.ContainsKey(x));
    }
}

public interface IAggregateNotificationPluginBuilder : IAggregateNotificationPluginsProvider
{
    AggregateNotificationPlugin Build();
}

public interface IAggregateNotificationPluginsProvider
{
    IAggregateNotificationPluginBuilder WithPushNotificationPlugin(PushNotificationPlugin notificationPlugin);
    IAggregateNotificationPluginBuilder WithEmailPlugin(EmailNotificationPlugin notificationPlugin);
}

public class AggregateNotificationPluginBuilder : IAggregateNotificationPluginBuilder
{
    private static Dictionary<NotificationType, INotificationPlugin> _plugins = new();

    private AggregateNotificationPluginBuilder()
    {
    }

    public static IAggregateNotificationPluginsProvider Create() => new AggregateNotificationPluginBuilder();

    public IAggregateNotificationPluginBuilder WithEmailPlugin(EmailNotificationPlugin notificationPlugin)
    {
        if (!_plugins.ContainsKey(NotificationType.Email))
        {
            _plugins[NotificationType.Email] = notificationPlugin;
        }

        return this;
    }

    public IAggregateNotificationPluginBuilder WithPushNotificationPlugin(PushNotificationPlugin notificationPlugin)
    {
        if (!_plugins.ContainsKey(NotificationType.PushNotification))
        {
            _plugins[NotificationType.PushNotification] = notificationPlugin;
        }

        return this;
    }

    public AggregateNotificationPlugin Build()
    {
        if (_plugins == null || !_plugins.Any())
        {
            throw new ArgumentNullException(nameof(_plugins), "No plugins provided");
        }

        return new AggregateNotificationPlugin(_plugins);
    }
}