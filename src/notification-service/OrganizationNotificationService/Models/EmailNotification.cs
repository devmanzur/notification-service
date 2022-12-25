using FluentValidation;
using OrganizationNotificationService.Exceptions;
using OrganizationNotificationService.Utils;

namespace OrganizationNotificationService.Models;

public class EmailNotification
{
    /// <summary>
    /// Recipient email address
    /// </summary>
    public string To { get; private set; }
    /// <summary>
    /// Subject of the email
    /// </summary>
    public string Subject { get; private set; }
    /// <summary>
    /// Body content of the email
    /// </summary>
    public string Body { get; private set; }
    /// <summary>
    /// Content type of the email, html or text
    /// </summary>
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