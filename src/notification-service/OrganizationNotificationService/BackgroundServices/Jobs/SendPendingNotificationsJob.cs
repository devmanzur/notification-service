using OrganizationNotificationService.Features.SendNotification;
using Quartz;

namespace OrganizationNotificationService.BackgroundServices.Jobs;

public class SendPendingNotificationsJob : IJob
{
    private readonly NotificationService _notificationService;

    public SendPendingNotificationsJob(ILogger<SendPendingNotificationsJob> logger,
        NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var pendingNotifications = await _notificationService.GetPendingNotifications();

        foreach (var notification in pendingNotifications)
        {
            await _notificationService.SendNotification(notification);
        }
    }
}