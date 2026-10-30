using AccessControl.Application.Common.Exceptions;
using AccessControl.Application.Common.Handlers;
using AccessControl.Application.Common.Results;
using AccessControl.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccessControl.Application.Features.Employees.Commands.DeactivateEmployee;

internal sealed class DeactivateEmployeeUseCase(
    IEmployeeRepository employeeRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeactivateEmployeeUseCase> logger)
    : BaseHandler(logger), IRequestHandler<DeactivateEmployeeCommand, Result>
{
    public Task<Result> Handle(DeactivateEmployeeCommand command, CancellationToken cancellationToken) =>
        ExecuteAsync(async () =>
        {
            var employee = await employeeRepository.GetByIdAsync(command.Id, cancellationToken)
                ?? throw new NotFoundException("El empleado no existe.");

            employee.Deactivate();
            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success("Empleado desactivado correctamente.");
        });
}
