namespace OrganizationNotificationPlugin.Models;

public class NotificationResponse
{
    public Guid Id { get; set; }
    public string Recipient { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public NotificationContentType ContentType { get; set; }
    public NotificationType NotificationType { get; set; }
    public NotificationStatus Status { get; set; }
    public DateTime? CreatedAt { get; set; }
}