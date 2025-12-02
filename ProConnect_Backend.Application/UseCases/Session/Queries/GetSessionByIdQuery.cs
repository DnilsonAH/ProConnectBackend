using AutoMapper;
using MediatR;
using ProConnect_Backend.Application.DTOsResponse.SessionDTOs;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.Session.Queries;

public class GetSessionByIdQuery : IRequest<SessionResponseDto>
{
    public uint Id { get; set; }
}

public class GetSessionByIdQueryHandler : IRequestHandler<GetSessionByIdQuery, SessionResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetSessionByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SessionResponseDto> Handle(GetSessionByIdQuery request, CancellationToken cancellationToken)
    {
        var session = await _unitOfWork.SessionRepository.GetSessionWithDetailsAsync(request.Id);
        var response = _mapper.Map<SessionResponseDto>(session);
        return response;
    }
}
