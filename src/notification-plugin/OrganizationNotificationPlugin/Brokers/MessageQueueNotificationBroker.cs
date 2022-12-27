using BreakingNews.Messages.Models;
using EasyNetQ;
using Microsoft.Extensions.Logging;
using OrganizationNotificationPlugin.Models;

namespace OrganizationNotificationPlugin.Brokers;

public class MessageQueueNotificationBroker : INotificationBroker
{
    private readonly IBus _bus;
    private readonly ILogger<MessageQueueNotificationBroker> _logger;

    public MessageQueueNotificationBroker(IBus bus,ILogger<MessageQueueNotificationBroker> logger)
    {
        _bus = bus;
        _logger = logger;
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

        // Console.WriteLine($"Received response, notification id: {response.Id} ");        
        _logger.LogInformation("Received response, notification id: {NotificationId}",response.Id);
        
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