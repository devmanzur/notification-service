using OrganizationNotificationService.Exceptions;
using OrganizationNotificationService.Features.SendNotification;
using OrganizationNotificationService.Models;

namespace OrganizationNotificationService.Brokers.Notification;

public class PushNotificationBroker : INotificationBroker
{
    public PushNotificationBroker()
    {
    }

    public async Task<Result> SendNotification(ApplicationNotification applicationNotification)
    {
        try
        {
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

            return Result.Success();
        }
        catch (TimeoutException e)
        {
            return Result.Failure(e.Message);
        }
    }
}