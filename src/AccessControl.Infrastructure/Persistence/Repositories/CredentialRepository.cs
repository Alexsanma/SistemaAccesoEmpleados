using AccessControl.Domain.Credentials;
using AccessControl.Domain.Repositories;
using AccessControl.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.Infrastructure.Persistence.Repositories;

internal sealed class CredentialRepository(AccessControlDbContext context) : ICredentialRepository
{
    public Task<Credential?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        context.Credentials.FirstOrDefaultAsync(credential => credential.Id == id, cancellationToken);

    public Task<Credential?> GetByCodeWithEmployeeAsync(
        CredentialCode code,
        CancellationToken cancellationToken = default) =>
        context.Credentials
            .Include(credential => credential.Employee)
            .FirstOrDefaultAsync(credential => credential.Code == code, cancellationToken);

    public async Task<IReadOnlyCollection<Credential>> GetByEmployeeAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default) =>
        await context.Credentials
            .AsNoTracking()
            .Where(credential => credential.EmployeeId == employeeId)
            .OrderByDescending(credential => credential.IssuedAt)
            .ToListAsync(cancellationToken);

    public Task<bool> HasNonRevokedCredentialAsync(Guid employeeId, CancellationToken cancellationToken = default) =>
        context.Credentials.AnyAsync(
            credential => credential.EmployeeId == employeeId && credential.Status != CredentialStatus.Revoked,
            cancellationToken);

    public Task<bool> ExistsByCodeAsync(CredentialCode code, CancellationToken cancellationToken = default) =>
        context.Credentials.AnyAsync(credential => credential.Code == code, cancellationToken);

    public async Task AddAsync(Credential credential, CancellationToken cancellationToken = default) =>
        await context.Credentials.AddAsync(credential, cancellationToken);
}
