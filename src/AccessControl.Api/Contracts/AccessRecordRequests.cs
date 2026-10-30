using System.ComponentModel.DataAnnotations;
using AccessControl.Application.Common.Pagination;
using AccessControl.Domain.AccessRecords;

namespace AccessControl.Api.Contracts;

public sealed class RegisterAccessRequest
{
    [Required(ErrorMessage = ValidationMessages.Required)]
    [MaxLength(20, ErrorMessage = ValidationMessages.MaxLength)]
    public string CredentialCode { get; init; } = string.Empty;

    [Required(ErrorMessage = ValidationMessages.Required)]
    public Guid? AccessPointId { get; init; }

    [Required(ErrorMessage = ValidationMessages.Required)]
    public MovementType? MovementType { get; init; }
}

public sealed class GetAccessHistoryRequest
{
    public int PageNumber { get; init; } = PaginationRules.DefaultPageNumber;
    public int PageSize { get; init; } = PaginationRules.DefaultPageSize;
    public Guid? EmployeeId { get; init; }
    public DateTime? From { get; init; }
    public DateTime? To { get; init; }
    public Guid? AccessPointId { get; init; }
    public AccessResult? Result { get; init; }
}
