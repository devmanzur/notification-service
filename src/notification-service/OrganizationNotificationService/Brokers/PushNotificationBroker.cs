using OrganizationNotificationService.Features.SendNotification;
using OrganizationNotificationService.Models;

namespace OrganizationNotificationService.Brokers;

public class PushNotificationBroker : INotificationBroker
{
    //todo: implement push notification broker
    public Task<Result> SendNotification(Notification notification)
    {
        throw new NotImplementedException();
    }
}