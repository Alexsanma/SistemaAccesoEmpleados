using AccessControl.Application.Common.Results;
using AccessControl.Application.Features.Credentials.Dtos;
using MediatR;

namespace AccessControl.Application.Features.Credentials.Queries.GetCredentialById;

public sealed record GetCredentialByIdQuery(Guid Id) : IRequest<Result<CredentialDto>>;
