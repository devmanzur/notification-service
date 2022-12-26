namespace OrganizationNotificationPlugin.Models;

public class AppNotification
{
    public string Recipient { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public NotificationContentType ContentType { get; set; }
    public NotificationType NotificationType { get; set; }
}