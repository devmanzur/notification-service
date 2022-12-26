using Microsoft.EntityFrameworkCore;
using OrganizationNotificationService.Brokers.Persistence;

namespace OrganizationNotificationService.BackgroundServices;

public class ApplicationDataSeedBackgroundService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ApplicationDataSeedBackgroundService> _logger;


    public ApplicationDataSeedBackgroundService(IServiceProvider serviceProvider,ILogger<ApplicationDataSeedBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("execution started");

        using var scope = _serviceProvider.CreateScope();
        await InitializeAuthDatabaseSeeding(cancellationToken, scope);
        
        _logger.LogInformation("execution completed");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    
    /// <summary>
    /// Finds any pending migrations and applies them, this is useful when we have pending migrations that need to be run at startups
    /// Such as when starting the application on a new environment
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="scope"></param>
    private async Task InitializeAuthDatabaseSeeding(CancellationToken cancellationToken, IServiceScope scope)
    {
        var context = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
        var pendingMigrations = await context.Database.GetPendingMigrationsAsync(cancellationToken);
        if (pendingMigrations.Any())
        {
            _logger.LogWarning("Running pending migrations");
            // Runs pending migrations
            await context.Database.MigrateAsync(cancellationToken);
            _logger.LogInformation("Pending migrations applied");
        }
    }
}