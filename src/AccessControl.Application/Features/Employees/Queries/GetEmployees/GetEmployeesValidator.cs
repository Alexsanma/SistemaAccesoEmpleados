using AccessControl.Application.Common.Pagination;
using FluentValidation;

namespace AccessControl.Application.Features.Employees.Queries.GetEmployees;

internal sealed class GetEmployeesValidator : AbstractValidator<GetEmployeesQuery>
{
    public GetEmployeesValidator()
    {
        this.ApplyPaginationRules(query => query.PageNumber, query => query.PageSize);

        RuleFor(query => query.Status)
            .IsInEnum().WithMessage("El estado laboral no es válido.");
    }
}
