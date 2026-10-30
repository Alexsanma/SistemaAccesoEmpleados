using AccessControl.Domain.AccessRecords;
using AccessControl.Domain.Common;

namespace AccessControl.Domain.Repositories;

public interface IAccessRecordRepository
{
    Task<MovementType?> GetLastAuthorizedMovementAsync(Guid employeeId, CancellationToken cancellationToken = default);
    Task<PagedList<AccessRecord>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Guid? employeeId,
        DateTime? from,
        DateTime? to,
        Guid? accessPointId,
        AccessResult? result,
        CancellationToken cancellationToken = default);
    Task AddAsync(AccessRecord accessRecord, CancellationToken cancellationToken = default);
}
