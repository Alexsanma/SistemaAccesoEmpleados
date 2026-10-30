using AccessControl.Application.Common.Exceptions;
using AccessControl.Application.Common.Handlers;
using AccessControl.Application.Common.Results;
using AccessControl.Application.Features.AccessRecords.Dtos;
using AccessControl.Domain.AccessRecords;
using AccessControl.Domain.Repositories;
using AccessControl.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccessControl.Application.Features.AccessRecords.Commands.RegisterAccess;

internal sealed class RegisterAccessUseCase(
    ICredentialRepository credentialRepository,
    IAccessPointRepository accessPointRepository,
    IAccessRecordRepository accessRecordRepository,
    IUnitOfWork unitOfWork,
    ILogger<RegisterAccessUseCase> logger)
    : BaseHandler(logger), IRequestHandler<RegisterAccessCommand, Result<AccessRecordDto>>
{
    public Task<Result<AccessRecordDto>> Handle(RegisterAccessCommand command, CancellationToken cancellationToken) =>
        ExecuteAsync(async () =>
        {
            var accessPoint = await accessPointRepository.GetByIdAsync(command.AccessPointId, cancellationToken)
                ?? throw new NotFoundException("El punto de acceso no existe.");

            var credentialCode = CredentialCode.Create(command.CredentialCode);
            var credential = await credentialRepository.GetByCodeWithEmployeeAsync(credentialCode, cancellationToken);
            var employee = credential?.Employee;

            var lastAuthorizedMovement = employee is null
                ? null
                : await accessRecordRepository.GetLastAuthorizedMovementAsync(employee.Id, cancellationToken);

            var decision = AccessPolicy.Evaluate(
                credential,
                employee,
                accessPoint,
                command.MovementType,
                lastAuthorizedMovement);

            var accessRecord = decision.IsAuthorized
                ? AccessRecord.Authorized(
                    credentialCode.Value, credential!.Id, employee!.Id, accessPoint.Id, command.MovementType)
                : AccessRecord.Rejected(
                    credentialCode.Value, credential?.Id, employee?.Id, accessPoint.Id, command.MovementType,
                    decision.RejectionReason!);

            await accessRecordRepository.AddAsync(accessRecord, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            var message = decision.IsAuthorized
                ? "Acceso autorizado y registrado."
                : $"Acceso rechazado: {decision.RejectionReason}";

            return Result<AccessRecordDto>.Success(accessRecord.ToDto(employee, accessPoint), message);
        });
}
