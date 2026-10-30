using AccessControl.Domain.Common;
using AccessControl.Domain.ValueObjects;

namespace AccessControl.Domain.Employees;

public sealed class Employee : Entity
{
    public const int NameMaxLength = 100;
    public const int PositionMaxLength = 100;
    public const int DepartmentMaxLength = 100;

    private Employee()
    {
    }

    private Employee(
        DocumentNumber documentNumber,
        string firstName,
        string lastName,
        Email email,
        string position,
        string department)
    {
        DocumentNumber = documentNumber;
        SetPersonalData(firstName, lastName, email);
        SetJobData(position, department);
        Status = EmploymentStatus.Active;
    }

    public DocumentNumber DocumentNumber { get; private set; } = null!;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public Email Email { get; private set; } = null!;
    public string Position { get; private set; } = string.Empty;
    public string Department { get; private set; } = string.Empty;
    public EmploymentStatus Status { get; private set; }
    public DateTime? DeactivatedAt { get; private set; }

    public string FullName => $"{FirstName} {LastName}";
    public bool IsActive => Status == EmploymentStatus.Active;

    public static Employee Create(
        string documentNumber,
        string firstName,
        string lastName,
        string email,
        string position,
        string department) =>
        new(DocumentNumber.Create(documentNumber), firstName, lastName, Email.Create(email), position, department);

    public void UpdateInformation(string firstName, string lastName, string email, string position, string department)
    {
        if (!IsActive)
            throw new BusinessRuleException("No es posible actualizar la información de un empleado inactivo.");

        SetPersonalData(firstName, lastName, Email.Create(email));
        SetJobData(position, department);
        MarkAsUpdated();
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new BusinessRuleException("El empleado ya se encuentra inactivo.");

        Status = EmploymentStatus.Inactive;
        DeactivatedAt = DateTime.UtcNow;
        MarkAsUpdated();
    }

    public void Reactivate()
    {
        if (IsActive)
            throw new BusinessRuleException("El empleado ya se encuentra activo.");

        Status = EmploymentStatus.Active;
        DeactivatedAt = null;
        MarkAsUpdated();
    }

    private void SetPersonalData(string firstName, string lastName, Email email)
    {
        FirstName = Guard.AgainstEmpty(firstName, "nombres", NameMaxLength);
        LastName = Guard.AgainstEmpty(lastName, "apellidos", NameMaxLength);
        Email = email;
    }

    private void SetJobData(string position, string department)
    {
        Position = Guard.AgainstEmpty(position, "cargo", PositionMaxLength);
        Department = Guard.AgainstEmpty(department, "área organizacional", DepartmentMaxLength);
    }
}
