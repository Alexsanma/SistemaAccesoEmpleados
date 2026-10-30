using System.ComponentModel.DataAnnotations;
using AccessControl.Domain.AccessPoints;

namespace AccessControl.Api.Contracts;

public sealed class CreateAccessPointRequest
{
    [Required(ErrorMessage = ValidationMessages.Required)]
    [MaxLength(100, ErrorMessage = ValidationMessages.MaxLength)]
    public string Name { get; init; } = string.Empty;

    [Required(ErrorMessage = ValidationMessages.Required)]
    public AccessPointType? Type { get; init; }

    [Required(ErrorMessage = ValidationMessages.Required)]
    [MaxLength(200, ErrorMessage = ValidationMessages.MaxLength)]
    public string Location { get; init; } = string.Empty;
}

public sealed class GetAccessPointsRequest
{
    public bool? IsEnabled { get; init; }
    public AccessPointType? Type { get; init; }
}
