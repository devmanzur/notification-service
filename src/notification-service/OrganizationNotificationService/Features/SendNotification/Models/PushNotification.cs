using FluentValidation;
using OrganizationNotificationService.Exceptions;
using OrganizationNotificationService.Utils;

namespace OrganizationNotificationService.Features.SendNotification.Models;

public class PushNotification
{
    public string DeviceRegistrationToken { get; private set; }
    public string Title { get; private set; }
    public string Body { get; private set; }

    public PushNotification(Notification notification)
    {
        if (notification.Type != NotificationType.PushNotification)
        {
            throw new InvalidActionException("Something went wrong on our side, please try again later",
                $"System tried to send push notification: for notification type {notification.Type}, notification id {notification.Id}");
        }
        
        DeviceRegistrationToken = notification.Recipient;
        Title = notification.Title;
        Body = notification.Body;
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