using AccessControl.Api.Responses;
using AccessControl.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.Api.Controllers;

[ApiController]
[Produces("application/json")]
public abstract class ApiController : ControllerBase
{
    protected IActionResult ToActionResult<T>(Result<T> result, int successStatusCode = StatusCodes.Status200OK)
    {
        if (!ModelState.IsValid)
            return ValidationProblemFromModelState();

        if (result.IsFailure)
            return ToErrorActionResult(result.Errors, result.Message);

        return StatusCode(successStatusCode, ApiResponse<T>.Ok(result.Value, result.Message));
    }

    protected IActionResult ToActionResult(Result result, int successStatusCode = StatusCodes.Status200OK)
    {
        if (!ModelState.IsValid)
            return ValidationProblemFromModelState();

        if (result.IsFailure)
            return ToErrorActionResult(result.Errors, result.Message);

        return StatusCode(successStatusCode, ApiResponse<object>.Ok(null, result.Message));
    }

    private IActionResult ValidationProblemFromModelState() =>
        BadRequest(ApiResponse<object>.Fail(
            ModelStateResponseFactory.ToApiErrors(ModelState),
            ModelStateResponseFactory.ValidationMessage));

    private IActionResult ToErrorActionResult(IReadOnlyCollection<Error> errors, string? message)
    {
        var dominantType = errors.FirstOrDefault()?.Type ?? ErrorType.Unexpected;
        var response = ApiResponse<object>.Fail(ApiErrorMapper.From(errors), message);

        var statusCode = dominantType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Business => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        return StatusCode(statusCode, response);
    }
}
