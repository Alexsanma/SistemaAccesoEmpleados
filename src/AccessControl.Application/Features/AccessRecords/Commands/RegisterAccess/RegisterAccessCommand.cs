using AccessControl.Application.Common.Results;
using AccessControl.Application.Features.AccessRecords.Dtos;
using AccessControl.Domain.AccessRecords;
using MediatR;

namespace AccessControl.Application.Features.AccessRecords.Commands.RegisterAccess;

public sealed record RegisterAccessCommand(
    string CredentialCode,
    Guid AccessPointId,
    MovementType MovementType) : IRequest<Result<AccessRecordDto>>;
