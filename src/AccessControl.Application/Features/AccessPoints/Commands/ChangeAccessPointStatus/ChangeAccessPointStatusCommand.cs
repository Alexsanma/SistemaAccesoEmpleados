using AccessControl.Application.Common.Results;
using MediatR;

namespace AccessControl.Application.Features.AccessPoints.Commands.ChangeAccessPointStatus;

public sealed record ChangeAccessPointStatusCommand(Guid Id, bool IsEnabled) : IRequest<Result>;
