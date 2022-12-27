using BreakingNews.Messages.Models;
using EasyNetQ;
using Microsoft.Extensions.Logging;
using OrganizationNotificationPlugin.Models;

namespace OrganizationNotificationPlugin.Brokers;

/// <summary>
/// Broker for the notification service, this uses RabbitMQ Messages to communicate with our notifications service
/// This uses the AMQP RPC protocol because we expect a response from our service
/// </summary>
public class MessageQueueNotificationBroker : INotificationBroker
{
    /// <summary>
    /// RabbitMQ bus provided by EasyNetQ
    /// </summary>
    private readonly IBus _bus;
    private readonly ILogger<MessageQueueNotificationBroker> _logger;

    public MessageQueueNotificationBroker(IBus bus,ILogger<MessageQueueNotificationBroker> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    /// <summary>
    /// Published the notification to notification service using rabbitMq,
    /// When the response is ready this thread receives the response and sends it back to caller
    /// </summary>
    /// <param name="notification"></param>
    /// <returns></returns>
    public async Task<NotificationResponse> PublishAsync(AppNotification notification)
    {
        try
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
        catch (Exception e)
        {
            _logger.LogError(e,
                "An error occurred while communicating with notification service, error: {Error}, notification type: {NotificationType}",
                e.Message, notification.NotificationType.ToString());
            throw;
        }
    }
}