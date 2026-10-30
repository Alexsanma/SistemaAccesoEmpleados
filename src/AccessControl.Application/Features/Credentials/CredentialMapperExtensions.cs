using AccessControl.Application.Features.Credentials.Dtos;
using AccessControl.Domain.Credentials;

namespace AccessControl.Application.Features.Credentials;

internal static class CredentialMapperExtensions
{
    public static CredentialDto ToDto(this Credential credential) =>
        new(
            credential.Id,
            credential.Code.Value,
            credential.EmployeeId,
            credential.Status,
            credential.CanBeUsed,
            credential.IssuedAt,
            credential.StatusReason,
            credential.StatusChangedAt);
}
