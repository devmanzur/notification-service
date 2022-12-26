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
        services.Configure<EmailNotificationBrokerConfiguration>(configuration.GetSection("EmailClient"));
        services.Configure<PushNotificationBrokerConfiguration>(configuration.GetSection("FCM"));
        
        // Inject hosted service
        services.AddHostedService<ApplicationDataSeedBackgroundService>();
    }
}