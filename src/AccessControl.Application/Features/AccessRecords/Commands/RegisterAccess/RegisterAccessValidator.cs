using AccessControl.Domain.ValueObjects;
using FluentValidation;

namespace AccessControl.Application.Features.AccessRecords.Commands.RegisterAccess;

internal sealed class RegisterAccessValidator : AbstractValidator<RegisterAccessCommand>
{
    public RegisterAccessValidator()
    {
        RuleFor(command => command.CredentialCode)
            .NotEmpty().WithMessage("El código de la credencial es obligatorio.")
            .MaximumLength(CredentialCode.MaxLength)
            .WithMessage($"El código de la credencial no puede superar {CredentialCode.MaxLength} caracteres.");

        RuleFor(command => command.AccessPointId)
            .NotEmpty().WithMessage("El punto de acceso es obligatorio.");

        RuleFor(command => command.MovementType)
            .IsInEnum().WithMessage("El tipo de movimiento debe ser Entry o Exit.");
    }
}
