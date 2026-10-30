using System.ComponentModel.DataAnnotations;

namespace AccessControl.Api.Contracts;

public sealed class ChangeCredentialStatusRequest
{
    [Required(ErrorMessage = ValidationMessages.Required)]
    [MaxLength(250, ErrorMessage = ValidationMessages.MaxLength)]
    public string Reason { get; init; } = string.Empty;
}
