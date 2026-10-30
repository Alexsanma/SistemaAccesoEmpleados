using AccessControl.Application.Common.Handlers;
using AccessControl.Application.Common.Results;
using AccessControl.Application.Features.Employees.Dtos;
using AccessControl.Domain.Common;
using AccessControl.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccessControl.Application.Features.Employees.Queries.GetEmployees;

internal sealed class GetEmployeesUseCase(
    IEmployeeRepository employeeRepository,
    ILogger<GetEmployeesUseCase> logger)
    : BaseHandler(logger), IRequestHandler<GetEmployeesQuery, Result<PagedList<EmployeeDto>>>
{
    public Task<Result<PagedList<EmployeeDto>>> Handle(GetEmployeesQuery query, CancellationToken cancellationToken) =>
        ExecuteAsync(async () =>
        {
            var employees = await employeeRepository.GetPagedAsync(
                query.PageNumber,
                query.PageSize,
                query.Search,
                query.Department,
                query.Status,
                cancellationToken);

            return Result<PagedList<EmployeeDto>>.Success(employees.Map(employee => employee.ToDto()));
        });
}
