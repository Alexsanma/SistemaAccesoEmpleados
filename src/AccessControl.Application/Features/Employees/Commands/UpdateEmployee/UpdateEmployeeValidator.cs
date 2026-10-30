using FluentValidation;

namespace AccessControl.Application.Features.Employees.Commands.UpdateEmployee;

internal sealed class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeValidator()
    {
        RuleFor(command => command.Id).NotEmpty().WithMessage("El identificador del empleado es obligatorio.");
        RuleFor(command => command.FirstName).ValidName("nombres");
        RuleFor(command => command.LastName).ValidName("apellidos");
        RuleFor(command => command.Email).ValidEmail();
        RuleFor(command => command.Position).ValidJobField("cargo");
        RuleFor(command => command.Department).ValidJobField("área organizacional");
    }
}
