using OrganizationNotificationPlugin.Models;

namespace OrganizationNotificationPlugin;
/// <summary>
/// The marker interface for all types of notification brokers
/// That communicates with notification service
/// </summary>
public interface INotificationBroker
{
    Task<NotificationResponse> PublishAsync(AppNotification notification);
}