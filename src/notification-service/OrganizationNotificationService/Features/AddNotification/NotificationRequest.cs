using FluentValidation;
using OrganizationNotificationService.Models;
using OrganizationNotificationService.Utils;

namespace OrganizationNotificationService.Features.AddNotification;

public class NotificationRequest
{
    public string Recipient { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string ContentType { get; set; }
    public string NotificationType { get; set; }

    public NotificationType Type => NotificationType.ToEnum<NotificationType>();
}

public class NotificationRequestValidator : BaseFluentValidator<NotificationRequest>
{
    public NotificationRequestValidator()
    {
        RuleFor(x => x.Recipient).NotEmpty().WithMessage("Please provide a valid Recipient");
        RuleFor(x => x.Title).NotEmpty().WithMessage("Please provide a valid title");
        RuleFor(x => x.Body).NotEmpty().WithMessage("Please provide a valid body");
        RuleFor(x => x.ContentType).NotEmpty().Must(EnumUtils.BelongToType<NotificationContentType>)
            .WithMessage("Please provide a valid content type: Text or Html");
        RuleFor(x => x.NotificationType).NotEmpty().Must(EnumUtils.BelongToType<NotificationType>)
            .WithMessage("Please provide a valid notification type: Email or PushNotification");
    }
}