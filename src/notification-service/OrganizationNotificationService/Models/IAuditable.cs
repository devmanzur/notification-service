namespace OrganizationNotificationService.Models;
/// <summary>
/// Marker interface for models that need to maintain audit information
/// Inheriting entities are automatically tracked and updated
/// </summary>
public interface IAuditable
{
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
}