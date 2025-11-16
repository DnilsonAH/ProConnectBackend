using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Application.DTOsResponse.SpecializationDTOs;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.Specialization.Queries.GetSpecializationById;

public class GetSpecializationByIdHandler : IRequestHandler<GetSpecializationByIdQuery, SpecializationResponseDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetSpecializationByIdHandler> _logger;

    public GetSpecializationByIdHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetSpecializationByIdHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<SpecializationResponseDto?> Handle(GetSpecializationByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("🔍 Buscando especialización: {SpecializationId}", request.SpecializationId);

        var specialization = await _unitOfWork.SpecializationRepository.GetByIdAsync(request.SpecializationId);
        
        if (specialization == null)
        {
            _logger.LogWarning("❌ Especialización no encontrada: {SpecializationId}", request.SpecializationId);
            return null;
        }

        var response = _mapper.Map<SpecializationResponseDto>(specialization);
        response.ProfessionName = specialization.Profession?.ProfessionName ?? "Sin profesión";
        response.TotalProfiles = specialization.ProfileSpecializations?.Count ?? 0;

        _logger.LogInformation("✅ Especialización encontrada: {SpecializationId} - {SpecializationName}", 
            specialization.SpecializationId, specialization.SpecializationName);
        
        return response;
    }
}
