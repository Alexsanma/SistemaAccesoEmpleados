using AccessControl.Application.Common.Results;
using MediatR;

namespace AccessControl.Application.Features.Credentials.Commands.UnblockCredential;

public sealed record UnblockCredentialCommand(Guid Id) : IRequest<Result>;
