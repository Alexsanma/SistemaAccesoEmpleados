namespace AccessControl.Application.Common.Results;

public sealed class Result : IResultFactory<Result>
{
    private Result(bool isSuccess, IReadOnlyCollection<Error> errors, string? message)
    {
        IsSuccess = isSuccess;
        Errors = errors;
        Message = message;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public IReadOnlyCollection<Error> Errors { get; }
    public string? Message { get; }

    public static Result Success(string? message = null) => new(true, [], message);

    public static Result Failure(Error error, string? message = null) => new(false, [error], message);

    public static Result Failure(IReadOnlyCollection<Error> errors, string? message) => new(false, errors, message);
}

public sealed class Result<T> : IResultFactory<Result<T>>
{
    private Result(bool isSuccess, T? value, IReadOnlyCollection<Error> errors, string? message)
    {
        IsSuccess = isSuccess;
        Value = value;
        Errors = errors;
        Message = message;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public IReadOnlyCollection<Error> Errors { get; }
    public string? Message { get; }

    public static Result<T> Success(T value, string? message = null) => new(true, value, [], message);

    public static Result<T> Failure(Error error, string? message = null) => new(false, default, [error], message);

    public static Result<T> Failure(IReadOnlyCollection<Error> errors, string? message) =>
        new(false, default, errors, message);
}
