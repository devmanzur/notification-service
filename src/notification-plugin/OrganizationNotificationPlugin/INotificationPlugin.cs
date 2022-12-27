using OrganizationNotificationPlugin.Models;

namespace OrganizationNotificationPlugin;

/// <summary>
/// The marker interface for all notification plugins
/// If a plugin does not implement this it wont be able to be used with the aggregate plugin
/// </summary>
public interface INotificationPlugin
{
    /// <summary>
    /// Sends notification
    /// </summary>
    /// <param name="notification"></param>
    /// <returns>Notification Id that can be used to inquire its status</returns>
    Task<Guid> SendNotificationAsync(AppNotification notification);
}