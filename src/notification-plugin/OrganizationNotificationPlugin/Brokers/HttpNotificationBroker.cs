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

    public HttpNotificationBroker(IHttpClientFactory  httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Published the notification to notification service using HTTP POST call
    /// </summary>
    /// <param name="notification"></param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException">Thrown when the response was not successful</exception>
    public async Task<NotificationResponse> PublishAsync(AppNotification notification)
    {
        var client = _httpClientFactory.CreateClient(PluginConstants.ClientName);
        var response = await client.SendPostRequestAsync<EnvelopeResponse<NotificationResponse>>(
            new RequestModel($"api/notifications").AddJsonBody(notification));
        if (response.IsSuccess)
        {
            return response.Body;
        }

        throw new HttpRequestException( "Failed to complete request");
    }
}