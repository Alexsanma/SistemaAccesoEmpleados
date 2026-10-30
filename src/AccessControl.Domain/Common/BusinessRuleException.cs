namespace AccessControl.Domain.Common;

public sealed class BusinessRuleException(string message) : Exception(message);
