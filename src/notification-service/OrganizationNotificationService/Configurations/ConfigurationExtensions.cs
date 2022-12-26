using EasyNetQ;
using Microsoft.EntityFrameworkCore;
using OrganizationNotificationService.BackgroundServices;
using OrganizationNotificationService.BackgroundServices.Jobs;
using OrganizationNotificationService.Brokers.Notification;
using OrganizationNotificationService.Brokers.Persistence;
using OrganizationNotificationService.Features.SendNotification;
using Quartz;

namespace OrganizationNotificationService.Configurations;

public static class ConfigurationExtensions
{
    private const string QueueConnectionStringKey = "ConnectionString";
    /// <summary>
    /// Injects all the dependencies required to run the notification service
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
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
        
        services.AddQuartz(q =>
        {
            // Create a "key" for the job
            var jobKey = new JobKey(nameof(SendPendingNotificationsJob));

            q.UseMicrosoftDependencyInjectionJobFactory();

            // Register the job with the DI container
            q.AddJob<SendPendingNotificationsJob>(opts => opts.WithIdentity(jobKey));

            // Create a trigger for the job
            q.AddTrigger(opts => opts
                .ForJob(jobKey) // link to the CompleteTodoItemJob
                .WithIdentity($"{jobKey}-trigger") 
                .WithCronSchedule("0 */2 * * * ?")); // run every 3 minutes

        });
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        
        //easyNetQ
        var bus = RabbitHutch.CreateBus(configuration.GetSection("RabbitMq")[QueueConnectionStringKey] ??
                                        throw new ArgumentNullException(QueueConnectionStringKey,
                                            "QueueConnectionString not defined within the provided configuration body"));
        services.AddSingleton(bus);
        services.AddHostedService<MessageQueueSubscriber>();
    }
}