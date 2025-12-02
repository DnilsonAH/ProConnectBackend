using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using ProConnect_Backend.Application.DTOsResponse.SessionDTOs;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.Session.Queries;

public class GetMyPreviousSessionsQuery : IRequest<IEnumerable<SessionResponseDto>>
{
}

public class GetMyPreviousSessionsQueryHandler : IRequestHandler<GetMyPreviousSessionsQuery, IEnumerable<SessionResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetMyPreviousSessionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<SessionResponseDto>> Handle(GetMyPreviousSessionsQuery request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        
        var sessions = await _unitOfWork.SessionRepository.GetPastSessionsAsync(uint.Parse(userId));
        
        var response = _mapper.Map<IEnumerable<SessionResponseDto>>(sessions);
        return response;
    }
}
