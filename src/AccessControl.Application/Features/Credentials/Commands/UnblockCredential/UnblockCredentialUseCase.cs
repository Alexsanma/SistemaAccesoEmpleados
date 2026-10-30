using AccessControl.Application.Common.Exceptions;
using AccessControl.Application.Common.Handlers;
using AccessControl.Application.Common.Results;
using AccessControl.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccessControl.Application.Features.Credentials.Commands.UnblockCredential;

internal sealed class UnblockCredentialUseCase(
    ICredentialRepository credentialRepository,
    IUnitOfWork unitOfWork,
    ILogger<UnblockCredentialUseCase> logger)
    : BaseHandler(logger), IRequestHandler<UnblockCredentialCommand, Result>
{
    public Task<Result> Handle(UnblockCredentialCommand command, CancellationToken cancellationToken) =>
        ExecuteAsync(async () =>
        {
            var credential = await credentialRepository.GetByIdAsync(command.Id, cancellationToken)
                ?? throw new NotFoundException("La credencial no existe.");

            credential.Unblock();
            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success("Credencial desbloqueada correctamente.");
        });
}
