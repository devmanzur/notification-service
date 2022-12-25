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
}

public class NotificationRequestValidator : BaseFluentValidator<NotificationRequest>
{
    public NotificationRequestValidator()
    {
        RuleFor(x => x.Recipient).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Body).NotEmpty();
        RuleFor(x => x.ContentType).NotEmpty().Must(EnumUtils.BelongToType<NotificationContentType>);
    }
}