using AccessControl.Domain.Common;

namespace AccessControl.Domain.AccessPoints;

public sealed class AccessPoint : Entity
{
    public const int NameMaxLength = 100;
    public const int LocationMaxLength = 200;

    private AccessPoint()
    {
    }

    private AccessPoint(string name, AccessPointType type, string location)
    {
        if (!Enum.IsDefined(type))
            throw new BusinessRuleException("El tipo de punto de acceso no es válido.");

        Name = Guard.AgainstEmpty(name, "nombre", NameMaxLength);
        Location = Guard.AgainstEmpty(location, "ubicación", LocationMaxLength);
        Type = type;
        IsEnabled = true;
    }

    public string Name { get; private set; } = string.Empty;
    public AccessPointType Type { get; private set; }
    public string Location { get; private set; } = string.Empty;
    public bool IsEnabled { get; private set; }

    public static AccessPoint Create(string name, AccessPointType type, string location) =>
        new(name, type, location);

    public void Enable()
    {
        if (IsEnabled)
            throw new BusinessRuleException("El punto de acceso ya se encuentra habilitado.");

        IsEnabled = true;
        MarkAsUpdated();
    }

    public void Disable()
    {
        if (!IsEnabled)
            throw new BusinessRuleException("El punto de acceso ya se encuentra deshabilitado.");

        IsEnabled = false;
        MarkAsUpdated();
    }
}
