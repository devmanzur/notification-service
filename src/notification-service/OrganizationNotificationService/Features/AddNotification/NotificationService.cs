using OrganizationNotificationService.Brokers.Persistence;
using OrganizationNotificationService.Models;
using OrganizationNotificationService.Utils;

namespace OrganizationNotificationService.Features.AddNotification;

public class NotificationService
{
    private readonly NotificationDbContext _dbContext;

    public NotificationService(NotificationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ApplicationNotification> AddNewEmailNotification(NotificationRequest request)
    {
        var notification = new ApplicationNotification(request.Recipient, request.Title, request.Body,
            request.ContentType.ToEnum<NotificationContentType>(), NotificationType.Email);
        
        // validates the entity is a valid email notification
        RuleValidator.Validate<EmailNotification,EmailNotificationValidator>(new EmailNotification(notification));

        _dbContext.Notifications.Add(notification);
        await _dbContext.SaveChangesAsync();
        return notification;
    }

    public async Task<ApplicationNotification> AddNewPushNotification(NotificationRequest request)
    {
        var notification = new ApplicationNotification(request.Recipient, request.Title, request.Body,
            request.ContentType.ToEnum<NotificationContentType>(), NotificationType.Email);

        // validates the entity is a valid push notification
        RuleValidator.Validate<PushNotification,PushNotificationValidator>(new PushNotification(notification));

        _dbContext.Notifications.Add(notification);
        await _dbContext.SaveChangesAsync();

        return notification;
    }
}