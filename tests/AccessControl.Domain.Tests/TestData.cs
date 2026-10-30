using System.Reflection;
using AccessControl.Domain.Credentials;
using AccessControl.Domain.Employees;
using AccessControl.Domain.ValueObjects;

namespace AccessControl.Domain.Tests;

internal static class TestData
{
    public static Employee CreateEmployee(
        string documentNumber = "1036123456",
        string email = "laura.gomez@empresa.com") =>
        Employee.Create(documentNumber, "Laura", "Gómez", email, "Analista", "Bodega");

    public static Credential CreateCredential()
    {
        var employee = CreateEmployee();
        var credential = Credential.Issue(employee, CredentialCode.Generate());

        typeof(Credential)
            .GetProperty(nameof(Credential.Employee), BindingFlags.Instance | BindingFlags.Public)!
            .SetValue(credential, employee);

        return credential;
    }
}
