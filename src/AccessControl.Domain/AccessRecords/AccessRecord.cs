using AccessControl.Domain.AccessPoints;
using AccessControl.Domain.Common;
using AccessControl.Domain.Employees;

namespace AccessControl.Domain.AccessRecords;

public sealed class AccessRecord : Entity
{
    public const int CredentialCodeMaxLength = 20;
    public const int RejectionReasonMaxLength = 250;

    private AccessRecord()
    {
    }

    private AccessRecord(
        string credentialCode,
        Guid? credentialId,
        Guid? employeeId,
        Guid accessPointId,
        MovementType movementType,
        AccessResult result,
        string? rejectionReason)
    {
        var code = Guard.AgainstEmpty(credentialCode, "código de credencial", int.MaxValue).ToUpperInvariant();
        CredentialCode = code.Length > CredentialCodeMaxLength ? code[..CredentialCodeMaxLength] : code;
        CredentialId = credentialId;
        EmployeeId = employeeId;
        AccessPointId = accessPointId;
        MovementType = movementType;
        Result = result;
        RejectionReason = rejectionReason;
        OccurredAt = DateTime.UtcNow;
    }

    public string CredentialCode { get; private set; } = string.Empty;
    public Guid? CredentialId { get; private set; }
    public Guid? EmployeeId { get; private set; }
    public Employee? Employee { get; private set; }
    public Guid AccessPointId { get; private set; }
    public AccessPoint? AccessPoint { get; private set; }
    public MovementType MovementType { get; private set; }
    public AccessResult Result { get; private set; }
    public string? RejectionReason { get; private set; }
    public DateTime OccurredAt { get; private set; }

    public bool IsAuthorized => Result == AccessResult.Authorized;

    public static AccessRecord Authorized(
        string credentialCode,
        Guid credentialId,
        Guid employeeId,
        Guid accessPointId,
        MovementType movementType) =>
        new(credentialCode, credentialId, employeeId, accessPointId, movementType, AccessResult.Authorized, null);

    public static AccessRecord Rejected(
        string credentialCode,
        Guid? credentialId,
        Guid? employeeId,
        Guid accessPointId,
        MovementType movementType,
        string reason) =>
        new(credentialCode, credentialId, employeeId, accessPointId, movementType, AccessResult.Rejected,
            Guard.AgainstEmpty(reason, "motivo de rechazo", RejectionReasonMaxLength));
}
