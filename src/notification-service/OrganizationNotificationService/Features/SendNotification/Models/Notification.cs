namespace OrganizationNotificationService.Features.SendNotification.Models;

public class Notification
{
    public Guid Id { get; set; }
    public string Recipient { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public NotificationContentType ContentType { get; set; }
    public NotificationType Type { get; set; }
    
}