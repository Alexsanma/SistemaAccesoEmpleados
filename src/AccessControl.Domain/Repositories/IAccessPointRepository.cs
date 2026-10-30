using AccessControl.Domain.AccessPoints;

namespace AccessControl.Domain.Repositories;

public interface IAccessPointRepository
{
    Task<AccessPoint?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<AccessPoint>> GetAllAsync(
        bool? isEnabled,
        AccessPointType? type,
        CancellationToken cancellationToken = default);
    Task AddAsync(AccessPoint accessPoint, CancellationToken cancellationToken = default);
}
