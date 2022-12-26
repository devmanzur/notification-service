using Microsoft.Extensions.Options;
using OrganizationNotificationService.Configurations;
using OrganizationNotificationService.Exceptions;
using OrganizationNotificationService.Features.SendNotification;
using OrganizationNotificationService.Models;

namespace OrganizationNotificationService.Brokers.Notification;

public class EmailNotificationBroker : INotificationBroker
{
    private readonly ILogger<EmailNotificationBroker> _logger;

    /// <summary>
    /// The email account that will be used to send the emails, e.g. no-reply@breakingnews.com
    /// </summary>
    private readonly string _sender;

    public EmailNotificationBroker(IOptionsMonitor<EmailNotificationBrokerConfiguration > optionsMonitor,ILogger<EmailNotificationBroker> logger)
    {
        _logger = logger;
        var configuration = optionsMonitor.CurrentValue;
        _sender = configuration.Sender ??
                  throw new SystemConfigurationMissingException(
                      "Something went wrong on our side, please try again later",
                      new ArgumentNullException(nameof(configuration.Sender),
                          $"Missing configuration value for {nameof(EmailNotificationBroker)}, for property {nameof(configuration.Sender)}"));
    }

    /// <summary>
    /// Validates incoming app notification and sends email 
    /// </summary>
    /// <param name="applicationNotification"></param>
    /// <returns></returns>
    public async Task<Result> SendNotification(ApplicationNotification applicationNotification)
    {
        try
        {
            _logger.LogInformation("Sending notification {NotificationId}", applicationNotification.Id);
            // any properties are invalid this will throw DomainValidationException exception here
            var emailNotification = new EmailNotification(applicationNotification);

            // Since we now have all the information required to send and email here, we can use any email sender library
            // I Personally use [FluentEmail](https://github.com/lukencode/FluentEmail)
            // Here we are showing example to send text email only,
            // For HTML template support, we can use this UsingTemplate(string template, T model, bool isHtml = true) 

            // var email = await Email
            //     .From(_sender)
            //     .To(emailNotification.To, "User")
            //     .Subject(emailNotification.Subject)
            //     .Body(emailNotification.Body)
            //     .SendAsync();

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