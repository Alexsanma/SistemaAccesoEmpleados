using AccessControl.Application.Common.Pagination;
using AccessControl.Application.Common.Results;
using AccessControl.Application.Features.Employees.Dtos;
using AccessControl.Domain.Common;
using AccessControl.Domain.Employees;
using MediatR;

namespace AccessControl.Application.Features.Employees.Queries.GetEmployees;

public sealed record GetEmployeesQuery(
    int PageNumber = PaginationRules.DefaultPageNumber,
    int PageSize = PaginationRules.DefaultPageSize,
    string? Search = null,
    string? Department = null,
    EmploymentStatus? Status = null) : IRequest<Result<PagedList<EmployeeDto>>>;
