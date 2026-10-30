using AccessControl.Application.Common.Handlers;
using AccessControl.Application.Common.Results;
using AccessControl.Application.Features.AccessPoints.Dtos;
using AccessControl.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccessControl.Application.Features.AccessPoints.Queries.GetAccessPoints;

internal sealed class GetAccessPointsUseCase(
    IAccessPointRepository accessPointRepository,
    ILogger<GetAccessPointsUseCase> logger)
    : BaseHandler(logger), IRequestHandler<GetAccessPointsQuery, Result<IReadOnlyCollection<AccessPointDto>>>
{
    public Task<Result<IReadOnlyCollection<AccessPointDto>>> Handle(
        GetAccessPointsQuery query,
        CancellationToken cancellationToken) =>
        ExecuteAsync(async () =>
        {
            var accessPoints = await accessPointRepository.GetAllAsync(query.IsEnabled, query.Type, cancellationToken);

            return Result<IReadOnlyCollection<AccessPointDto>>.Success(
                accessPoints.Select(accessPoint => accessPoint.ToDto()).ToList());
        });
}
