using AccessControl.Application.Common.Exceptions;
using AccessControl.Application.Common.Handlers;
using AccessControl.Application.Common.Results;
using AccessControl.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccessControl.Application.Features.Credentials.Commands.RevokeCredential;

internal sealed class RevokeCredentialUseCase(
    ICredentialRepository credentialRepository,
    IUnitOfWork unitOfWork,
    ILogger<RevokeCredentialUseCase> logger)
    : BaseHandler(logger), IRequestHandler<RevokeCredentialCommand, Result>
{
    public Task<Result> Handle(RevokeCredentialCommand command, CancellationToken cancellationToken) =>
        ExecuteAsync(async () =>
        {
            var credential = await credentialRepository.GetByIdAsync(command.Id, cancellationToken)
                ?? throw new NotFoundException("La credencial no existe.");

            credential.Revoke(command.Reason);
            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success("Credencial revocada correctamente.");
        });
}
