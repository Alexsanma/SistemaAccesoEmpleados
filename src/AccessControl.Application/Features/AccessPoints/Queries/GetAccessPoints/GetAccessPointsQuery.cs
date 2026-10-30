using AccessControl.Application.Common.Results;
using AccessControl.Application.Features.AccessPoints.Dtos;
using AccessControl.Domain.AccessPoints;
using MediatR;

namespace AccessControl.Application.Features.AccessPoints.Queries.GetAccessPoints;

public sealed record GetAccessPointsQuery(bool? IsEnabled = null, AccessPointType? Type = null)
    : IRequest<Result<IReadOnlyCollection<AccessPointDto>>>;
