using AutoMapper;
using MediatR;
using ProConnect_Backend.Application.DTOsResponse.SessionDTOs;
using ProConnect_Backend.Domain.DTOsRequest.SessionDTOs;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.Session.Commands;

public class UpdateSessionCommand : IRequest<SessionResponseDto>
{
    public uint Id { get; set; }
    public UpdateSessionDto Model { get; set; }
}

public class UpdateSessionCommandHandler : IRequestHandler<UpdateSessionCommand, SessionResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateSessionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SessionResponseDto> Handle(UpdateSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _unitOfWork.SessionRepository.GetByIdAsync(request.Id);
        if (session == null)
        {
            return null;
        }

        _mapper.Map(request.Model, session);

        _unitOfWork.SessionRepository.Update(session);
        await _unitOfWork.SaveChangesAsync();

        var response = _mapper.Map<SessionResponseDto>(session);
        return response;
    }
}
