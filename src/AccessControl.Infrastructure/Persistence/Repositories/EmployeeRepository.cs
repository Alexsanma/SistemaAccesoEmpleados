using AccessControl.Domain.Common;
using AccessControl.Domain.Employees;
using AccessControl.Domain.Repositories;
using AccessControl.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.Infrastructure.Persistence.Repositories;

internal sealed class EmployeeRepository(AccessControlDbContext context) : IEmployeeRepository
{
    public Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        context.Employees.FirstOrDefaultAsync(employee => employee.Id == id, cancellationToken);

    public Task<bool> ExistsByDocumentNumberAsync(
        DocumentNumber documentNumber,
        CancellationToken cancellationToken = default) =>
        context.Employees.AnyAsync(employee => employee.DocumentNumber == documentNumber, cancellationToken);

    public Task<bool> ExistsByEmailAsync(
        Email email,
        Guid? excludedEmployeeId = null,
        CancellationToken cancellationToken = default) =>
        context.Employees.AnyAsync(
            employee => employee.Email == email && employee.Id != excludedEmployeeId,
            cancellationToken);

    public Task<PagedList<Employee>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? search,
        string? department,
        EmploymentStatus? status,
        CancellationToken cancellationToken = default)
    {
        var query = context.Employees.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            var documentNumber = TryCreateDocumentNumber(term);

            query = query.Where(employee =>
                employee.FirstName.Contains(term) ||
                employee.LastName.Contains(term) ||
                (documentNumber != null && employee.DocumentNumber == documentNumber));
        }

        if (!string.IsNullOrWhiteSpace(department))
        {
            var normalizedDepartment = department.Trim();
            query = query.Where(employee => employee.Department == normalizedDepartment);
        }

        if (status.HasValue)
            query = query.Where(employee => employee.Status == status.Value);

        return query
            .OrderBy(employee => employee.LastName)
            .ThenBy(employee => employee.FirstName)
            .ToPagedListAsync(pageNumber, pageSize, cancellationToken);
    }

    public async Task AddAsync(Employee employee, CancellationToken cancellationToken = default) =>
        await context.Employees.AddAsync(employee, cancellationToken);

    private static DocumentNumber? TryCreateDocumentNumber(string value)
    {
        try
        {
            return DocumentNumber.Create(value);
        }
        catch (BusinessRuleException)
        {
            return null;
        }
    }
}
