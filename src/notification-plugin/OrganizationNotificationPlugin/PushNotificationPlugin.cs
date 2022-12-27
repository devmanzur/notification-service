using FluentValidation;
using OrganizationNotificationPlugin.Models;
using OrganizationNotificationPlugin.Utils;

namespace OrganizationNotificationPlugin;
/// <summary>
/// Plugin to send push notifications
/// </summary>
public class PushNotificationPlugin : INotificationPlugin
{
    private readonly INotificationBroker _notificationBroker;

    public PushNotificationPlugin(INotificationBroker notificationBroker)
    {
        _notificationBroker = notificationBroker;
    }

    /// <summary>
    /// Validates the notification type and constraints
    /// Sends notification using the injected notification broker
    /// </summary>
    /// <param name="notification"></param>
    /// <returns></returns>
    public async Task<Guid> SendNotificationAsync(AppNotification notification)
    {
        // validate push notification
        RuleValidator.Validate<AppNotification, AppPushNotificationValidator>(notification);

        // Send the email notification
        var response = await _notificationBroker.PublishAsync(notification);

        return response.Id;
    }
}
/// <summary>
/// Validates the notification object is a valid push notification
/// </summary>
class AppPushNotificationValidator : BaseFluentValidator<AppNotification>
{
    public AppPushNotificationValidator()
    {
        RuleFor(x => x.Recipient).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Body).NotEmpty();
        RuleFor(x => x.ContentType).NotNull().Must(x => x == NotificationContentType.Text);
        RuleFor(x => x.NotificationType).NotNull().Must(x => x == NotificationType.PushNotification);
    }
}