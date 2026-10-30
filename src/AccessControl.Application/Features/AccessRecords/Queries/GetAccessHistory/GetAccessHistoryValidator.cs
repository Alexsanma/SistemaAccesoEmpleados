using AccessControl.Application.Common.Pagination;
using FluentValidation;

namespace AccessControl.Application.Features.AccessRecords.Queries.GetAccessHistory;

internal sealed class GetAccessHistoryValidator : AbstractValidator<GetAccessHistoryQuery>
{
    public GetAccessHistoryValidator()
    {
        this.ApplyPaginationRules(query => query.PageNumber, query => query.PageSize);

        RuleFor(query => query)
            .Must(query => query.From is null || query.To is null || query.From <= query.To)
            .OverridePropertyName("From")
            .WithMessage("La fecha inicial debe ser menor o igual a la fecha final.");

        RuleFor(query => query.Result)
            .IsInEnum().WithMessage("El resultado de validación debe ser Authorized o Rejected.");
    }
}
