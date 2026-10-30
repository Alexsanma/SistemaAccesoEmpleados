using AccessControl.Domain.Employees;
using AccessControl.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControl.Infrastructure.Persistence.Configurations;

internal sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");
        builder.HasKey(employee => employee.Id);
        builder.Property(employee => employee.Id).ValueGeneratedNever();

        builder.Property(employee => employee.DocumentNumber)
            .HasConversion(documentNumber => documentNumber.Value, value => DocumentNumber.Create(value))
            .HasMaxLength(DocumentNumber.MaxLength)
            .IsRequired();

        builder.Property(employee => employee.Email)
            .HasConversion(email => email.Value, value => Email.Create(value))
            .HasMaxLength(Email.MaxLength)
            .IsRequired();

        builder.Property(employee => employee.FirstName).HasMaxLength(Employee.NameMaxLength).IsRequired();
        builder.Property(employee => employee.LastName).HasMaxLength(Employee.NameMaxLength).IsRequired();
        builder.Property(employee => employee.Position).HasMaxLength(Employee.PositionMaxLength).IsRequired();
        builder.Property(employee => employee.Department).HasMaxLength(Employee.DepartmentMaxLength).IsRequired();

        builder.Property(employee => employee.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(employee => employee.DocumentNumber).IsUnique();
        builder.HasIndex(employee => employee.Email).IsUnique();
        builder.HasIndex(employee => employee.Department);
    }
}
