﻿using Microsoft.EntityFrameworkCore;
using OrganizationNotificationService.Exceptions;
using OrganizationNotificationService.Models;

namespace OrganizationNotificationService.Features.SendNotification;

public partial class NotificationService
{
    public async Task SendNotification(ApplicationNotification notification)
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
}