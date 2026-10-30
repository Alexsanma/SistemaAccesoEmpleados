using AccessControl.Application.Common.Results;
using MediatR;

namespace AccessControl.Application.Features.Employees.Commands.CreateEmployee;

public sealed record CreateEmployeeCommand(
    string DocumentNumber,
    string FirstName,
    string LastName,
    string Email,
    string Position,
    string Department) : IRequest<Result<Guid>>;
