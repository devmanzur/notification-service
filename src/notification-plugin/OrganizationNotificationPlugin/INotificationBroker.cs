using OrganizationNotificationPlugin.Models;

namespace OrganizationNotificationPlugin;

public interface INotificationBroker
{
    Task<NotificationResponse> PublishAsync(AppNotification notification);
}