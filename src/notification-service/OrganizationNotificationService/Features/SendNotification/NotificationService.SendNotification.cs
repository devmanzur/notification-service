using Microsoft.EntityFrameworkCore;
using OrganizationNotificationService.Exceptions;
using OrganizationNotificationService.Models;

namespace OrganizationNotificationService.Features.SendNotification;

public partial class NotificationService
{
    /// <summary>
    /// Sends the notification to its appropriate destination depending on the notification type
    /// </summary>
    /// <param name="notification"></param>
    /// <exception cref="InvalidActionException"></exception>
    public async Task SendNotification(ApplicationNotification notification)
    {
        try
        {
            var sendNotification = notification.Type switch
            {
                NotificationType.Email => await _emailNotificationBroker.SendNotification(notification),
                NotificationType.PushNotification => await _pushNotificationBroker.SendNotification(notification),
                _ => throw new InvalidActionException("Invalid notification type",
                    "Attempted to send a unsupported notification type")
            };

            await _dbContext.Notifications
                .Where(x => x.Id == notification.Id)
                .ExecuteUpdateAsync(
                    s => s.SetProperty(x => x.Status,
                        sendNotification.IsSuccess ? NotificationStatus.Sent : NotificationStatus.Failed));
        }
        catch (DomainValidationException e)
        {
            _logger.LogError(e,"Notification has one or more invalid properties and failed validation");
            
            await _dbContext.Notifications
                .Where(x => x.Id == notification.Id)
                .ExecuteUpdateAsync(
                    s => s.SetProperty(x => x.Status,
                        NotificationStatus.Corrupted));
        }
    }
}