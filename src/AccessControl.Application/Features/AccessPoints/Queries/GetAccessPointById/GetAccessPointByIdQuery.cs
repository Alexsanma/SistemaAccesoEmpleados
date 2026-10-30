using AccessControl.Application.Common.Results;
using AccessControl.Application.Features.AccessPoints.Dtos;
using MediatR;

namespace AccessControl.Application.Features.AccessPoints.Queries.GetAccessPointById;

public sealed record GetAccessPointByIdQuery(Guid Id) : IRequest<Result<AccessPointDto>>;
