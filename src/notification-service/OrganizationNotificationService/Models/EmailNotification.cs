using FluentValidation;
using OrganizationNotificationService.Exceptions;
using OrganizationNotificationService.Utils;

namespace OrganizationNotificationService.Models;

public class EmailNotification
{
    public string To { get; private set; }
    public string Subject { get; private set; }
    public string Body { get; private set; }
    public NotificationContentType ContentType { get; private set; }

    public EmailNotification(ApplicationNotification applicationNotification)
    {
        if (applicationNotification.Type != NotificationType.Email)
        {
            throw new InvalidActionException("Something went wrong on our side, please try again later",
                $"System tried to send email notification: for notification type {applicationNotification.Type}, notification id {applicationNotification.Id}");
        }

        To = applicationNotification.Recipient;
        Subject = applicationNotification.Title;
        Body = applicationNotification.Body;
        ContentType = applicationNotification.ContentType;
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