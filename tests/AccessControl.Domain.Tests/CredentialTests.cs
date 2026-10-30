using AccessControl.Domain.Common;
using AccessControl.Domain.Credentials;
using AccessControl.Domain.ValueObjects;

namespace AccessControl.Domain.Tests;

public sealed class CredentialTests
{
    [Fact]
    public void Issue_ForActiveEmployee_ReturnsUsableCredential()
    {
        var employee = TestData.CreateEmployee();

        var credential = Credential.Issue(employee, CredentialCode.Generate());

        Assert.Equal(CredentialStatus.Active, credential.Status);
        Assert.True(credential.CanBeUsed);
        Assert.Equal(employee.Id, credential.EmployeeId);
        Assert.StartsWith(CredentialCode.Prefix, credential.Code.Value);
    }

    [Fact]
    public void Issue_ForInactiveEmployee_ThrowsBusinessRuleException()
    {
        var employee = TestData.CreateEmployee();
        employee.Deactivate();

        Assert.Throws<BusinessRuleException>(() => Credential.Issue(employee, CredentialCode.Generate()));
    }

    [Fact]
    public void Block_ActiveCredential_CannotBeUsed()
    {
        var credential = TestData.CreateCredential();

        credential.Block("Pérdida reportada");

        Assert.Equal(CredentialStatus.Blocked, credential.Status);
        Assert.False(credential.CanBeUsed);
        Assert.Equal("Pérdida reportada", credential.StatusReason);
    }

    [Fact]
    public void Unblock_BlockedCredential_CanBeUsedAgain()
    {
        var credential = TestData.CreateCredential();
        credential.Block("Revisión");

        credential.Unblock();

        Assert.True(credential.CanBeUsed);
        Assert.Null(credential.StatusReason);
    }

    [Fact]
    public void Revoke_IsFinal_CannotBeBlockedOrUnblocked()
    {
        var credential = TestData.CreateCredential();

        credential.Revoke("Retiro");

        Assert.Equal(CredentialStatus.Revoked, credential.Status);
        Assert.Throws<BusinessRuleException>(() => credential.Block("Otro motivo"));
        Assert.Throws<BusinessRuleException>(credential.Unblock);
        Assert.Throws<BusinessRuleException>(() => credential.Revoke("Otra vez"));
    }

    [Fact]
    public void Block_WithoutReason_ThrowsBusinessRuleException()
    {
        var credential = TestData.CreateCredential();

        Assert.Throws<BusinessRuleException>(() => credential.Block(" "));
    }
}
