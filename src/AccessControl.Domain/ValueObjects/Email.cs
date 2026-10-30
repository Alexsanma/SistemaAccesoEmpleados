using System.Text.RegularExpressions;
using AccessControl.Domain.Common;

namespace AccessControl.Domain.ValueObjects;

public sealed partial record Email
{
    public const int MaxLength = 150;

    private Email(string value) => Value = value;

    public string Value { get; }

    public static Email Create(string? value)
    {
        var normalized = Guard.AgainstEmpty(value, "correo electrónico", MaxLength).ToLowerInvariant();

        if (!EmailRegex().IsMatch(normalized))
            throw new BusinessRuleException("El correo electrónico no tiene un formato válido.");

        return new Email(normalized);
    }

    public override string ToString() => Value;

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();
}
