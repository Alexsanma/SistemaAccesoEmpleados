using AccessControl.Domain.Common;
using AccessControl.Domain.Employees;

namespace AccessControl.Domain.Tests;

public sealed class EmployeeTests
{
    [Fact]
    public void Create_WithValidData_ReturnsActiveEmployeeWithNormalizedValues()
    {
        var employee = TestData.CreateEmployee(documentNumber: " ab12345 ", email: "Laura.Gomez@Empresa.com");

        Assert.Equal(EmploymentStatus.Active, employee.Status);
        Assert.Equal("AB12345", employee.DocumentNumber.Value);
        Assert.Equal("laura.gomez@empresa.com", employee.Email.Value);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("12-345")]
    [InlineData("")]
    public void Create_WithInvalidDocumentNumber_ThrowsBusinessRuleException(string documentNumber)
    {
        Assert.Throws<BusinessRuleException>(() => TestData.CreateEmployee(documentNumber: documentNumber));
    }

    [Fact]
    public void Create_WithInvalidEmail_ThrowsBusinessRuleException()
    {
        Assert.Throws<BusinessRuleException>(() => TestData.CreateEmployee(email: "correo-invalido"));
    }

    [Fact]
    public void Deactivate_ActiveEmployee_ChangesStatusAndSetsDate()
    {
        var employee = TestData.CreateEmployee();

        employee.Deactivate();

        Assert.False(employee.IsActive);
        Assert.NotNull(employee.DeactivatedAt);
    }

    [Fact]
    public void Deactivate_InactiveEmployee_ThrowsBusinessRuleException()
    {
        var employee = TestData.CreateEmployee();
        employee.Deactivate();

        Assert.Throws<BusinessRuleException>(employee.Deactivate);
    }

    [Fact]
    public void UpdateInformation_InactiveEmployee_ThrowsBusinessRuleException()
    {
        var employee = TestData.CreateEmployee();
        employee.Deactivate();

        Assert.Throws<BusinessRuleException>(() =>
            employee.UpdateInformation("Ana", "Pérez", "ana@empresa.com", "Analista", "Bodega"));
    }
}
