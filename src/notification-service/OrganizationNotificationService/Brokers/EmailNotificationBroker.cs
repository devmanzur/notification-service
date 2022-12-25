using OrganizationNotificationService.Features.SendNotification;
using OrganizationNotificationService.Models;

namespace OrganizationNotificationService.Brokers;

public class EmailNotificationBroker : INotificationBroker
{
    //todo: implement email notification
    public Task<Result> SendNotification(Notification notification)
    {
        throw new NotImplementedException();
    }
}