using AccessControl.Domain.AccessPoints;
using FluentValidation;

namespace AccessControl.Application.Features.AccessPoints.Commands.CreateAccessPoint;

internal sealed class CreateAccessPointValidator : AbstractValidator<CreateAccessPointCommand>
{
    public CreateAccessPointValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty().WithMessage("El nombre del punto de acceso es obligatorio.")
            .MaximumLength(AccessPoint.NameMaxLength);

        RuleFor(command => command.Type)
            .IsInEnum().WithMessage("El tipo de punto de acceso no es válido.");

        RuleFor(command => command.Location)
            .NotEmpty().WithMessage("La ubicación del punto de acceso es obligatoria.")
            .MaximumLength(AccessPoint.LocationMaxLength);
    }
}
