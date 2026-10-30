using AccessControl.Domain.Employees;

namespace AccessControl.Application.Features.Employees.Dtos;

public sealed record EmployeeDto(
    Guid Id,
    string DocumentNumber,
    string FirstName,
    string LastName,
    string FullName,
    string Email,
    string Position,
    string Department,
    EmploymentStatus Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? DeactivatedAt);
