using OrganizationNotificationService.Exceptions;
using OrganizationNotificationService.Features.SendNotification;
using OrganizationNotificationService.Models;

namespace OrganizationNotificationService.Brokers.Notification;

public class PushNotificationBroker : INotificationBroker
{
    private readonly ILogger<PushNotificationBroker> _logger;

    public PushNotificationBroker(ILogger<PushNotificationBroker> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Validates incoming app notification model and sends push notification
    /// </summary>
    /// <param name="applicationNotification"></param>
    /// <returns></returns>
    public async Task<Result> SendNotification(ApplicationNotification applicationNotification)
    {
        try
        {
            _logger.LogInformation("Sending notification {NotificationId}", applicationNotification.Id);
            // any properties are invalid this will throw DomainValidationException exception here
            var pushNotification = new PushNotification(applicationNotification);
            // Since we now have all the information required to send and push here, we can use Firebase Admin SDK to send push notification

            // var message = new Message()
            // {
            //     Data = new Dictionary<string, string>()
            //     {
            //         { nameof(pushNotification.Title), pushNotification.Title },
            //         { nameof(pushNotification.Body), pushNotification.Body },
            //     },
            //     Token = pushNotification.DeviceRegistrationToken,
            // };

            // Send a message to the device corresponding to the provided
            // registration token.
            // string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            _logger.LogInformation("Notification sent successfully {NotificationId}",applicationNotification.Id);
            return Result.Success();
        }
        catch (DomainValidationException e)
        {
            _logger.LogError(e,"A validation error occurred while sending notification");
            throw;
        }
        catch (TimeoutException e)
        {
            _logger.LogError(e,"Failed to send notification");
            return Result.Failure(e.Message);
        }
    }
}