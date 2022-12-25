namespace OrganizationNotificationService.Configurations;

/// <summary>
/// This configuration is usually setup in dependency configuration
/// </summary>
public class EmailNotificationBrokerConfiguration
{
    /// <summary>
    /// Api key generated from mailgun
    /// </summary>
    public string? ApiKey { get; set; }
    /// <summary>
    /// Our application domain
    /// </summary>
    public string? Domain { get; set; }
    /// <summary>
    /// Email address that will be used to send this
    /// </summary>
    public string? Sender { get; set; }
    

}