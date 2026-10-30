using System.ComponentModel.DataAnnotations;
using AccessControl.Application.Common.Pagination;
using AccessControl.Domain.Employees;

namespace AccessControl.Api.Contracts;

public sealed class CreateEmployeeRequest
{
    [Required(ErrorMessage = ValidationMessages.Required)]
    [StringLength(20, MinimumLength = 5, ErrorMessage = ValidationMessages.LengthRange)]
    public string DocumentNumber { get; init; } = string.Empty;

    [Required(ErrorMessage = ValidationMessages.Required)]
    [MaxLength(100, ErrorMessage = ValidationMessages.MaxLength)]
    public string FirstName { get; init; } = string.Empty;

    [Required(ErrorMessage = ValidationMessages.Required)]
    [MaxLength(100, ErrorMessage = ValidationMessages.MaxLength)]
    public string LastName { get; init; } = string.Empty;

    [Required(ErrorMessage = ValidationMessages.Required)]
    [EmailAddress(ErrorMessage = ValidationMessages.Email)]
    [MaxLength(150, ErrorMessage = ValidationMessages.MaxLength)]
    public string Email { get; init; } = string.Empty;

    [Required(ErrorMessage = ValidationMessages.Required)]
    [MaxLength(100, ErrorMessage = ValidationMessages.MaxLength)]
    public string Position { get; init; } = string.Empty;

    [Required(ErrorMessage = ValidationMessages.Required)]
    [MaxLength(100, ErrorMessage = ValidationMessages.MaxLength)]
    public string Department { get; init; } = string.Empty;
}

public sealed class UpdateEmployeeRequest
{
    [Required(ErrorMessage = ValidationMessages.Required)]
    [MaxLength(100, ErrorMessage = ValidationMessages.MaxLength)]
    public string FirstName { get; init; } = string.Empty;

    [Required(ErrorMessage = ValidationMessages.Required)]
    [MaxLength(100, ErrorMessage = ValidationMessages.MaxLength)]
    public string LastName { get; init; } = string.Empty;

    [Required(ErrorMessage = ValidationMessages.Required)]
    [EmailAddress(ErrorMessage = ValidationMessages.Email)]
    [MaxLength(150, ErrorMessage = ValidationMessages.MaxLength)]
    public string Email { get; init; } = string.Empty;

    [Required(ErrorMessage = ValidationMessages.Required)]
    [MaxLength(100, ErrorMessage = ValidationMessages.MaxLength)]
    public string Position { get; init; } = string.Empty;

    [Required(ErrorMessage = ValidationMessages.Required)]
    [MaxLength(100, ErrorMessage = ValidationMessages.MaxLength)]
    public string Department { get; init; } = string.Empty;
}

public sealed class GetEmployeesRequest
{
    public int PageNumber { get; init; } = PaginationRules.DefaultPageNumber;
    public int PageSize { get; init; } = PaginationRules.DefaultPageSize;
    public string? Search { get; init; }
    public string? Department { get; init; }
    public EmploymentStatus? Status { get; init; }
}
