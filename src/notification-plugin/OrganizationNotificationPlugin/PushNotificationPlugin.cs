using FluentValidation;
using OrganizationNotificationPlugin.Models;
using OrganizationNotificationPlugin.Utils;

namespace OrganizationNotificationPlugin;

public class PushNotificationPlugin : INotificationPlugin
{
    private readonly INotificationBroker _httpNotificationBroker;

    public PushNotificationPlugin(INotificationBroker httpNotificationBroker)
    {
        _httpNotificationBroker = httpNotificationBroker;
    }

    public async Task<Guid> SendNotificationAsync(AppNotification notification)
    {
        // validate push notification
        RuleValidator.Validate<AppNotification, AppPushNotificationValidator>(notification);

        // Send the email notification
        var response = await _httpNotificationBroker.PublishAsync(notification);

        return response.Id;
    }
}

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