using FluentValidation;
using OrganizationNotificationPlugin.Models;
using OrganizationNotificationPlugin.Utils;

namespace OrganizationNotificationPlugin;

public class EmailNotificationPlugin : INotificationPlugin
{
    private readonly INotificationBroker _httpNotificationBroker;

    public EmailNotificationPlugin(INotificationBroker httpNotificationBroker)
    {
        _httpNotificationBroker = httpNotificationBroker;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="notification"></param>
    /// <returns></returns>
    public async Task<Guid> SendNotificationAsync(AppNotification notification)
    {
        // validate email notification
        RuleValidator.Validate<AppNotification, AppEmailNotificationValidator>(notification);

        // Send the email notification
        var response = await _httpNotificationBroker.PublishAsync(notification);

        return response.Id;
    }
}

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