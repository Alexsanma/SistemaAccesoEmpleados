using AccessControl.Domain.Employees;
using AccessControl.Domain.ValueObjects;
using FluentValidation;

namespace AccessControl.Application.Features.Employees;

internal static class EmployeeValidationRules
{
    public static IRuleBuilderOptions<T, string> ValidName<T>(this IRuleBuilder<T, string> rule, string fieldName) =>
        rule.NotEmpty().WithMessage($"El campo {fieldName} es obligatorio.")
            .MaximumLength(Employee.NameMaxLength)
            .WithMessage($"El campo {fieldName} no puede superar {Employee.NameMaxLength} caracteres.");

    public static IRuleBuilderOptions<T, string> ValidEmail<T>(this IRuleBuilder<T, string> rule) =>
        rule.NotEmpty().WithMessage("El correo electrónico es obligatorio.")
            .MaximumLength(Email.MaxLength)
            .EmailAddress().WithMessage("El correo electrónico no tiene un formato válido.");

    public static IRuleBuilderOptions<T, string> ValidJobField<T>(this IRuleBuilder<T, string> rule, string fieldName) =>
        rule.NotEmpty().WithMessage($"El campo {fieldName} es obligatorio.")
            .MaximumLength(Employee.PositionMaxLength)
            .WithMessage($"El campo {fieldName} no puede superar {Employee.PositionMaxLength} caracteres.");
}
