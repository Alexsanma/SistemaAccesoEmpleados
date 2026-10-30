using AccessControl.Domain.AccessRecords;
using AccessControl.Domain.Credentials;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControl.Infrastructure.Persistence.Configurations;

internal sealed class AccessRecordConfiguration : IEntityTypeConfiguration<AccessRecord>
{
    public void Configure(EntityTypeBuilder<AccessRecord> builder)
    {
        builder.ToTable("AccessRecords");
        builder.HasKey(accessRecord => accessRecord.Id);
        builder.Property(accessRecord => accessRecord.Id).ValueGeneratedNever();

        builder.Property(accessRecord => accessRecord.CredentialCode)
            .HasMaxLength(AccessRecord.CredentialCodeMaxLength)
            .IsRequired();

        builder.Property(accessRecord => accessRecord.MovementType)
            .HasConversion<string>()
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(accessRecord => accessRecord.Result)
            .HasConversion<string>()
            .HasMaxLength(15)
            .IsRequired();

        builder.Property(accessRecord => accessRecord.RejectionReason)
            .HasMaxLength(AccessRecord.RejectionReasonMaxLength);

        builder.HasOne<Credential>()
            .WithMany()
            .HasForeignKey(accessRecord => accessRecord.CredentialId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(accessRecord => accessRecord.Employee)
            .WithMany()
            .HasForeignKey(accessRecord => accessRecord.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(accessRecord => accessRecord.AccessPoint)
            .WithMany()
            .HasForeignKey(accessRecord => accessRecord.AccessPointId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(accessRecord => new { accessRecord.EmployeeId, accessRecord.OccurredAt });
        builder.HasIndex(accessRecord => new { accessRecord.AccessPointId, accessRecord.OccurredAt });
        builder.HasIndex(accessRecord => accessRecord.OccurredAt);
    }
}
