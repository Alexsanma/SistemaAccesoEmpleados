using AccessControl.Domain.AccessRecords;
using AccessControl.Domain.Common;
using AccessControl.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.Infrastructure.Persistence.Repositories;

internal sealed class AccessRecordRepository(AccessControlDbContext context) : IAccessRecordRepository
{
    public Task<MovementType?> GetLastAuthorizedMovementAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default) =>
        context.AccessRecords
            .AsNoTracking()
            .Where(accessRecord => accessRecord.EmployeeId == employeeId &&
                                   accessRecord.Result == AccessResult.Authorized)
            .OrderByDescending(accessRecord => accessRecord.OccurredAt)
            .Select(accessRecord => (MovementType?)accessRecord.MovementType)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<PagedList<AccessRecord>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Guid? employeeId,
        DateTime? from,
        DateTime? to,
        Guid? accessPointId,
        AccessResult? result,
        CancellationToken cancellationToken = default)
    {
        var query = context.AccessRecords.AsNoTracking();

        if (employeeId.HasValue)
            query = query.Where(accessRecord => accessRecord.EmployeeId == employeeId.Value);

        if (from.HasValue)
            query = query.Where(accessRecord => accessRecord.OccurredAt >= from.Value);

        if (to.HasValue)
            query = query.Where(accessRecord => accessRecord.OccurredAt <= to.Value);

        if (accessPointId.HasValue)
            query = query.Where(accessRecord => accessRecord.AccessPointId == accessPointId.Value);

        if (result.HasValue)
            query = query.Where(accessRecord => accessRecord.Result == result.Value);

        return query
            .Include(accessRecord => accessRecord.Employee)
            .Include(accessRecord => accessRecord.AccessPoint)
            .OrderByDescending(accessRecord => accessRecord.OccurredAt)
            .ToPagedListAsync(pageNumber, pageSize, cancellationToken);
    }

    public async Task AddAsync(AccessRecord accessRecord, CancellationToken cancellationToken = default) =>
        await context.AccessRecords.AddAsync(accessRecord, cancellationToken);
}
