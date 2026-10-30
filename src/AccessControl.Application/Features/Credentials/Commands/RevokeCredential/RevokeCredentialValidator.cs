using AccessControl.Domain.Credentials;
using FluentValidation;

namespace AccessControl.Application.Features.Credentials.Commands.RevokeCredential;

internal sealed class RevokeCredentialValidator : AbstractValidator<RevokeCredentialCommand>
{
    public RevokeCredentialValidator()
    {
        RuleFor(command => command.Id).NotEmpty().WithMessage("El identificador de la credencial es obligatorio.");
        RuleFor(command => command.Reason)
            .NotEmpty().WithMessage("El motivo de la revocación es obligatorio.")
            .MaximumLength(Credential.ReasonMaxLength);
    }
}
