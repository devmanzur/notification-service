namespace OrganizationNotificationPlugin.Models;

public class AppNotification
{
    /// <summary>
    /// Receiver of the notification, e.g. email, device id
    /// </summary>
    public string Recipient { get; set; }

    /// <summary>
    /// Title of the notification
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Body of the notification
    /// </summary>
    public string Body { get; set; }

    /// <summary>
    /// The Content type of the notification body, could be HTML, Text etc. Note: Some content types are not supported by certain notification types
    /// If invalid content type notification type combination is used this will throw validation exception when trying to send notification
    /// </summary>
    public NotificationContentType ContentType { get; set; }

    /// <summary>
    /// Type of the notification, This is used by the broker to determine which broker to use for sending a message
    /// </summary>
    public NotificationType NotificationType { get; set; }
}