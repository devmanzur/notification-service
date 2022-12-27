using Microsoft.Extensions.Logging;
using OrganizationNotificationPlugin.Brokers.Models;
using OrganizationNotificationPlugin.Models;
using OrganizationNotificationPlugin.Utils;

namespace OrganizationNotificationPlugin.Brokers;

/// <summary>
/// Broker for the notification service, this uses REST API Calls to communicate with our notifications service
/// </summary>
public class HttpNotificationBroker : INotificationBroker
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<HttpNotificationBroker> _logger;

    public HttpNotificationBroker(IHttpClientFactory httpClientFactory, ILogger<HttpNotificationBroker> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    /// <summary>
    /// Published the notification to notification service using HTTP POST call
    /// </summary>
    /// <param name="notification"></param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException">Thrown when the response was not successful</exception>
    public async Task<NotificationResponse> PublishAsync(AppNotification notification)
    {
        try
        {
            var client = _httpClientFactory.CreateClient(PluginConstants.ClientName);
            var response = await client.SendPostRequestAsync<EnvelopeResponse<NotificationResponse>>(
                new RequestModel($"api/notifications").AddJsonBody(notification));
            if (response.IsSuccess)
            {
                _logger.LogInformation("Received successful response");
                return response.Body;
            }

            throw new HttpRequestException("Failed to complete request");
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