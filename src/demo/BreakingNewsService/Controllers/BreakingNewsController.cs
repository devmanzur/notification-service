using BreakingNewsService.Models;
using Microsoft.AspNetCore.Mvc;
using OrganizationNotificationPlugin;
using OrganizationNotificationPlugin.Models;

namespace BreakingNewsService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BreakingNewsController : ControllerBase
{
    private readonly AggregateNotificationPlugin _aggregatePlugin;

    private readonly string[] _subscribedEmails = new[] { "manzur@gmail.com", "dotnetdev@gmail.com", "dummyuser@gmail.com" };

    private readonly string[] _subscribedDeviceRegistrationTokens = new[]
        { "device-12654dfdfd", "device-68867684564545dfdsfsd", "device-5645646dfsdfsdf6sdf5" };


    public BreakingNewsController(INotificationBroker broker)
    {
        _aggregatePlugin = AggregateNotificationPluginBuilder.Create()
            .WithEmailPlugin(new EmailNotificationPlugin(broker))
            .WithPushNotificationPlugin(new PushNotificationPlugin(broker))
            .Build();
    }

    [HttpPost]
    public async Task<ActionResult<List<Guid>>> CreateBreakingNews([FromBody] BreakingNewsRequest request)
    {
        // save breaking news to db or do other exciting stuff
        var notifications = new List<AppNotification>();

        foreach (var email in _subscribedEmails)
        {
            notifications.Add(new AppNotification()
            {
                Body = request.Summary,
                Recipient = email,
                Title = request.Headline,
                ContentType = NotificationContentType.Text,
                NotificationType = NotificationType.Email
            });
        }

        foreach (var device in _subscribedDeviceRegistrationTokens)
        {
            notifications.Add(new AppNotification()
            {
                Body = request.Summary,
                Recipient = device,
                Title = request.Headline,
                ContentType = NotificationContentType.Text,
                NotificationType = NotificationType.PushNotification
            });
        }

        var responses = await _aggregatePlugin.SendNotificationsAsync(notifications);
        
        return Ok(responses);
    }
}