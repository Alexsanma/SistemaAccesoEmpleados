using System.Security.Cryptography;
using AccessControl.Domain.Common;

namespace AccessControl.Domain.ValueObjects;

public sealed record CredentialCode
{
    public const string Prefix = "CRD-";
    public const int MaxLength = 20;
    private const string Alphabet = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
    private const int RandomLength = 8;

    private CredentialCode(string value) => Value = value;

    public string Value { get; }

    public static CredentialCode Create(string? value)
    {
        var normalized = Guard.AgainstEmpty(value, "código de credencial", MaxLength).ToUpperInvariant();
        return new CredentialCode(normalized);
    }

    public static CredentialCode Generate()
    {
        var suffix = RandomNumberGenerator.GetString(Alphabet, RandomLength);
        return new CredentialCode(Prefix + suffix);
    }

    public override string ToString() => Value;
}
