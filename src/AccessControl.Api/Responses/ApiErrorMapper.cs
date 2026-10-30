using AccessControl.Application.Common.Results;

namespace AccessControl.Api.Responses;

internal static class ApiErrorMapper
{
    public static ApiError From(Error error) => new(error.Code, error.Description, MapType(error.Type));

    public static IReadOnlyCollection<ApiError> From(IReadOnlyCollection<Error> errors) =>
        errors.Count == 0 ? [] : errors.Select(From).ToList();

    private static string MapType(ErrorType type) => type switch
    {
        ErrorType.None => "None",
        ErrorType.Validation => "Validation",
        ErrorType.Business => "Business",
        ErrorType.NotFound => "NotFound",
        ErrorType.Unexpected => "Unexpected",
        _ => "Unknown"
    };
}
