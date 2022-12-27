using BreakingNews.Messages.Models;
using EasyNetQ;
using OrganizationNotificationService.Brokers.Persistence;
using OrganizationNotificationService.Exceptions;
using OrganizationNotificationService.Models;
using OrganizationNotificationService.Utils;

namespace OrganizationNotificationService.BackgroundServices;

/// <summary>
/// RabbitMQ based notification listener that listens for any incoming NotificationMessage AMQP RPC Calls
/// </summary>
public class AddNotificationEventListenerBackgroundService : BackgroundService
{
    private readonly IBus _bus;
    private readonly ILogger<AddNotificationEventListenerBackgroundService> _logger;
    private readonly IServiceScope _scope;

    public AddNotificationEventListenerBackgroundService(IBus bus, IServiceProvider serviceProvider,
        ILogger<AddNotificationEventListenerBackgroundService> logger)
    {
        _bus = bus;
        _logger = logger;
        _scope = serviceProvider.CreateScope();
    }

    /// <summary>
    /// Validates and stores the email notification
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    private async Task<NotificationResponse> AddNewEmailNotification(NotificationDbContext dbContext,
        NotificationRequest request)
    {
        var notification = new ApplicationNotification(request.Recipient, request.Title, request.Body,
            request.ContentType.ToEnum<NotificationContentType>(), NotificationType.Email);

        // validates the entity is a valid email notification
        RuleValidator.Validate<ApplicationNotification, ApplicationEmailNotificationValidator>(notification);

        dbContext.Notifications.Add(notification);
        await dbContext.SaveChangesAsync();

        return new NotificationResponse(notification);
    }

    /// <summary>
    /// Validates and stores the push notification
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    private async Task<NotificationResponse> AddNewPushNotification(NotificationDbContext dbContext,
        NotificationRequest request)
    {
        var notification = new ApplicationNotification(request.Recipient, request.Title, request.Body,
            request.ContentType.ToEnum<NotificationContentType>(), NotificationType.PushNotification);

        // validates the entity is a valid push notification
        RuleValidator.Validate<ApplicationNotification, ApplicationPushNotificationValidator>(notification);

        dbContext.Notifications.Add(notification);
        await dbContext.SaveChangesAsync();

        return new NotificationResponse(notification);
    }

    /// <summary>
    /// Runs whenever a NotificationMessage AMPQ RPC Call is received 
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <exception cref="DomainValidationException"></exception>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_bus.Advanced.IsConnected)
        {
            // Allow it 1 minute to establish connection
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            _logger.LogInformation("{ServiceName} started execution",
                nameof(AddNotificationEventListenerBackgroundService));
        }

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

                    using var scope = _scope.ServiceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetService<NotificationDbContext>();

                    var createNotification = request.GetType() switch
                    {
                        NotificationType.Email => await AddNewEmailNotification(dbContext!, request),
                        NotificationType.PushNotification => await AddNewPushNotification(dbContext!,
                            request),
                        _ => throw new DomainValidationException("Invalid Notification type",
                            "Attempted to create unsupported notification type")
                    };

                    _logger.LogInformation("Notification saved, {NotificationId}", createNotification.Id);

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
                    _logger.LogError(e, e.Message);


                    return null;
                }
                catch (Exception e)
                {
                    // Handling any unexpected exceptions here, so the application does not close if background service crashes
                    _logger.LogError(e, e.Message);
                    return null;
                }
            }
            , cancellationToken: stoppingToken);
    }
}