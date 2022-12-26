using OrganizationNotificationService.Features.SendNotification;
using Quartz;

namespace OrganizationNotificationService.BackgroundServices.Jobs;

/// <summary>
/// Finds pending notifications and dispatches them
/// </summary>
public class SendPendingNotificationsJob : IJob
{
    private readonly ILogger<SendPendingNotificationsJob> _logger;
    private readonly NotificationService _notificationService;

    public SendPendingNotificationsJob(ILogger<SendPendingNotificationsJob> logger,
        NotificationService notificationService)
    {
        _logger = logger;
        _notificationService = notificationService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("execution started");
        var pendingNotifications = await _notificationService.GetPendingNotifications();

        if (pendingNotifications.Any())
        {
            _logger.LogWarning("Found pending notifications");

            foreach (var notification in pendingNotifications)
            {
                try
                {
                    await _notificationService.SendNotification(notification);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error occurred while sending notification id: {NotificationId}, notification type: {NotificationType}",
                        notification.Id, notification.Type.ToString());
                }
            }
        }

        _logger.LogInformation("execution completed");
    }
}