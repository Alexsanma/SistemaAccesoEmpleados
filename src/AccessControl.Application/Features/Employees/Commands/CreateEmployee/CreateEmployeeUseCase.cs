using AccessControl.Application.Common.Handlers;
using AccessControl.Application.Common.Results;
using AccessControl.Domain.Common;
using AccessControl.Domain.Employees;
using AccessControl.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccessControl.Application.Features.Employees.Commands.CreateEmployee;

internal sealed class CreateEmployeeUseCase(
    IEmployeeRepository employeeRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateEmployeeUseCase> logger)
    : BaseHandler(logger), IRequestHandler<CreateEmployeeCommand, Result<Guid>>
{
    public Task<Result<Guid>> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken) =>
        ExecuteAsync(async () =>
        {
            var employee = Employee.Create(
                command.DocumentNumber,
                command.FirstName,
                command.LastName,
                command.Email,
                command.Position,
                command.Department);

            if (await employeeRepository.ExistsByDocumentNumberAsync(employee.DocumentNumber, cancellationToken))
                throw new BusinessRuleException("Ya existe un empleado con la identificación indicada.");

            if (await employeeRepository.ExistsByEmailAsync(employee.Email, cancellationToken: cancellationToken))
                throw new BusinessRuleException("Ya existe un empleado con el correo electrónico indicado.");

            await employeeRepository.AddAsync(employee, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            return Result<Guid>.Success(employee.Id, "Empleado registrado correctamente.");
        });
}
