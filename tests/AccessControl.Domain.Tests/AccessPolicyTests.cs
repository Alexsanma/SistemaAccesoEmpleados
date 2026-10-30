using AccessControl.Domain.AccessPoints;
using AccessControl.Domain.AccessRecords;

namespace AccessControl.Domain.Tests;

public sealed class AccessPolicyTests
{
    private readonly AccessPoint _accessPoint = AccessPoint.Create("Recepción", AccessPointType.Reception, "Piso 1");

    [Fact]
    public void Evaluate_EntryWithValidConditions_IsAuthorized()
    {
        var credential = TestData.CreateCredential();

        var decision = AccessPolicy.Evaluate(credential, credential.Employee, _accessPoint, MovementType.Entry, null);

        Assert.True(decision.IsAuthorized);
        Assert.Null(decision.RejectionReason);
    }

    [Fact]
    public void Evaluate_ExitAfterEntry_IsAuthorized()
    {
        var credential = TestData.CreateCredential();

        var decision = AccessPolicy.Evaluate(
            credential, credential.Employee, _accessPoint, MovementType.Exit, MovementType.Entry);

        Assert.True(decision.IsAuthorized);
    }

    [Fact]
    public void Evaluate_UnknownCredential_IsRejected()
    {
        var decision = AccessPolicy.Evaluate(null, null, _accessPoint, MovementType.Entry, null);

        Assert.False(decision.IsAuthorized);
        Assert.NotNull(decision.RejectionReason);
    }

    [Fact]
    public void Evaluate_BlockedCredential_IsRejected()
    {
        var credential = TestData.CreateCredential();
        credential.Block("Pérdida");

        var decision = AccessPolicy.Evaluate(credential, credential.Employee, _accessPoint, MovementType.Entry, null);

        Assert.False(decision.IsAuthorized);
    }

    [Fact]
    public void Evaluate_RevokedCredential_IsRejected()
    {
        var credential = TestData.CreateCredential();
        credential.Revoke("Retiro");

        var decision = AccessPolicy.Evaluate(credential, credential.Employee, _accessPoint, MovementType.Entry, null);

        Assert.False(decision.IsAuthorized);
    }

    [Fact]
    public void Evaluate_InactiveEmployee_IsRejected()
    {
        var credential = TestData.CreateCredential();
        credential.Employee!.Deactivate();

        var decision = AccessPolicy.Evaluate(credential, credential.Employee, _accessPoint, MovementType.Entry, null);

        Assert.False(decision.IsAuthorized);
    }

    [Fact]
    public void Evaluate_DisabledAccessPoint_IsRejected()
    {
        var credential = TestData.CreateCredential();
        _accessPoint.Disable();

        var decision = AccessPolicy.Evaluate(credential, credential.Employee, _accessPoint, MovementType.Entry, null);

        Assert.False(decision.IsAuthorized);
    }

    [Fact]
    public void Evaluate_DuplicatedEntry_IsRejected()
    {
        var credential = TestData.CreateCredential();

        var decision = AccessPolicy.Evaluate(
            credential, credential.Employee, _accessPoint, MovementType.Entry, MovementType.Entry);

        Assert.False(decision.IsAuthorized);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(MovementType.Exit)]
    public void Evaluate_ExitWithoutPreviousEntry_IsRejected(MovementType? lastMovement)
    {
        var credential = TestData.CreateCredential();

        var decision = AccessPolicy.Evaluate(
            credential, credential.Employee, _accessPoint, MovementType.Exit, lastMovement);

        Assert.False(decision.IsAuthorized);
    }

    [Fact]
    public void RejectedRecord_KeepsRejectionReason()
    {
        var record = AccessRecord.Rejected("crd-x", null, null, _accessPoint.Id, MovementType.Entry, "No registrada");

        Assert.Equal(AccessResult.Rejected, record.Result);
        Assert.Equal("CRD-X", record.CredentialCode);
        Assert.Equal("No registrada", record.RejectionReason);
    }
}
