using AccessControl.Application.Common.Exceptions;
using AccessControl.Application.Common.Handlers;
using AccessControl.Application.Common.Results;
using AccessControl.Application.Features.Credentials.Dtos;
using AccessControl.Domain.Common;
using AccessControl.Domain.Credentials;
using AccessControl.Domain.Repositories;
using AccessControl.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccessControl.Application.Features.Credentials.Commands.AssignCredential;

internal sealed class AssignCredentialUseCase(
    IEmployeeRepository employeeRepository,
    ICredentialRepository credentialRepository,
    IUnitOfWork unitOfWork,
    ILogger<AssignCredentialUseCase> logger)
    : BaseHandler(logger), IRequestHandler<AssignCredentialCommand, Result<CredentialDto>>
{
    private const int MaxCodeGenerationAttempts = 5;

    public Task<Result<CredentialDto>> Handle(AssignCredentialCommand command, CancellationToken cancellationToken) =>
        ExecuteAsync(async () =>
        {
            var employee = await employeeRepository.GetByIdAsync(command.EmployeeId, cancellationToken)
                ?? throw new NotFoundException("El empleado no existe.");

            if (await credentialRepository.HasNonRevokedCredentialAsync(employee.Id, cancellationToken))
                throw new BusinessRuleException(
                    "El empleado ya tiene una credencial vigente o bloqueada. Debe revocarla antes de asignar una nueva.");

            var code = await GenerateUniqueCodeAsync(cancellationToken);
            var credential = Credential.Issue(employee, code);

            await credentialRepository.AddAsync(credential, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            return Result<CredentialDto>.Success(credential.ToDto(), "Credencial asignada correctamente.");
        });

    private async Task<CredentialCode> GenerateUniqueCodeAsync(CancellationToken cancellationToken)
    {
        for (var attempt = 0; attempt < MaxCodeGenerationAttempts; attempt++)
        {
            var code = CredentialCode.Generate();
            if (!await credentialRepository.ExistsByCodeAsync(code, cancellationToken))
                return code;
        }

        throw new InvalidOperationException("No fue posible generar un código de credencial único.");
    }
}
