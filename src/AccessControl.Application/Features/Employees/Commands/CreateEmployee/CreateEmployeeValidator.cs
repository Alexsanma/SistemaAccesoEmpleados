using AccessControl.Domain.ValueObjects;
using FluentValidation;

namespace AccessControl.Application.Features.Employees.Commands.CreateEmployee;

internal sealed class CreateEmployeeValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeValidator()
    {
        RuleFor(command => command.DocumentNumber)
            .NotEmpty().WithMessage("La identificación es obligatoria.")
            .Length(DocumentNumber.MinLength, DocumentNumber.MaxLength)
            .WithMessage($"La identificación debe tener entre {DocumentNumber.MinLength} y {DocumentNumber.MaxLength} caracteres.");

        RuleFor(command => command.FirstName).ValidName("nombres");
        RuleFor(command => command.LastName).ValidName("apellidos");
        RuleFor(command => command.Email).ValidEmail();
        RuleFor(command => command.Position).ValidJobField("cargo");
        RuleFor(command => command.Department).ValidJobField("área organizacional");
    }
}
