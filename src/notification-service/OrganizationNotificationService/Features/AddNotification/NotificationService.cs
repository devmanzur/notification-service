using Microsoft.EntityFrameworkCore;
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

    public async Task<NotificationResponse?> GetNotificationStatus(Guid notificationId)
    {
        var notification =
            await _dbContext.Notifications.AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == notificationId);
        if (notification == null)
        {
            return null;
        }

        return new NotificationResponse(notification);
    }
}