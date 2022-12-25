using Microsoft.EntityFrameworkCore;
using OrganizationNotificationService.BackgroundServices;
using OrganizationNotificationService.Brokers.Notification;
using OrganizationNotificationService.Brokers.Persistence;
using OrganizationNotificationService.Features.AddNotification;

namespace OrganizationNotificationService.Configurations;

public static class ConfigurationExtensions
{
    public static void AddNotificationInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Inject notification brokers
        services.AddScoped<EmailNotificationBroker>();
        services.AddScoped<PushNotificationBroker>();
        // Inject notification service
        services.AddScoped<NotificationService>();
        // Inject db context
        services.AddDbContext<NotificationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("NotificationServiceDatabase"));
        });
        // Inject broker configurations
        EmailNotificationBrokerConfiguration emailConfig = new EmailNotificationBrokerConfiguration();
        configuration.GetSection("EmailClient").Bind(emailConfig);
        PushNotificationBrokerConfiguration fcmConfig = new PushNotificationBrokerConfiguration();
        configuration.GetSection("FCM").Bind(fcmConfig);
        
        services.AddSingleton(emailConfig);
        services.AddSingleton(fcmConfig);
        
        services.AddHostedService<ApplicationDataSeedBackgroundService>();
    }
}