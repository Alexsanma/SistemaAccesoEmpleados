namespace AccessControl.Application.Common.Results;

public interface IResultFactory<TSelf>
    where TSelf : IResultFactory<TSelf>
{
    static abstract TSelf Failure(IReadOnlyCollection<Error> errors, string? message);
}
