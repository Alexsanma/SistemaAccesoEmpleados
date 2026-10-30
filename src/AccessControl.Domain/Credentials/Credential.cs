using AccessControl.Domain.Common;
using AccessControl.Domain.Employees;
using AccessControl.Domain.ValueObjects;

namespace AccessControl.Domain.Credentials;

public sealed class Credential : Entity
{
    public const int ReasonMaxLength = 250;

    private Credential()
    {
    }

    private Credential(CredentialCode code, Guid employeeId)
    {
        Code = code;
        EmployeeId = employeeId;
        Status = CredentialStatus.Active;
        IssuedAt = DateTime.UtcNow;
    }

    public CredentialCode Code { get; private set; } = null!;
    public Guid EmployeeId { get; private set; }
    public Employee? Employee { get; private set; }
    public CredentialStatus Status { get; private set; }
    public DateTime IssuedAt { get; private set; }
    public string? StatusReason { get; private set; }
    public DateTime? StatusChangedAt { get; private set; }

    public bool CanBeUsed => Status == CredentialStatus.Active;

    public static Credential Issue(Employee employee, CredentialCode code)
    {
        ArgumentNullException.ThrowIfNull(employee);

        if (!employee.IsActive)
            throw new BusinessRuleException("No se puede asignar una credencial a un empleado inactivo.");

        return new Credential(code, employee.Id);
    }

    public void Block(string reason)
    {
        if (Status == CredentialStatus.Revoked)
            throw new BusinessRuleException("Una credencial revocada no puede bloquearse.");

        if (Status == CredentialStatus.Blocked)
            throw new BusinessRuleException("La credencial ya se encuentra bloqueada.");

        ChangeStatus(CredentialStatus.Blocked, reason);
    }

    public void Unblock()
    {
        if (Status != CredentialStatus.Blocked)
            throw new BusinessRuleException("Solo es posible desbloquear credenciales bloqueadas.");

        Status = CredentialStatus.Active;
        StatusReason = null;
        StatusChangedAt = DateTime.UtcNow;
        MarkAsUpdated();
    }

    public void Revoke(string reason)
    {
        if (Status == CredentialStatus.Revoked)
            throw new BusinessRuleException("La credencial ya se encuentra revocada.");

        ChangeStatus(CredentialStatus.Revoked, reason);
    }

    private void ChangeStatus(CredentialStatus status, string reason)
    {
        StatusReason = Guard.AgainstEmpty(reason, "motivo", ReasonMaxLength);
        Status = status;
        StatusChangedAt = DateTime.UtcNow;
        MarkAsUpdated();
    }
}
