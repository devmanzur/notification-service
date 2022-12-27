using Microsoft.EntityFrameworkCore;
using OrganizationNotificationService.Brokers.Notification;
using OrganizationNotificationService.Brokers.Persistence;
using OrganizationNotificationService.Models;

namespace OrganizationNotificationService.Features.SendNotification;

public partial class NotificationService
{
    private readonly NotificationDbContext _dbContext;
    private readonly EmailNotificationBroker _emailNotificationBroker;
    private readonly PushNotificationBroker _pushNotificationBroker;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(NotificationDbContext dbContext, EmailNotificationBroker emailNotificationBroker,
        PushNotificationBroker pushNotificationBroker,ILogger<NotificationService> logger)
    {
        _dbContext = dbContext;
        _emailNotificationBroker = emailNotificationBroker;
        _pushNotificationBroker = pushNotificationBroker;
        _logger = logger;
    }

    /// <summary>
    /// Returns the notification properties if found
    /// Otherwise returns null
    /// </summary>
    /// <param name="notificationId">Id of the notification</param>
    /// <returns></returns>
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

    /// <summary>
    /// Returns list of notifications whose Status is set to "Pending"
    /// </summary>
    /// <returns>list of notifications</returns>
    public async Task<List<ApplicationNotification>> GetPendingNotifications()
    {
        return await _dbContext.Notifications.AsNoTracking().Where(x => x.Status == NotificationStatus.Pending)
            .OrderBy(x => x.CreatedAt)
            .Take(50)
            .ToListAsync();
    }
}