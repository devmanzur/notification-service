using OrganizationNotificationService.Models;

namespace OrganizationNotificationService.Features.SendNotification;
/// <summary>
/// This is the marker interface that all notification brokers must implement
/// </summary>
public interface INotificationBroker
{
    Task<Result> SendNotification(ApplicationNotification applicationNotification);
}