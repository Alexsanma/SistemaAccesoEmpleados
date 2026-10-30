using AccessControl.Api.Contracts;
using AccessControl.Api.Responses;
using AccessControl.Application.Features.Credentials.Commands.AssignCredential;
using AccessControl.Application.Features.Credentials.Dtos;
using AccessControl.Application.Features.Credentials.Queries.GetEmployeeCredentials;
using AccessControl.Application.Features.Employees.Commands.CreateEmployee;
using AccessControl.Application.Features.Employees.Commands.DeactivateEmployee;
using AccessControl.Application.Features.Employees.Commands.UpdateEmployee;
using AccessControl.Application.Features.Employees.Dtos;
using AccessControl.Application.Features.Employees.Queries.GetEmployeeById;
using AccessControl.Application.Features.Employees.Queries.GetEmployees;
using AccessControl.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.Api.Controllers;

[Route("api/employees")]
public sealed class EmployeesController(ISender sender) : ApiController
{
    /// <summary>Registra un nuevo empleado (RF-01).</summary>
    [HttpPost]
    [ProducesResponseType<ApiResponse<Guid>>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateEmployeeCommand(
            request.DocumentNumber,
            request.FirstName,
            request.LastName,
            request.Email,
            request.Position,
            request.Department);

        return ToActionResult(await sender.Send(command, cancellationToken), StatusCodes.Status201Created);
    }

    /// <summary>Consulta los empleados de forma paginada, con filtros opcionales.</summary>
    [HttpGet]
    [ProducesResponseType<ApiResponse<PagedList<EmployeeDto>>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList([FromQuery] GetEmployeesRequest request, CancellationToken cancellationToken)
    {
        var query = new GetEmployeesQuery(
            request.PageNumber,
            request.PageSize,
            request.Search,
            request.Department,
            request.Status);

        return ToActionResult(await sender.Send(query, cancellationToken));
    }

    /// <summary>Consulta un empleado por su identificador.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<ApiResponse<EmployeeDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken) =>
        ToActionResult(await sender.Send(new GetEmployeeByIdQuery(id), cancellationToken));

    /// <summary>Actualiza la información de un empleado activo.</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateEmployeeRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateEmployeeCommand(
            id,
            request.FirstName,
            request.LastName,
            request.Email,
            request.Position,
            request.Department);

        return ToActionResult(await sender.Send(command, cancellationToken));
    }

    /// <summary>Desactiva el vínculo laboral de un empleado.</summary>
    [HttpPatch("{id:guid}/deactivate")]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken) =>
        ToActionResult(await sender.Send(new DeactivateEmployeeCommand(id), cancellationToken));

    /// <summary>Asigna una nueva credencial de acceso al empleado (RF-02).</summary>
    [HttpPost("{id:guid}/credentials")]
    [ProducesResponseType<ApiResponse<CredentialDto>>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignCredential(Guid id, CancellationToken cancellationToken) =>
        ToActionResult(
            await sender.Send(new AssignCredentialCommand(id), cancellationToken),
            StatusCodes.Status201Created);

    /// <summary>Consulta las credenciales asignadas a un empleado.</summary>
    [HttpGet("{id:guid}/credentials")]
    [ProducesResponseType<ApiResponse<IReadOnlyCollection<CredentialDto>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCredentials(Guid id, CancellationToken cancellationToken) =>
        ToActionResult(await sender.Send(new GetEmployeeCredentialsQuery(id), cancellationToken));
}
