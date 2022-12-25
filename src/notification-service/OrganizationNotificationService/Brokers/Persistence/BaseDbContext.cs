using Microsoft.EntityFrameworkCore;
using OrganizationNotificationService.Models;

namespace OrganizationNotificationService.Brokers.Persistence;

public abstract class BaseDbContext<T> : DbContext where T : DbContext
{


    protected BaseDbContext(DbContextOptions<T> options) : base(options)
    {

    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        Audit();
        var changesMade = await base.SaveChangesAsync(cancellationToken);
        return changesMade;
    }

    public override int SaveChanges()
    {
        Audit();
        var changesMade = base.SaveChanges();
        return changesMade;
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        Audit();
        var changesMade = base.SaveChanges(acceptAllChangesOnSuccess);
        return changesMade;
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = new CancellationToken())
    {
        Audit();
        var changesMade = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        return changesMade;
    }

    #region Change tracking
    private void Audit()
    {
        foreach (var entry in ChangeTracker.Entries<IAuditable>().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.LastUpdatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastUpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
    }
    #endregion
}