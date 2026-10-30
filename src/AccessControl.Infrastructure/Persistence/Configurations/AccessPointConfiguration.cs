using AccessControl.Domain.AccessPoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControl.Infrastructure.Persistence.Configurations;

internal sealed class AccessPointConfiguration : IEntityTypeConfiguration<AccessPoint>
{
    public void Configure(EntityTypeBuilder<AccessPoint> builder)
    {
        builder.ToTable("AccessPoints");
        builder.HasKey(accessPoint => accessPoint.Id);
        builder.Property(accessPoint => accessPoint.Id).ValueGeneratedNever();

        builder.Property(accessPoint => accessPoint.Name).HasMaxLength(AccessPoint.NameMaxLength).IsRequired();
        builder.Property(accessPoint => accessPoint.Location).HasMaxLength(AccessPoint.LocationMaxLength).IsRequired();

        builder.Property(accessPoint => accessPoint.Type)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.HasIndex(accessPoint => accessPoint.Name).IsUnique();
    }
}
