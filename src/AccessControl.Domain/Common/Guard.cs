namespace AccessControl.Domain.Common;

internal static class Guard
{
    public static string AgainstEmpty(string? value, string fieldName, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new BusinessRuleException($"El campo {fieldName} es obligatorio.");

        var trimmed = value.Trim();
        if (trimmed.Length > maxLength)
            throw new BusinessRuleException($"El campo {fieldName} no puede superar {maxLength} caracteres.");

        return trimmed;
    }
}
