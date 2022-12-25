using Microsoft.EntityFrameworkCore;
using OrganizationNotificationService.Brokers.Persistence.Configurations;
using OrganizationNotificationService.Models;

namespace OrganizationNotificationService.Brokers.Persistence;

public class NotificationDbContext : BaseDbContext<NotificationDbContext>
{
    public DbSet<ApplicationNotification> Notifications { get; set; }

    
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("notifications");
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ApplicationNotificationConfiguration());
    }
}