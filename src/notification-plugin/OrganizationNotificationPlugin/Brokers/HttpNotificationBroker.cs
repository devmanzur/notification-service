using OrganizationNotificationPlugin.Models;
using OrganizationNotificationPlugin.Utils;

namespace OrganizationNotificationPlugin.Brokers;

/// <summary>
/// Broker for the notification service, this uses REST API Calls to communicate with our service
/// </summary>
public class HttpNotificationBroker : INotificationBroker
{
    private readonly HttpClient _client;

    public HttpNotificationBroker(HttpClient httpClient)
    {
        _client = httpClient;
    }

    public async Task<NotificationResponse> PublishAsync(AppNotification notification)
    {
        var response = await _client.SendPostRequestAsync<EnvelopeResponse<NotificationResponse>>(
            new RequestModel($"api/notifications").AddJsonBody(notification));
        if (response.IsSuccess)
        {
            return response.Body;
        }

        throw new HttpRequestException(response.Errors?.ToString() ?? "Failed to complete request");
    }
}