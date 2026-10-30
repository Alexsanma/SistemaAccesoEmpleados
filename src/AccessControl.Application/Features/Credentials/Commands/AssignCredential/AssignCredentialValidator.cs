using FluentValidation;

namespace AccessControl.Application.Features.Credentials.Commands.AssignCredential;

internal sealed class AssignCredentialValidator : AbstractValidator<AssignCredentialCommand>
{
    public AssignCredentialValidator()
    {
        RuleFor(command => command.EmployeeId)
            .NotEmpty().WithMessage("El identificador del empleado es obligatorio.");
    }
}
