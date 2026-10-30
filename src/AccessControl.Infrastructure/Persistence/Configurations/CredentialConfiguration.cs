using AccessControl.Domain.Credentials;
using AccessControl.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControl.Infrastructure.Persistence.Configurations;

internal sealed class CredentialConfiguration : IEntityTypeConfiguration<Credential>
{
    public void Configure(EntityTypeBuilder<Credential> builder)
    {
        builder.ToTable("Credentials");
        builder.HasKey(credential => credential.Id);
        builder.Property(credential => credential.Id).ValueGeneratedNever();

        builder.Property(credential => credential.Code)
            .HasConversion(code => code.Value, value => CredentialCode.Create(value))
            .HasMaxLength(CredentialCode.MaxLength)
            .IsRequired();

        builder.Property(credential => credential.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(credential => credential.StatusReason).HasMaxLength(Credential.ReasonMaxLength);

        builder.HasOne(credential => credential.Employee)
            .WithMany()
            .HasForeignKey(credential => credential.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(credential => credential.Code).IsUnique();
        builder.HasIndex(credential => new { credential.EmployeeId, credential.Status });
    }
}
