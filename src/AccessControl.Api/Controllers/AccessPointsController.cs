using AccessControl.Api.Contracts;
using AccessControl.Api.Responses;
using AccessControl.Application.Features.AccessPoints.Commands.ChangeAccessPointStatus;
using AccessControl.Application.Features.AccessPoints.Commands.CreateAccessPoint;
using AccessControl.Application.Features.AccessPoints.Dtos;
using AccessControl.Application.Features.AccessPoints.Queries.GetAccessPointById;
using AccessControl.Application.Features.AccessPoints.Queries.GetAccessPoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.Api.Controllers;

[Route("api/access-points")]
public sealed class AccessPointsController(ISender sender) : ApiController
{
    /// <summary>Registra un punto de acceso físico (RF-06).</summary>
    [HttpPost]
    [ProducesResponseType<ApiResponse<Guid>>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateAccessPointRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateAccessPointCommand(request.Name, request.Type!.Value, request.Location);
        return ToActionResult(await sender.Send(command, cancellationToken), StatusCodes.Status201Created);
    }

    /// <summary>Consulta los puntos de acceso, filtrando opcionalmente por estado y tipo.</summary>
    [HttpGet]
    [ProducesResponseType<ApiResponse<IReadOnlyCollection<AccessPointDto>>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList(
        [FromQuery] GetAccessPointsRequest request,
        CancellationToken cancellationToken) =>
        ToActionResult(await sender.Send(new GetAccessPointsQuery(request.IsEnabled, request.Type), cancellationToken));

    /// <summary>Consulta un punto de acceso por su identificador.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<ApiResponse<AccessPointDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken) =>
        ToActionResult(await sender.Send(new GetAccessPointByIdQuery(id), cancellationToken));

    /// <summary>Habilita un punto de acceso.</summary>
    [HttpPatch("{id:guid}/enable")]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Enable(Guid id, CancellationToken cancellationToken) =>
        ToActionResult(await sender.Send(new ChangeAccessPointStatusCommand(id, true), cancellationToken));

    /// <summary>Deshabilita un punto de acceso.</summary>
    [HttpPatch("{id:guid}/disable")]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Disable(Guid id, CancellationToken cancellationToken) =>
        ToActionResult(await sender.Send(new ChangeAccessPointStatusCommand(id, false), cancellationToken));
}
