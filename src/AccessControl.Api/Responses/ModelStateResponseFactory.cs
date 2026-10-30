using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AccessControl.Api.Responses;

internal static class ModelStateResponseFactory
{
    public const string ValidationMessage = "La solicitud contiene errores de validación.";

    public static IReadOnlyCollection<ApiError> ToApiErrors(ModelStateDictionary modelState) =>
        modelState
            .Where(entry => entry.Value is { Errors.Count: > 0 })
            .SelectMany(entry => entry.Value!.Errors.Select(error => ToApiError(entry.Key, error)))
            .ToList();

    public static IActionResult Create(ActionContext context) =>
        new BadRequestObjectResult(
            ApiResponse<object>.Fail(ToApiErrors(context.ModelState), ValidationMessage));

    private static ApiError ToApiError(string key, ModelError error)
    {
        var field = key.TrimStart('$', '.');
        if (string.IsNullOrEmpty(field))
            field = "Body";

        var isFormatError = key.StartsWith('$') || error.Exception is not null;
        var description = isFormatError || string.IsNullOrWhiteSpace(error.ErrorMessage)
            ? $"El valor enviado para '{field}' no tiene un formato válido."
            : error.ErrorMessage;

        return new ApiError(field, description, "Validation");
    }
}
