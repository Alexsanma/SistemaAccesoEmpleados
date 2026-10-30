using AccessControl.Application.Features.AccessPoints.Dtos;
using AccessControl.Domain.AccessPoints;

namespace AccessControl.Application.Features.AccessPoints;

internal static class AccessPointMapperExtensions
{
    public static AccessPointDto ToDto(this AccessPoint accessPoint) =>
        new(
            accessPoint.Id,
            accessPoint.Name,
            accessPoint.Type,
            accessPoint.Location,
            accessPoint.IsEnabled,
            accessPoint.CreatedAt,
            accessPoint.UpdatedAt);
}
