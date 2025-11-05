using AutoMapper;
using MediatR;
using ProConnect_Backend.Application.DTOsResponse.SpecialityDtos;
using ProConnect_Backend.Domain.Ports;
using ProConnect_Backend.Infrastructure.Adapters;

namespace ProConnect_Backend.Application.UseCases.Specialty.Command;

public class UpdateSpecialtyCommand : IRequest
{
    public Guid SpecialtyId { get; set; }
    public SpecialtyUpdateDto SpecialtyUpdateDto { get; set; } = null!;
}

public class UpdateSpecialtyCommandHandler : IRequestHandler<UpdateSpecialtyCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateSpecialtyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task Handle(UpdateSpecialtyCommand request, CancellationToken cancellationToken)
    {
        var specialty = await _unitOfWork.SpecialtyRepository.GetByIdAsync(request.SpecialtyId);
        if (specialty == null)
        {
            throw new Exception($"LA ESPECIALIDAD CON EL ID {request.SpecialtyId} NO FUE ENCONTRADA.");
        }
        _mapper.Map(request.SpecialtyUpdateDto, specialty);
        _unitOfWork.SpecialtyRepository.Update(specialty);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
