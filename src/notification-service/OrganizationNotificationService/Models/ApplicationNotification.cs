using FluentValidation;
using OrganizationNotificationService.Utils;

namespace OrganizationNotificationService.Models;

public class ApplicationNotification : BaseEntity, IAuditable
{
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
        RuleValidator.Validate<ApplicationNotification,NotificationValidator>(this);
    }

    public string Recipient { get; private set; }
    public string Title { get; private set; }
    public string Body { get; private set; }
    public NotificationContentType ContentType { get; private set; }
    public NotificationType Type { get; private set; }
    public NotificationStatus Status { get; private set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
}

public class NotificationValidator : BaseFluentValidator<ApplicationNotification>
{
    public NotificationValidator()
    {
        RuleFor(x => x.Recipient).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Body).NotEmpty();
        RuleFor(x => x.ContentType).NotNull();
        RuleFor(x => x.Type).NotNull();
        RuleFor(x => x.Status).NotNull();
    }
}