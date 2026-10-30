using AccessControl.Domain.AccessPoints;

namespace AccessControl.Application.Features.AccessPoints.Dtos;

public sealed record AccessPointDto(
    Guid Id,
    string Name,
    AccessPointType Type,
    string Location,
    bool IsEnabled,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
