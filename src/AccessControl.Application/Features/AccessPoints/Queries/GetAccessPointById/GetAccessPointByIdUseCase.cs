using AccessControl.Application.Common.Exceptions;
using AccessControl.Application.Common.Handlers;
using AccessControl.Application.Common.Results;
using AccessControl.Application.Features.AccessPoints.Dtos;
using AccessControl.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccessControl.Application.Features.AccessPoints.Queries.GetAccessPointById;

internal sealed class GetAccessPointByIdUseCase(
    IAccessPointRepository accessPointRepository,
    ILogger<GetAccessPointByIdUseCase> logger)
    : BaseHandler(logger), IRequestHandler<GetAccessPointByIdQuery, Result<AccessPointDto>>
{
    public Task<Result<AccessPointDto>> Handle(GetAccessPointByIdQuery query, CancellationToken cancellationToken) =>
        ExecuteAsync(async () =>
        {
            var accessPoint = await accessPointRepository.GetByIdAsync(query.Id, cancellationToken)
                ?? throw new NotFoundException("El punto de acceso no existe.");

            return Result<AccessPointDto>.Success(accessPoint.ToDto());
        });
}
