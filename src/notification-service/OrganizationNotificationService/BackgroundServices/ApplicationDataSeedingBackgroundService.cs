using Microsoft.EntityFrameworkCore;
using OrganizationNotificationService.Brokers.Persistence;

namespace OrganizationNotificationService.BackgroundServices;

public class ApplicationDataSeedBackgroundService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;


    public ApplicationDataSeedBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        await InitializeAuthDatabaseSeeding(cancellationToken, scope);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    
    private async Task InitializeAuthDatabaseSeeding(CancellationToken cancellationToken, IServiceScope scope)
    {
        var context = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
        if (context.Database.IsSqlServer())
        {
            // Runs pending migrations
            await context.Database.MigrateAsync(cancellationToken);
        }
    }
}