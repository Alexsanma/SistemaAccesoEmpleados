using AccessControl.Application.Common.Results;
using MediatR;

namespace AccessControl.Application.Features.Credentials.Commands.BlockCredential;

public sealed record BlockCredentialCommand(Guid Id, string Reason) : IRequest<Result>;
