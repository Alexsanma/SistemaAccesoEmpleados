using AccessControl.Application.Common.Handlers;
using AccessControl.Application.Common.Results;
using AccessControl.Domain.AccessPoints;
using AccessControl.Domain.Common;
using AccessControl.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccessControl.Application.Features.AccessPoints.Commands.CreateAccessPoint;

internal sealed class CreateAccessPointUseCase(
    IAccessPointRepository accessPointRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateAccessPointUseCase> logger)
    : BaseHandler(logger), IRequestHandler<CreateAccessPointCommand, Result<Guid>>
{
    public Task<Result<Guid>> Handle(CreateAccessPointCommand command, CancellationToken cancellationToken) =>
        ExecuteAsync(async () =>
        {
            var accessPoint = AccessPoint.Create(command.Name, command.Type, command.Location);

            if (await accessPointRepository.ExistsByNameAsync(accessPoint.Name, cancellationToken))
                throw new BusinessRuleException("Ya existe un punto de acceso con el nombre indicado.");

            await accessPointRepository.AddAsync(accessPoint, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            return Result<Guid>.Success(accessPoint.Id, "Punto de acceso registrado correctamente.");
        });
}
