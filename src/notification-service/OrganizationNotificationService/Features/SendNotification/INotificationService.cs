namespace OrganizationNotificationService.Features.SendNotification;

public interface INotificationService
{
    Task SendEmailAsync(string to, string subject, string body);
    Task SendPushNotificationAsync(string deviceId, string message);
}