using BreakingNews.Messages.Models;
using EasyNetQ;
using OrganizationNotificationPlugin.Models;

namespace OrganizationNotificationPlugin.Brokers;

public class MessageQueueNotificationBroker : INotificationBroker
{
    private readonly IBus _bus;

    public MessageQueueNotificationBroker(IBus bus)
    {
        _bus = bus;
    }

    public async Task<NotificationResponse> PublishAsync(AppNotification notification)
    {
        var request = new NotificationMessage()
        {
            Recipient = notification.Recipient,
            NotificationType = notification.NotificationType.ToString(),
            Body = notification.Body,
            Title = notification.Title,
            ContentType = notification.ContentType.ToString()
        };
        var response = await _bus.Rpc.RequestAsync<NotificationMessage, NotificationResponseMessage>(request);

        return new NotificationResponse
        {
            Id = Guid.Parse(response.Id!),
            Recipient = response.Recipient,
            NotificationType = response.NotificationType,
            ContentType = response.ContentType,
            Title = response.Title,
            Status = response.Status,
            Body = response.Body,
            CreatedAt = DateTimeOffset.Parse(response.CreatedAt)
        };
    }
}