using AccessControl.Api.Contracts;
using AccessControl.Api.Responses;
using AccessControl.Application.Features.AccessRecords.Commands.RegisterAccess;
using AccessControl.Application.Features.AccessRecords.Dtos;
using AccessControl.Application.Features.AccessRecords.Queries.GetAccessHistory;
using AccessControl.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.Api.Controllers;

[Route("api/access-records")]
public sealed class AccessRecordsController(ISender sender) : ApiController
{
    /// <summary>
    /// Procesa una solicitud de entrada o salida (RF-03). Tanto los accesos autorizados como los
    /// rechazados quedan registrados; el campo "result" indica el resultado de la validación.
    /// </summary>
    [HttpPost]
    [ProducesResponseType<ApiResponse<AccessRecordDto>>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Register(
        [FromBody] RegisterAccessRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterAccessCommand(
            request.CredentialCode,
            request.AccessPointId!.Value,
            request.MovementType!.Value);

        return ToActionResult(await sender.Send(command, cancellationToken), StatusCodes.Status201Created);
    }

    /// <summary>
    /// Consulta paginada del historial de accesos por empleado, rango de fechas,
    /// punto de acceso y resultado de validación (RF-04).
    /// </summary>
    [HttpGet]
    [ProducesResponseType<ApiResponse<PagedList<AccessRecordDto>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetHistory(
        [FromQuery] GetAccessHistoryRequest request,
        CancellationToken cancellationToken)
    {
        var query = new GetAccessHistoryQuery(
            request.PageNumber,
            request.PageSize,
            request.EmployeeId,
            request.From,
            request.To,
            request.AccessPointId,
            request.Result);

        return ToActionResult(await sender.Send(query, cancellationToken));
    }
}
