using AccessControl.Application.Common.Exceptions;
using AccessControl.Application.Common.Handlers;
using AccessControl.Application.Common.Results;
using AccessControl.Domain.Common;
using AccessControl.Domain.Repositories;
using AccessControl.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccessControl.Application.Features.Employees.Commands.UpdateEmployee;

internal sealed class UpdateEmployeeUseCase(
    IEmployeeRepository employeeRepository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateEmployeeUseCase> logger)
    : BaseHandler(logger), IRequestHandler<UpdateEmployeeCommand, Result>
{
    public Task<Result> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken) =>
        ExecuteAsync(async () =>
        {
            var employee = await employeeRepository.GetByIdAsync(command.Id, cancellationToken)
                ?? throw new NotFoundException("El empleado no existe.");

            var email = Email.Create(command.Email);
            if (await employeeRepository.ExistsByEmailAsync(email, employee.Id, cancellationToken))
                throw new BusinessRuleException("Ya existe otro empleado con el correo electrónico indicado.");

            employee.UpdateInformation(
                command.FirstName,
                command.LastName,
                command.Email,
                command.Position,
                command.Department);

            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success("Empleado actualizado correctamente.");
        });
}
