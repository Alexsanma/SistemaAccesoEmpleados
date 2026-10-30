using AccessControl.Application.Common.Results;
using MediatR;

namespace AccessControl.Application.Features.Employees.Commands.UpdateEmployee;

public sealed record UpdateEmployeeCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Position,
    string Department) : IRequest<Result>;
