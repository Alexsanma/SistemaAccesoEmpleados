namespace AccessControl.Api.Contracts;

internal static class ValidationMessages
{
    public const string Required = "El campo {0} es obligatorio.";
    public const string MaxLength = "El campo {0} no puede superar {1} caracteres.";
    public const string LengthRange = "El campo {0} debe tener entre {2} y {1} caracteres.";
    public const string Email = "El campo {0} no tiene un formato de correo válido.";
}
