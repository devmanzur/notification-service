using FluentValidation;
using OrganizationNotificationPlugin.Models;
using OrganizationNotificationPlugin.Utils;

namespace OrganizationNotificationPlugin;

/// <summary>
/// Plugin to send email notifications
/// </summary>
public class EmailNotificationPlugin : INotificationPlugin
{
    private readonly INotificationBroker _notificationBroker;

    public EmailNotificationPlugin(INotificationBroker notificationBroker)
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
        // validate email notification
        RuleValidator.Validate<AppNotification, AppEmailNotificationValidator>(notification);

        // Send the email notification
        var response = await _notificationBroker.PublishAsync(notification);

        return response.Id;
    }
}

/// <summary>
/// Validates the notification object is a valid email
/// </summary>
class AppEmailNotificationValidator : BaseFluentValidator<AppNotification>
{
    public AppEmailNotificationValidator()
    {
        RuleFor(x => x.Recipient).NotEmpty().EmailAddress();
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Body).NotEmpty();
        RuleFor(x => x.ContentType).NotNull();
        RuleFor(x => x.NotificationType).NotNull().Must(x => x == NotificationType.Email);
    }
}