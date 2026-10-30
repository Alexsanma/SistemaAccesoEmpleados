using AccessControl.Application.Common.Exceptions;
using AccessControl.Application.Common.Results;
using AccessControl.Domain.Common;
using Microsoft.Extensions.Logging;

namespace AccessControl.Application.Common.Handlers;

public abstract class BaseHandler(ILogger logger)
{
    private const string UnexpectedErrorMessage = "Ocurrió un error inesperado al procesar la solicitud.";

    protected async Task<Result<T>> ExecuteAsync<T>(Func<Task<Result<T>>> action)
    {
        try
        {
            return await action();
        }
        catch (BusinessRuleException exception)
        {
            return Result<T>.Failure(Error.Business(exception.Message));
        }
        catch (NotFoundException exception)
        {
            return Result<T>.Failure(Error.NotFound(exception.Message));
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled exception in {Handler}", GetType().Name);
            return Result<T>.Failure(Error.Unexpected(UnexpectedErrorMessage));
        }
    }

    protected async Task<Result> ExecuteAsync(Func<Task<Result>> action)
    {
        try
        {
            return await action();
        }
        catch (BusinessRuleException exception)
        {
            return Result.Failure(Error.Business(exception.Message));
        }
        catch (NotFoundException exception)
        {
            return Result.Failure(Error.NotFound(exception.Message));
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled exception in {Handler}", GetType().Name);
            return Result.Failure(Error.Unexpected(UnexpectedErrorMessage));
        }
    }
}
