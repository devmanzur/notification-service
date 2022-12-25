namespace OrganizationNotificationService.Configurations;


/// <summary>
/// This configuration is usually setup in dependency configuration
/// </summary>
public class PushNotificationBrokerConfiguration
{
    /// <summary>
    /// The path to  google-services.json file
    /// </summary>
    public string ServiceAccountFilePath { get; set; }
}