using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Application.DTOsResponse.ProfessionDTOs;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.Profession.Queries.GetProfessionById;

public class GetProfessionByIdHandler : IRequestHandler<GetProfessionByIdQuery, ProfessionResponseDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProfessionByIdHandler> _logger;

    public GetProfessionByIdHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetProfessionByIdHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProfessionResponseDto?> Handle(GetProfessionByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("🔍 Buscando profesión: {ProfessionId}", request.ProfessionId);

        var profession = await _unitOfWork.ProfessionRepository.GetProfessionWithCategoryAsync(request.ProfessionId);
        
        if (profession == null)
        {
            _logger.LogWarning("❌ Profesión no encontrada: {ProfessionId}", request.ProfessionId);
            return null;
        }

        var response = _mapper.Map<ProfessionResponseDto>(profession);
        response.CategoryName = profession.Category?.CategoryName ?? "Sin categoría";
        response.TotalSpecializations = profession.Specializations?.Count ?? 0;

        _logger.LogInformation("✅ Profesión encontrada: {ProfessionId} - {ProfessionName}", 
            profession.ProfessionId, profession.ProfessionName);
        
        return response;
    }
}
