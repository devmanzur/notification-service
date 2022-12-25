using OrganizationNotificationService.Models;

namespace OrganizationNotificationService.Features.SendNotification;

public interface INotificationBroker
{
    Task<Result> SendNotification(Notification notification);
}