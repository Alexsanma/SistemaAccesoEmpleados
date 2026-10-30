using AccessControl.Domain.AccessRecords;

namespace AccessControl.Application.Features.AccessRecords.Dtos;

public sealed record AccessRecordDto(
    Guid Id,
    string CredentialCode,
    Guid? EmployeeId,
    string? EmployeeFullName,
    Guid AccessPointId,
    string? AccessPointName,
    MovementType MovementType,
    AccessResult Result,
    string? RejectionReason,
    DateTime OccurredAt);
