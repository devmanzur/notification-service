using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizationNotificationService.Models;

namespace OrganizationNotificationService.Brokers.Persistence.Configurations;
/// <summary>
/// Configures the ApplicationNotification model
/// </summary>
public class ApplicationNotificationConfiguration : IEntityTypeConfiguration<ApplicationNotification>
{
    public void Configure(EntityTypeBuilder<ApplicationNotification> builder)
    {
        builder.Property(x => x.ContentType).IsRequired().HasConversion<string>().HasMaxLength(100);
        builder.Property(x => x.Status).IsRequired().HasConversion<string>().HasMaxLength(50);
        builder.Property(x => x.Type).IsRequired().HasConversion<string>().HasMaxLength(50);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Recipient).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Body).IsRequired();

    }
}