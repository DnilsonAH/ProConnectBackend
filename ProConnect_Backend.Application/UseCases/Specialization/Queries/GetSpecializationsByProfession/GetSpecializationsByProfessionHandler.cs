using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Application.DTOsResponse.SpecializationDTOs;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.Specialization.Queries.GetSpecializationsByProfession;

public class GetSpecializationsByProfessionHandler : IRequestHandler<GetSpecializationsByProfessionQuery, IEnumerable<SpecializationResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetSpecializationsByProfessionHandler> _logger;

    public GetSpecializationsByProfessionHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetSpecializationsByProfessionHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<SpecializationResponseDto>> Handle(GetSpecializationsByProfessionQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("📋 Obteniendo especializaciones de la profesión: {ProfessionId}", request.ProfessionId);

        // Validar que la profesión exista
        var profession = await _unitOfWork.ProfessionRepository.GetByIdAsync(request.ProfessionId);
        if (profession == null)
        {
            _logger.LogWarning("❌ Profesión no encontrada: {ProfessionId}", request.ProfessionId);
            throw new KeyNotFoundException($"No se encontró la profesión con ID {request.ProfessionId}");
        }

        var allSpecializations = await _unitOfWork.SpecializationRepository.GetAllAsync();
        var specializations = allSpecializations.Where(s => s.ProfessionId == request.ProfessionId).ToList();
        
        var response = specializations.Select(s =>
        {
            var dto = _mapper.Map<SpecializationResponseDto>(s);
            dto.ProfessionName = profession.ProfessionName;
            dto.TotalProfiles = s.ProfileSpecializations?.Count ?? 0;
            return dto;
        }).ToList();

        _logger.LogInformation("✅ Se encontraron {Count} especializaciones en la profesión {ProfessionName}", 
            response.Count, profession.ProfessionName);
        
        return response;
    }
}
