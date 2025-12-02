using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using ProConnect_Backend.Application.DTOsResponse.SessionDTOs;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.Session.Queries;

public class GetMyFutureSessionsQuery : IRequest<IEnumerable<SessionResponseDto>>
{
}

public class GetMyFutureSessionsQueryHandler : IRequestHandler<GetMyFutureSessionsQuery, IEnumerable<SessionResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetMyFutureSessionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<SessionResponseDto>> Handle(GetMyFutureSessionsQuery request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        
        var sessions = await _unitOfWork.SessionRepository.GetUpcomingSessionsAsync(uint.Parse(userId));
        
        var response = _mapper.Map<IEnumerable<SessionResponseDto>>(sessions);
        return response;
    }
}
