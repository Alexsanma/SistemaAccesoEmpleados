using AccessControl.Application.Common.Results;
using MediatR;

namespace AccessControl.Application.Features.Employees.Commands.DeactivateEmployee;

public sealed record DeactivateEmployeeCommand(Guid Id) : IRequest<Result>;
