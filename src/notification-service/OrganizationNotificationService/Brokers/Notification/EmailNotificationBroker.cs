using Microsoft.Extensions.Options;
using OrganizationNotificationService.Configurations;
using OrganizationNotificationService.Exceptions;
using OrganizationNotificationService.Features.SendNotification;
using OrganizationNotificationService.Models;

namespace OrganizationNotificationService.Brokers.Notification;

public class EmailNotificationBroker : INotificationBroker
{
    /// <summary>
    /// The email account that will be used to send the emails, e.g. no-reply@breakingnews.com
    /// </summary>
    private readonly string _sender;

    public EmailNotificationBroker(IOptionsMonitor<EmailNotificationBrokerConfiguration > optionsMonitor)
    {
        var configuration = optionsMonitor.CurrentValue;
        _sender = configuration.Sender ??
                  throw new SystemConfigurationMissingException(
                      "Something went wrong on our side, please try again later",
                      new ArgumentNullException(nameof(configuration.Sender),
                          $"Missing configuration value for {nameof(EmailNotificationBroker)}, for property {nameof(configuration.Sender)}"));
    }

    /// <summary>
    /// Converts and 
    /// </summary>
    /// <param name="applicationNotification"></param>
    /// <returns></returns>
    public async Task<Result> SendNotification(ApplicationNotification applicationNotification)
    {
        try
        {
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

            return Result.Success();
        }
        catch (TimeoutException e)
        {
            return Result.Failure(e.Message);
        }
    }
}