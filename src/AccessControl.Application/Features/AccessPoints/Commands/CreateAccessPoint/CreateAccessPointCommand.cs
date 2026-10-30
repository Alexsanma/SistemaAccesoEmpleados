using AccessControl.Application.Common.Results;
using AccessControl.Domain.AccessPoints;
using MediatR;

namespace AccessControl.Application.Features.AccessPoints.Commands.CreateAccessPoint;

public sealed record CreateAccessPointCommand(
    string Name,
    AccessPointType Type,
    string Location) : IRequest<Result<Guid>>;
