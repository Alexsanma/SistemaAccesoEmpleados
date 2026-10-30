using AccessControl.Domain.Repositories;

namespace AccessControl.Infrastructure.Persistence;

internal sealed class UnitOfWork(AccessControlDbContext context) : IUnitOfWork
{
    public Task<int> CommitAsync(CancellationToken cancellationToken = default) =>
        context.SaveChangesAsync(cancellationToken);
}
