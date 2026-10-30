using AccessControl.Application.Features.Employees.Dtos;
using AccessControl.Domain.Employees;

namespace AccessControl.Application.Features.Employees;

internal static class EmployeeMapperExtensions
{
    public static EmployeeDto ToDto(this Employee employee) =>
        new(
            employee.Id,
            employee.DocumentNumber.Value,
            employee.FirstName,
            employee.LastName,
            employee.FullName,
            employee.Email.Value,
            employee.Position,
            employee.Department,
            employee.Status,
            employee.CreatedAt,
            employee.UpdatedAt,
            employee.DeactivatedAt);
}
