using AccessControl.Domain.Credentials;
using FluentValidation;

namespace AccessControl.Application.Features.Credentials.Commands.BlockCredential;

internal sealed class BlockCredentialValidator : AbstractValidator<BlockCredentialCommand>
{
    public BlockCredentialValidator()
    {
        RuleFor(command => command.Id).NotEmpty().WithMessage("El identificador de la credencial es obligatorio.");
        RuleFor(command => command.Reason)
            .NotEmpty().WithMessage("El motivo del bloqueo es obligatorio.")
            .MaximumLength(Credential.ReasonMaxLength);
    }
}
