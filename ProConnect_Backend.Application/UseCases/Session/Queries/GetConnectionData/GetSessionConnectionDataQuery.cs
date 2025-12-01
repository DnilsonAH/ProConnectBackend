using MediatR;
using ProConnect_Backend.Application.DTOsResponse.SessionDTOs;

namespace ProConnect_Backend.Application.UseCases.Session.Queries.GetConnectionData;

public record GetSessionConnectionDataQuery(uint SessionId, uint UserId) : IRequest<ConnectionDataResponseDto>;