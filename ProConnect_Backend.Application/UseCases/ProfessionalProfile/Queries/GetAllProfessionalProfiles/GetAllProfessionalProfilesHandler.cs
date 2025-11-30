using AutoMapper;
using MediatR;
using ProConnect_Backend.Application.DTOsResponse.ProfessionalProfileDTOs;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.ProfessionalProfile.Queries.GetAllProfessionalProfiles;

public class GetAllProfessionalProfilesHandler : IRequestHandler<GetAllProfessionalProfilesQuery, IEnumerable<ProfessionalProfileResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllProfessionalProfilesHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProfessionalProfileResponseDto>> Handle(GetAllProfessionalProfilesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.ProfessionalProfileRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ProfessionalProfileResponseDto>>(entities);
    }
}
