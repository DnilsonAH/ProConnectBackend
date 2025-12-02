using AutoMapper;
using MediatR;
using ProConnect_Backend.Application.DTOsResponse.SessionDTOs;
using ProConnect_Backend.Domain.DTOsRequest.SessionDTOs;
using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.Session.Commands;

public class CreateSessionCommand : IRequest<SessionResponseDto>
{
    public CreateSessionDto Model { get; set; }
}

public class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand, SessionResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateSessionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SessionResponseDto> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        var client = await _unitOfWork.UserRepository.GetByIdAsync(request.Model.ClientId);
        if (client == null)
        {
            throw new Exception($"Client with ID {request.Model.ClientId} not found.");
        }

        var professional = await _unitOfWork.UserRepository.GetByIdAsync(request.Model.ProfessionalId);
        if (professional == null)
        {
            throw new Exception($"Professional with ID {request.Model.ProfessionalId} not found.");
        }
        
        var session = _mapper.Map<Domain.Entities.Session>(request.Model);
        
        await _unitOfWork.SessionRepository.AddAsync(session);
        await _unitOfWork.SaveChangesAsync();

        var createdSession = await _unitOfWork.SessionRepository.GetSessionWithDetailsAsync(session.SessionId);

        var response = _mapper.Map<SessionResponseDto>(createdSession);
        return response;
    }
}
