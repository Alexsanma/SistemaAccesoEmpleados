using AccessControl.Domain.Credentials;
using AccessControl.Domain.ValueObjects;

namespace AccessControl.Domain.Repositories;

public interface ICredentialRepository
{
    Task<Credential?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Credential?> GetByCodeWithEmployeeAsync(CredentialCode code, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Credential>> GetByEmployeeAsync(Guid employeeId, CancellationToken cancellationToken = default);
    Task<bool> HasNonRevokedCredentialAsync(Guid employeeId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(CredentialCode code, CancellationToken cancellationToken = default);
    Task AddAsync(Credential credential, CancellationToken cancellationToken = default);
}
