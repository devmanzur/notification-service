using FluentValidation;
using OrganizationNotificationService.Exceptions;
using OrganizationNotificationService.Utils;

namespace OrganizationNotificationService.Models;
/// <summary>
/// Push notification value object
/// </summary>
public class PushNotification
{
    public string DeviceRegistrationToken { get; private set; }
    public string Title { get; private set; }
    public string Body { get; private set; }

    public PushNotification(ApplicationNotification applicationNotification)
    {
        if (applicationNotification.Type != NotificationType.PushNotification)
        {
            throw new InvalidActionException("Something went wrong on our side, please try again later",
                $"System tried to send push notification: for notification type {applicationNotification.Type}, notification id {applicationNotification.Id}");
        }

        DeviceRegistrationToken = applicationNotification.Recipient;
        Title = applicationNotification.Title;
        Body = applicationNotification.Body;
        RuleValidator.Validate<PushNotification,PushNotificationValidator>(this);
    }
}

public class PushNotificationValidator : BaseFluentValidator<PushNotification>
{
    public PushNotificationValidator()
    {
        RuleFor(x => x.DeviceRegistrationToken).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Body).NotEmpty();
    }
}