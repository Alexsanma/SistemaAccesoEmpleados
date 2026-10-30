using AccessControl.Domain.AccessPoints;
using AccessControl.Domain.Credentials;
using AccessControl.Domain.Employees;

namespace AccessControl.Domain.AccessRecords;

public static class AccessPolicy
{
    public static AccessDecision Evaluate(
        Credential? credential,
        Employee? employee,
        AccessPoint accessPoint,
        MovementType movementType,
        MovementType? lastAuthorizedMovement)
    {
        if (!Enum.IsDefined(movementType))
            return AccessDecision.Deny("El tipo de movimiento no es válido.");

        if (credential is null || employee is null)
            return AccessDecision.Deny("La credencial no está registrada en el sistema.");

        if (!accessPoint.IsEnabled)
            return AccessDecision.Deny("El punto de acceso se encuentra deshabilitado.");

        if (credential.Status == CredentialStatus.Blocked)
            return AccessDecision.Deny("La credencial se encuentra bloqueada.");

        if (credential.Status == CredentialStatus.Revoked)
            return AccessDecision.Deny("La credencial se encuentra revocada.");

        if (!employee.IsActive)
            return AccessDecision.Deny("El empleado tiene el vínculo laboral inactivo.");

        if (movementType == MovementType.Entry && lastAuthorizedMovement == MovementType.Entry)
            return AccessDecision.Deny("El empleado ya registra una entrada sin salida.");

        if (movementType == MovementType.Exit && lastAuthorizedMovement != MovementType.Entry)
            return AccessDecision.Deny("El empleado no registra una entrada previa.");

        return AccessDecision.Allow();
    }
}
