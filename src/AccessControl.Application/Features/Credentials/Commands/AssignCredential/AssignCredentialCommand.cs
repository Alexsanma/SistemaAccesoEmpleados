using AccessControl.Application.Common.Results;
using AccessControl.Application.Features.Credentials.Dtos;
using MediatR;

namespace AccessControl.Application.Features.Credentials.Commands.AssignCredential;

public sealed record AssignCredentialCommand(Guid EmployeeId) : IRequest<Result<CredentialDto>>;
