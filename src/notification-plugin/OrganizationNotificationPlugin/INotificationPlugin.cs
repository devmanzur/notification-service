using OrganizationNotificationPlugin.Models;

namespace OrganizationNotificationPlugin;

public interface INotificationPlugin
{
    /// <summary>
    /// Sends notification
    /// </summary>
    /// <param name="notification"></param>
    /// <returns>Notification Id that can be used to inquire its status</returns>
    Task<Guid> SendNotificationAsync(AppNotification notification);
}