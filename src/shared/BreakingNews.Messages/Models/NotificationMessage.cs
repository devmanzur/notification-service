namespace BreakingNews.Messages.Models;

public class NotificationMessage
{
    public string? Recipient { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
    public string? ContentType { get; set; }
    public string? NotificationType { get; set; }
}