using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Application.DTOsResponse.ProfessionDTOs;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.Profession.Commands.UpdateProfession;

public class UpdateProfessionHandler : IRequestHandler<UpdateProfessionCommand, ProfessionResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateProfessionHandler> _logger;

    public UpdateProfessionHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<UpdateProfessionHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProfessionResponseDto> Handle(UpdateProfessionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("📝 Actualizando profesión: {ProfessionId}", request.ProfessionId);

        // Buscar la profesión
        var profession = await _unitOfWork.ProfessionRepository.GetByIdAsync(request.ProfessionId);
        if (profession == null)
        {
            _logger.LogWarning("❌ Profesión no encontrada: {ProfessionId}", request.ProfessionId);
            throw new KeyNotFoundException($"No se encontró la profesión con ID {request.ProfessionId}");
        }

        // Validar que la categoría exista
        var category = await _unitOfWork.ProfessionCategoryRepository.GetByIdAsync(request.Dto.CategoryId);
        if (category == null)
        {
            _logger.LogWarning("❌ Categoría no encontrada: {CategoryId}", request.Dto.CategoryId);
            throw new KeyNotFoundException($"No se encontró la categoría con ID {request.Dto.CategoryId}");
        }

        // Validar que no exista otra profesión con el mismo nombre
        var existingProfession = await _unitOfWork.ProfessionRepository.GetByNameAsync(request.Dto.ProfessionName);
        if (existingProfession != null && existingProfession.ProfessionId != request.ProfessionId)
        {
            _logger.LogWarning("⚠️ Ya existe otra profesión con el nombre: {ProfessionName}", request.Dto.ProfessionName);
            throw new InvalidOperationException($"Ya existe otra profesión con el nombre '{request.Dto.ProfessionName}'");
        }

        // Actualizar campos
        profession.CategoryId = request.Dto.CategoryId;
        profession.ProfessionName = request.Dto.ProfessionName;
        profession.Description = request.Dto.Description;

        _unitOfWork.ProfessionRepository.Update(profession);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("✅ Profesión actualizada: {ProfessionId} - {ProfessionName}", 
            profession.ProfessionId, profession.ProfessionName);

        var response = _mapper.Map<ProfessionResponseDto>(profession);
        response.CategoryName = category.CategoryName;
        response.TotalSpecializations = profession.Specializations?.Count ?? 0;
        
        return response;
    }
}
