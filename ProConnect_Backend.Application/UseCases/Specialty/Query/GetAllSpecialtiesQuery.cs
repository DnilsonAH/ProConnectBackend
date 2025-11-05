using AutoMapper;
using MediatR;
using ProConnect_Backend.Application.DTOsResponse.SpecialityDtos;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.Specialty.Query;

public class GetAllSpecialtiesQuery : IRequest<IEnumerable<SpecialtyGetDto>> {}

public class GetAllSpecialtiesQueryHandler : IRequestHandler<GetAllSpecialtiesQuery, IEnumerable<SpecialtyGetDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllSpecialtiesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<IEnumerable<SpecialtyGetDto>> Handle(GetAllSpecialtiesQuery request, CancellationToken cancellationToken)
    {
        var specialties = await _unitOfWork.SpecialtyRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<SpecialtyGetDto>>(specialties);
    }
}
