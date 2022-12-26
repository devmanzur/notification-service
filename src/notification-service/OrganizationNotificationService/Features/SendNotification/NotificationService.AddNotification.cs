using OrganizationNotificationService.Models;
using OrganizationNotificationService.Utils;

namespace OrganizationNotificationService.Features.SendNotification;

public partial class NotificationService
{
    public async Task<NotificationResponse> AddNewEmailNotification(NotificationRequest request)
    {
        var notification = new ApplicationNotification(request.Recipient, request.Title, request.Body,
            request.ContentType.ToEnum<NotificationContentType>(), NotificationType.Email);

        // validates the entity is a valid email notification
        RuleValidator.Validate<ApplicationNotification, ApplicationEmailNotificationValidator>(notification);

        _dbContext.Notifications.Add(notification);
        await _dbContext.SaveChangesAsync();
        
        return new NotificationResponse(notification);
    }

    public async Task<NotificationResponse> AddNewPushNotification(NotificationRequest request)
    {
        var notification = new ApplicationNotification(request.Recipient, request.Title, request.Body,
            request.ContentType.ToEnum<NotificationContentType>(), NotificationType.PushNotification);

        // validates the entity is a valid push notification
        RuleValidator.Validate<ApplicationNotification, ApplicationPushNotificationValidator>(notification);

        _dbContext.Notifications.Add(notification);
        await _dbContext.SaveChangesAsync();

        return new NotificationResponse(notification);
    }
}