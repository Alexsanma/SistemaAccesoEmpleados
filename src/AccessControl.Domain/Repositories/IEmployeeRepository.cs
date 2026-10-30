using AccessControl.Domain.Common;
using AccessControl.Domain.Employees;
using AccessControl.Domain.ValueObjects;

namespace AccessControl.Domain.Repositories;

public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByDocumentNumberAsync(DocumentNumber documentNumber, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(Email email, Guid? excludedEmployeeId = null, CancellationToken cancellationToken = default);
    Task<PagedList<Employee>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? search,
        string? department,
        EmploymentStatus? status,
        CancellationToken cancellationToken = default);
    Task AddAsync(Employee employee, CancellationToken cancellationToken = default);
}
