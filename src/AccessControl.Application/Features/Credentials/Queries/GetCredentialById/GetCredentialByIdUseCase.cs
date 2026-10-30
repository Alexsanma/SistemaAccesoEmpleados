using AccessControl.Application.Common.Exceptions;
using AccessControl.Application.Common.Handlers;
using AccessControl.Application.Common.Results;
using AccessControl.Application.Features.Credentials.Dtos;
using AccessControl.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccessControl.Application.Features.Credentials.Queries.GetCredentialById;

internal sealed class GetCredentialByIdUseCase(
    ICredentialRepository credentialRepository,
    ILogger<GetCredentialByIdUseCase> logger)
    : BaseHandler(logger), IRequestHandler<GetCredentialByIdQuery, Result<CredentialDto>>
{
    public Task<Result<CredentialDto>> Handle(GetCredentialByIdQuery query, CancellationToken cancellationToken) =>
        ExecuteAsync(async () =>
        {
            var credential = await credentialRepository.GetByIdAsync(query.Id, cancellationToken)
                ?? throw new NotFoundException("La credencial no existe.");

            return Result<CredentialDto>.Success(credential.ToDto());
        });
}
