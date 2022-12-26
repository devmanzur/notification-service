using BreakingNews.Messages.Models;
using EasyNetQ;
using OrganizationNotificationService.Exceptions;
using OrganizationNotificationService.Features.SendNotification;
using OrganizationNotificationService.Models;
using OrganizationNotificationService.Utils;

namespace OrganizationNotificationService.BackgroundServices;

public class MessageQueueSubscriber : BackgroundService
{
    private readonly IBus _bus;
    private readonly NotificationService _notificationService;

    public MessageQueueSubscriber(IBus bus, NotificationService notificationService)
    {
        _bus = bus;
        _notificationService = notificationService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.Rpc.RespondAsync<NotificationMessage, NotificationResponseMessage>(async message =>
            {
                try
                {
                    var request = new NotificationRequest()
                    {
                        NotificationType = message.NotificationType!,
                        Body = message.Body!,
                        Recipient = message.Recipient!,
                        Title = message.Recipient!,
                        ContentType = message.ContentType!
                    };

                    RuleValidator.Validate<NotificationRequest, NotificationRequestValidator>(request);
                    var createNotification = request.GetType() switch
                    {
                        NotificationType.Email => await _notificationService.AddNewEmailNotification(request),
                        NotificationType.PushNotification => await _notificationService.AddNewPushNotification(request),
                        _ => throw new DomainValidationException("Invalid Notification type",
                            "Attempted to create unsupported notification type")
                    };
                    return new NotificationResponseMessage()
                    {
                        Id = createNotification.Id,
                        Recipient = createNotification.Recipient,
                        NotificationType = createNotification.NotificationType,
                        ContentType = createNotification.ContentType,
                        Title = createNotification.Title,
                        Status = createNotification.Status,
                        Body = createNotification.Body,
                        CreatedAt = createNotification.CreatedAt!
                    };
                }
                catch (DomainValidationException e)
                {
                    return new NotificationResponseMessage()
                    {
                        Id = null
                    };
                }
            }
, cancellationToken: stoppingToken);
    }
}