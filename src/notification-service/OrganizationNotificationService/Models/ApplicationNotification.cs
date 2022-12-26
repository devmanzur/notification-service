using FluentValidation;
using OrganizationNotificationService.Utils;

namespace OrganizationNotificationService.Models;

public class ApplicationNotification : BaseEntity, IAuditable
{
    /// <summary>
    /// Empty constructor used by ef core
    /// </summary>
    protected ApplicationNotification()
    {
    }

    public ApplicationNotification(string recipient, string title, string body, NotificationContentType contentType,
        NotificationType type)
    {
        Recipient = recipient;
        Title = title;
        Body = body;
        ContentType = contentType;
        Type = type;
        Status = NotificationStatus.Pending;
        RuleValidator.Validate<ApplicationNotification, ApplicationNotificationValidator>(this);
    }

    /// <summary>
    /// The recipient that will receive the notification
    /// </summary>
    public string Recipient { get; private set; }

    /// <summary>
    /// Title of the notification
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Body of the notification
    /// </summary>
    public string Body { get; private set; }

    /// <summary>
    /// Content type of the notification, e.g. text or html
    /// </summary>
    public NotificationContentType ContentType { get; private set; }

    /// <summary>
    /// Type of the notification, this property is used to re-direct the notification to proper broker
    /// </summary>
    public NotificationType Type { get; private set; }

    /// <summary>
    /// Status of the notification, this indicates what is the current state of this notification so actors can filter and take appropriate action
    /// </summary>
    public NotificationStatus Status { get; private set; }

    /// <summary>
    /// Date and time the notification was created
    /// </summary>
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// Date and time the notification was last updated
    /// </summary>
    public DateTime? LastUpdatedAt { get; set; }
}

public class ApplicationNotificationValidator : BaseFluentValidator<ApplicationNotification>
{
    public ApplicationNotificationValidator()
    {
        RuleFor(x => x.Recipient).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Body).NotEmpty();
        RuleFor(x => x.ContentType).NotNull();
        RuleFor(x => x.Type).NotNull();
        RuleFor(x => x.Status).NotNull();
    }
}

public class ApplicationPushNotificationValidator : BaseFluentValidator<ApplicationNotification>
{
    public ApplicationPushNotificationValidator()
    {
        RuleFor(x => x.Recipient).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Body).NotEmpty();
        RuleFor(x => x.ContentType).NotNull().Must(x => x == NotificationContentType.Text)
            .WithMessage("Only text is supported as push notification content");
        RuleFor(x => x.Type).NotNull().Must(x => x == NotificationType.PushNotification);
    }
}

public class ApplicationEmailNotificationValidator : BaseFluentValidator<ApplicationNotification>
{
    public ApplicationEmailNotificationValidator()
    {
        RuleFor(x => x.Recipient).NotEmpty().EmailAddress();
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Body).NotEmpty();
        RuleFor(x => x.ContentType).NotNull();
        RuleFor(x => x.Type).NotNull().Must(x => x == NotificationType.Email);
    }
}