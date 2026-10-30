using AccessControl.Api.Contracts;
using AccessControl.Api.Responses;
using AccessControl.Application.Features.Credentials.Commands.BlockCredential;
using AccessControl.Application.Features.Credentials.Commands.RevokeCredential;
using AccessControl.Application.Features.Credentials.Commands.UnblockCredential;
using AccessControl.Application.Features.Credentials.Dtos;
using AccessControl.Application.Features.Credentials.Queries.GetCredentialByCode;
using AccessControl.Application.Features.Credentials.Queries.GetCredentialById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.Api.Controllers;

[Route("api/credentials")]
public sealed class CredentialsController(ISender sender) : ApiController
{
    /// <summary>Consulta una credencial y su estado por identificador.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<ApiResponse<CredentialDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken) =>
        ToActionResult(await sender.Send(new GetCredentialByIdQuery(id), cancellationToken));

    /// <summary>Consulta una credencial y su estado por código.</summary>
    [HttpGet("code/{code}")]
    [ProducesResponseType<ApiResponse<CredentialDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByCode(string code, CancellationToken cancellationToken) =>
        ToActionResult(await sender.Send(new GetCredentialByCodeQuery(code), cancellationToken));

    /// <summary>Bloquea temporalmente una credencial.</summary>
    [HttpPatch("{id:guid}/block")]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Block(
        Guid id,
        [FromBody] ChangeCredentialStatusRequest request,
        CancellationToken cancellationToken) =>
        ToActionResult(await sender.Send(new BlockCredentialCommand(id, request.Reason), cancellationToken));

    /// <summary>Desbloquea una credencial bloqueada.</summary>
    [HttpPatch("{id:guid}/unblock")]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Unblock(Guid id, CancellationToken cancellationToken) =>
        ToActionResult(await sender.Send(new UnblockCredentialCommand(id), cancellationToken));

    /// <summary>Revoca definitivamente una credencial.</summary>
    [HttpPatch("{id:guid}/revoke")]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Revoke(
        Guid id,
        [FromBody] ChangeCredentialStatusRequest request,
        CancellationToken cancellationToken) =>
        ToActionResult(await sender.Send(new RevokeCredentialCommand(id, request.Reason), cancellationToken));
}
