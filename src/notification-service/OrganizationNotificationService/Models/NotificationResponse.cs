namespace OrganizationNotificationService.Models;

/// <summary>
/// Model notification service uses to respond with
/// </summary>
public class NotificationResponse
{
    public string Id { get; set; }
    public string Recipient { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string ContentType { get; set; }
    public string NotificationType { get; set; }
    public string Status { get; set; }
    public string? CreatedAt { get; set; }

    public NotificationResponse()
    {
        
    }

    public NotificationResponse(ApplicationNotification notification)
    {
        Recipient = notification.Recipient;
        Title = notification.Title;
        Body = notification.Body;
        ContentType = notification.ContentType.ToString();
        NotificationType = notification.Type.ToString();
        Status = notification.Status.ToString();
        Id = notification.Id.ToString();
        CreatedAt = notification.CreatedAt?.ToString("s");
    }
}