using AccessControl.Domain.AccessPoints;
using AccessControl.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.Infrastructure.Persistence.Repositories;

internal sealed class AccessPointRepository(AccessControlDbContext context) : IAccessPointRepository
{
    public Task<AccessPoint?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        context.AccessPoints.FirstOrDefaultAsync(accessPoint => accessPoint.Id == id, cancellationToken);

    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default) =>
        context.AccessPoints.AnyAsync(accessPoint => accessPoint.Name == name, cancellationToken);

    public async Task<IReadOnlyCollection<AccessPoint>> GetAllAsync(
        bool? isEnabled,
        AccessPointType? type,
        CancellationToken cancellationToken = default)
    {
        var query = context.AccessPoints.AsNoTracking();

        if (isEnabled.HasValue)
            query = query.Where(accessPoint => accessPoint.IsEnabled == isEnabled.Value);

        if (type.HasValue)
            query = query.Where(accessPoint => accessPoint.Type == type.Value);

        return await query.OrderBy(accessPoint => accessPoint.Name).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(AccessPoint accessPoint, CancellationToken cancellationToken = default) =>
        await context.AccessPoints.AddAsync(accessPoint, cancellationToken);
}
