namespace OrganizationNotificationService.Models;
/// <summary>
/// Marker interface for all db entities
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; set; }
}