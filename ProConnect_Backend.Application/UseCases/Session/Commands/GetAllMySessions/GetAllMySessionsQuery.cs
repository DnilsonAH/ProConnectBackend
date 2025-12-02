using MediatR;
using ProConnect_Backend.Application.DTOsResponse.SessionDTOs;

namespace ProConnect_Backend.Application.UseCases.Session.Commands.GetAllMySessions;

public record GetAllMySessionsQuery(uint UserId) : IRequest<IEnumerable<SessionResponseDTOs>>;
