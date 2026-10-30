using AccessControl.Application.Common.Exceptions;
using AccessControl.Application.Common.Handlers;
using AccessControl.Application.Common.Results;
using AccessControl.Application.Features.Credentials.Dtos;
using AccessControl.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccessControl.Application.Features.Credentials.Queries.GetEmployeeCredentials;

internal sealed class GetEmployeeCredentialsUseCase(
    IEmployeeRepository employeeRepository,
    ICredentialRepository credentialRepository,
    ILogger<GetEmployeeCredentialsUseCase> logger)
    : BaseHandler(logger), IRequestHandler<GetEmployeeCredentialsQuery, Result<IReadOnlyCollection<CredentialDto>>>
{
    public Task<Result<IReadOnlyCollection<CredentialDto>>> Handle(
        GetEmployeeCredentialsQuery query,
        CancellationToken cancellationToken) =>
        ExecuteAsync(async () =>
        {
            _ = await employeeRepository.GetByIdAsync(query.EmployeeId, cancellationToken)
                ?? throw new NotFoundException("El empleado no existe.");

            var credentials = await credentialRepository.GetByEmployeeAsync(query.EmployeeId, cancellationToken);

            return Result<IReadOnlyCollection<CredentialDto>>.Success(
                credentials.Select(credential => credential.ToDto()).ToList());
        });
}
