using OrganizationNotificationService.Configurations;
using OrganizationNotificationService.Features.SendNotification;
using OrganizationNotificationService.Models;

namespace OrganizationNotificationService.Brokers;

public class PushNotificationBroker : INotificationBroker
{
    public PushNotificationBroker()
    {
        
    }
    public Task<Result> SendNotification(Notification notification)
    {
        throw new NotImplementedException();
    }
}