namespace BreakingNews.Messages.Models;

public class NotificationResponseMessage
{
    public string? Id { get; set; }
    public string? Recipient { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
    public string? ContentType { get; set; }
    public string? NotificationType { get; set; }
    public string? Status { get; set; }
    public string CreatedAt { get; set; }
}