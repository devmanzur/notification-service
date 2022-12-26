using OrganizationNotificationPlugin.Models;
using OrganizationNotificationPlugin.Utils;

namespace OrganizationNotificationPlugin.Brokers;

public class HttpNotificationBroker : INotificationBroker
{
    private readonly HttpClient _client;

    public HttpNotificationBroker(NotificationSinkConfiguration notificationSinkConfiguration)
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri(notificationSinkConfiguration.ServerUrl);
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