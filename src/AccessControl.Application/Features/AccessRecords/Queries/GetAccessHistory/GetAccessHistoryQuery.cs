using AccessControl.Application.Common.Pagination;
using AccessControl.Application.Common.Results;
using AccessControl.Application.Features.AccessRecords.Dtos;
using AccessControl.Domain.AccessRecords;
using AccessControl.Domain.Common;
using MediatR;

namespace AccessControl.Application.Features.AccessRecords.Queries.GetAccessHistory;

public sealed record GetAccessHistoryQuery(
    int PageNumber = PaginationRules.DefaultPageNumber,
    int PageSize = PaginationRules.DefaultPageSize,
    Guid? EmployeeId = null,
    DateTime? From = null,
    DateTime? To = null,
    Guid? AccessPointId = null,
    AccessResult? Result = null) : IRequest<Result<PagedList<AccessRecordDto>>>;
