namespace AccessControl.Domain.AccessRecords;

public sealed record AccessDecision(bool IsAuthorized, string? RejectionReason)
{
    public static AccessDecision Allow() => new(true, null);
    public static AccessDecision Deny(string reason) => new(false, reason);
}
