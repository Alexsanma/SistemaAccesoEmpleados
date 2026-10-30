using AccessControl.Application.Common.Results;
using AccessControl.Application.Features.Credentials.Dtos;
using MediatR;

namespace AccessControl.Application.Features.Credentials.Queries.GetEmployeeCredentials;

public sealed record GetEmployeeCredentialsQuery(Guid EmployeeId) : IRequest<Result<IReadOnlyCollection<CredentialDto>>>;
