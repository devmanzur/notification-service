using FluentValidation;
using OrganizationNotificationService.Utils;

namespace OrganizationNotificationService.Features.SendNotification;

public class EmailNotification
{
    public string To { get; private set; }
    public string Subject { get; private set; }
    public string Body { get; private set; }
    public NotificationContentType ContentType { get; private set; }

    public EmailNotification(Notification notification)
    {
        if (notification.Type != NotificationType.Email)
        {
            throw new Exception(
                $"System tried to send email notification: for notification type {notification.Type}, notification id {notification.Id}");
        }

        To = notification.Recipient;
        Subject = notification.Title;
        Body = notification.Body;
        ContentType = notification.ContentType;
        RuleValidator.Validate<EmailNotification, EmailNotificationValidator>(this);
    }
}

public class EmailNotificationValidator : BaseFluentValidator<EmailNotification>
{
    public EmailNotificationValidator()
    {
        RuleFor(x => x.To).NotEmpty().EmailAddress();
        RuleFor(x => x.Subject).NotEmpty();
        RuleFor(x => x.Body).NotEmpty();
        RuleFor(x => x.ContentType).NotNull();
    }
}