using AccessControl.Application.Common.Exceptions;
using AccessControl.Application.Common.Handlers;
using AccessControl.Application.Common.Results;
using AccessControl.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccessControl.Application.Features.AccessPoints.Commands.ChangeAccessPointStatus;

internal sealed class ChangeAccessPointStatusUseCase(
    IAccessPointRepository accessPointRepository,
    IUnitOfWork unitOfWork,
    ILogger<ChangeAccessPointStatusUseCase> logger)
    : BaseHandler(logger), IRequestHandler<ChangeAccessPointStatusCommand, Result>
{
    public Task<Result> Handle(ChangeAccessPointStatusCommand command, CancellationToken cancellationToken) =>
        ExecuteAsync(async () =>
        {
            var accessPoint = await accessPointRepository.GetByIdAsync(command.Id, cancellationToken)
                ?? throw new NotFoundException("El punto de acceso no existe.");

            if (command.IsEnabled)
                accessPoint.Enable();
            else
                accessPoint.Disable();

            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success(command.IsEnabled
                ? "Punto de acceso habilitado correctamente."
                : "Punto de acceso deshabilitado correctamente.");
        });
}
