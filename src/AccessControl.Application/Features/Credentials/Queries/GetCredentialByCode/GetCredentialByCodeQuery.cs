using AccessControl.Application.Common.Results;
using AccessControl.Application.Features.Credentials.Dtos;
using MediatR;

namespace AccessControl.Application.Features.Credentials.Queries.GetCredentialByCode;

public sealed record GetCredentialByCodeQuery(string Code) : IRequest<Result<CredentialDto>>;
