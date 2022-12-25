namespace OrganizationNotificationService.Models;

public interface IAuditable
{
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
}