using AccessControl.Application.Common.Results;
using AccessControl.Application.Features.Employees.Dtos;
using MediatR;

namespace AccessControl.Application.Features.Employees.Queries.GetEmployeeById;

public sealed record GetEmployeeByIdQuery(Guid Id) : IRequest<Result<EmployeeDto>>;
