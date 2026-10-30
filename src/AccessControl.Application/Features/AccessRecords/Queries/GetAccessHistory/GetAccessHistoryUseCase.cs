using AccessControl.Application.Common.Handlers;
using AccessControl.Application.Common.Results;
using AccessControl.Application.Features.AccessRecords.Dtos;
using AccessControl.Domain.Common;
using AccessControl.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccessControl.Application.Features.AccessRecords.Queries.GetAccessHistory;

internal sealed class GetAccessHistoryUseCase(
    IAccessRecordRepository accessRecordRepository,
    ILogger<GetAccessHistoryUseCase> logger)
    : BaseHandler(logger), IRequestHandler<GetAccessHistoryQuery, Result<PagedList<AccessRecordDto>>>
{
    public Task<Result<PagedList<AccessRecordDto>>> Handle(
        GetAccessHistoryQuery query,
        CancellationToken cancellationToken) =>
        ExecuteAsync(async () =>
        {
            var from = query.From?.ToUniversalTime();
            var to = query.To is { TimeOfDay.Ticks: 0 } endDate
                ? endDate.AddDays(1).AddTicks(-1).ToUniversalTime()
                : query.To?.ToUniversalTime();

            var records = await accessRecordRepository.GetPagedAsync(
                query.PageNumber,
                query.PageSize,
                query.EmployeeId,
                from,
                to,
                query.AccessPointId,
                query.Result,
                cancellationToken);

            return Result<PagedList<AccessRecordDto>>.Success(records.Map(record => record.ToDto()));
        });
}
