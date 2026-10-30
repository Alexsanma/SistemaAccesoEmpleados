using AccessControl.Application.Common.Exceptions;
using AccessControl.Application.Common.Handlers;
using AccessControl.Application.Common.Results;
using AccessControl.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccessControl.Application.Features.Credentials.Commands.BlockCredential;

internal sealed class BlockCredentialUseCase(
    ICredentialRepository credentialRepository,
    IUnitOfWork unitOfWork,
    ILogger<BlockCredentialUseCase> logger)
    : BaseHandler(logger), IRequestHandler<BlockCredentialCommand, Result>
{
    public Task<Result> Handle(BlockCredentialCommand command, CancellationToken cancellationToken) =>
        ExecuteAsync(async () =>
        {
            var credential = await credentialRepository.GetByIdAsync(command.Id, cancellationToken)
                ?? throw new NotFoundException("La credencial no existe.");

            credential.Block(command.Reason);
            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success("Credencial bloqueada correctamente.");
        });
}
