using AccessControl.Application.Features.AccessRecords.Dtos;
using AccessControl.Domain.AccessPoints;
using AccessControl.Domain.AccessRecords;
using AccessControl.Domain.Employees;

namespace AccessControl.Application.Features.AccessRecords;

internal static class AccessRecordMapperExtensions
{
    public static AccessRecordDto ToDto(this AccessRecord accessRecord) =>
        accessRecord.ToDto(accessRecord.Employee, accessRecord.AccessPoint);

    public static AccessRecordDto ToDto(this AccessRecord accessRecord, Employee? employee, AccessPoint? accessPoint) =>
        new(
            accessRecord.Id,
            accessRecord.CredentialCode,
            accessRecord.EmployeeId,
            employee?.FullName,
            accessRecord.AccessPointId,
            accessPoint?.Name,
            accessRecord.MovementType,
            accessRecord.Result,
            accessRecord.RejectionReason,
            accessRecord.OccurredAt);
}
