using AccessControl.Application.Common.Results;
using MediatR;

namespace AccessControl.Application.Features.Credentials.Commands.RevokeCredential;

public sealed record RevokeCredentialCommand(Guid Id, string Reason) : IRequest<Result>;
