using AccessControl.Domain.Credentials;

namespace AccessControl.Application.Features.Credentials.Dtos;

public sealed record CredentialDto(
    Guid Id,
    string Code,
    Guid EmployeeId,
    CredentialStatus Status,
    bool CanBeUsed,
    DateTime IssuedAt,
    string? StatusReason,
    DateTime? StatusChangedAt);
