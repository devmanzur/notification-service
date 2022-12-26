using OrganizationNotificationPlugin.Models;
using OrganizationNotificationPlugin.Utils;

namespace OrganizationNotificationPlugin.Brokers;

/// <summary>
/// Broker for the notification service, this uses REST API Calls to communicate with our service
/// </summary>
public class HttpNotificationBroker : INotificationBroker
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpNotificationBroker(IHttpClientFactory  httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

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