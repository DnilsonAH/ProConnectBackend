using AutoMapper;
using MediatR;
using ProConnect_Backend.Application.DTOsResponse.ProfessionalProfileDTOs;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.ProfessionalProfile.Queries.GetProfessionalProfileById;

public class GetProfessionalProfileByIdHandler : IRequestHandler<GetProfessionalProfileByIdQuery, ProfessionalProfileResponseDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProfessionalProfileByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProfessionalProfileResponseDto?> Handle(GetProfessionalProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.ProfessionalProfileRepository.GetProfileWithDetailsAsync(request.ProfileId);
        return _mapper.Map<ProfessionalProfileResponseDto>(entity);
    }
}
