namespace OrganizationNotificationService.Models;

/// <summary>
/// Different states the notification can be in,
/// </summary>
public enum NotificationStatus
{
    Pending, // Notification waiting to be sent
    Sent, // Notification was sent successfully
    Failed, // Failed to Send notification
    Corrupted, // Failed validation when sending
    Unknown
}