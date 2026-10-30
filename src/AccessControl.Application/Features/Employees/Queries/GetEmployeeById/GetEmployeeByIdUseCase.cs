using AccessControl.Application.Common.Exceptions;
using AccessControl.Application.Common.Handlers;
using AccessControl.Application.Common.Results;
using AccessControl.Application.Features.Employees.Dtos;
using AccessControl.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccessControl.Application.Features.Employees.Queries.GetEmployeeById;

internal sealed class GetEmployeeByIdUseCase(
    IEmployeeRepository employeeRepository,
    ILogger<GetEmployeeByIdUseCase> logger)
    : BaseHandler(logger), IRequestHandler<GetEmployeeByIdQuery, Result<EmployeeDto>>
{
    public Task<Result<EmployeeDto>> Handle(GetEmployeeByIdQuery query, CancellationToken cancellationToken) =>
        ExecuteAsync(async () =>
        {
            var employee = await employeeRepository.GetByIdAsync(query.Id, cancellationToken)
                ?? throw new NotFoundException("El empleado no existe.");

            return Result<EmployeeDto>.Success(employee.ToDto());
        });
}
