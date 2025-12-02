using AutoMapper;
using MediatR;
using ProConnect_Backend.Application.DTOsResponse.SessionDTOs;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.Session.Queries;

public class GetAllSessionsQuery : IRequest<IEnumerable<SessionResponseDto>>
{
}

public class GetAllSessionsQueryHandler : IRequestHandler<GetAllSessionsQuery, IEnumerable<SessionResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllSessionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SessionResponseDto>> Handle(GetAllSessionsQuery request, CancellationToken cancellationToken)
    {
        var sessions = await _unitOfWork.SessionRepository.GetAllWithDetailsAsync();
        var response = _mapper.Map<IEnumerable<SessionResponseDto>>(sessions);
        return response;
    }
}
