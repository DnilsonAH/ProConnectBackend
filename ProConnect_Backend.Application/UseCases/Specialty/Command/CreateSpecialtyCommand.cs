using AutoMapper;
using MediatR;
using ProConnect_Backend.Application.DTOsResponse.SpecialityDtos;
using ProConnect_Backend.Domain.Ports;
using ProConnect_Backend.Infrastructure.Adapters;

namespace ProConnect_Backend.Application.UseCases.Specialty.Command;

public class CreateSpecialtyCommand : IRequest<SpecialtyDto>
{
    public SpecialtyDto SpecialtyDto { get; set; } = null!;
}

public class CreateSpecialtyCommandHandler : IRequestHandler<CreateSpecialtyCommand, SpecialtyDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateSpecialtyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<SpecialtyDto> Handle(CreateSpecialtyCommand request, CancellationToken cancellationToken)
    {
        var specialty = _mapper.Map<Domain.Entities.Specialty>(request.SpecialtyDto);
        await _unitOfWork.SpecialtyRepository.AddAsync(specialty);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<SpecialtyDto>(specialty);
    }
}
