using FluentValidation;

namespace AccessControl.Application.Common.Pagination;

public static class PaginationRules
{
    public const int DefaultPageNumber = 1;
    public const int DefaultPageSize = 10;
    public const int MaxPageSize = 100;

    public static void ApplyPaginationRules<T>(
        this AbstractValidator<T> validator,
        Func<T, int> pageNumber,
        Func<T, int> pageSize)
    {
        validator.RuleFor(request => pageNumber(request))
            .GreaterThan(0)
            .OverridePropertyName("PageNumber")
            .WithMessage("El número de página debe ser mayor a cero.");

        validator.RuleFor(request => pageSize(request))
            .InclusiveBetween(1, MaxPageSize)
            .OverridePropertyName("PageSize")
            .WithMessage($"El tamaño de página debe estar entre 1 y {MaxPageSize}.");
    }
}
