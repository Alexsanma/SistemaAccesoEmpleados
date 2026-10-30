namespace AccessControl.Application.Common.Results;

public sealed record Error(string Code, string Description, ErrorType Type)
{
    public static Error Validation(string code, string description) => new(code, description, ErrorType.Validation);
    public static Error Business(string description) => new("BusinessRule", description, ErrorType.Business);
    public static Error NotFound(string description) => new("NotFound", description, ErrorType.NotFound);
    public static Error Unexpected(string description) => new("Unexpected", description, ErrorType.Unexpected);
}
