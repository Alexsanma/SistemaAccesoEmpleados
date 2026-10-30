using AccessControl.Application.Common.Exceptions;
using AccessControl.Application.Common.Handlers;
using AccessControl.Application.Common.Results;
using AccessControl.Application.Features.Credentials.Dtos;
using AccessControl.Domain.Repositories;
using AccessControl.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccessControl.Application.Features.Credentials.Queries.GetCredentialByCode;

internal sealed class GetCredentialByCodeUseCase(
    ICredentialRepository credentialRepository,
    ILogger<GetCredentialByCodeUseCase> logger)
    : BaseHandler(logger), IRequestHandler<GetCredentialByCodeQuery, Result<CredentialDto>>
{
    public Task<Result<CredentialDto>> Handle(GetCredentialByCodeQuery query, CancellationToken cancellationToken) =>
        ExecuteAsync(async () =>
        {
            var code = CredentialCode.Create(query.Code);
            var credential = await credentialRepository.GetByCodeWithEmployeeAsync(code, cancellationToken)
                ?? throw new NotFoundException("La credencial no existe.");

            return Result<CredentialDto>.Success(credential.ToDto());
        });
}
