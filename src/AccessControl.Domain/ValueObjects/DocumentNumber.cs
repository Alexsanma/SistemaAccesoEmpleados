using AccessControl.Domain.Common;

namespace AccessControl.Domain.ValueObjects;

public sealed record DocumentNumber
{
    public const int MinLength = 5;
    public const int MaxLength = 20;

    private DocumentNumber(string value) => Value = value;

    public string Value { get; }

    public static DocumentNumber Create(string? value)
    {
        var normalized = Guard.AgainstEmpty(value, "identificación", MaxLength).ToUpperInvariant();

        if (normalized.Length < MinLength || !normalized.All(char.IsLetterOrDigit))
            throw new BusinessRuleException(
                $"La identificación debe ser alfanumérica y tener entre {MinLength} y {MaxLength} caracteres.");

        return new DocumentNumber(normalized);
    }

    public override string ToString() => Value;
}
