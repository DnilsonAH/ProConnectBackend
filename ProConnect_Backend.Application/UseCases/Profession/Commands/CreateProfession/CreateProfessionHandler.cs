using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Application.DTOsResponse.ProfessionDTOs;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.Profession.Commands.CreateProfession;

public class CreateProfessionHandler : IRequestHandler<CreateProfessionCommand, ProfessionResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateProfessionHandler> _logger;

    public CreateProfessionHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CreateProfessionHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProfessionResponseDto> Handle(CreateProfessionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("📝 Creando nueva profesión: {ProfessionName} en categoría {CategoryId}", 
            request.Dto.ProfessionName, request.Dto.CategoryId);

        // Validar que la categoría exista
        var category = await _unitOfWork.ProfessionCategoryRepository.GetByIdAsync(request.Dto.CategoryId);
        if (category == null)
        {
            _logger.LogWarning("❌ Categoría no encontrada: {CategoryId}", request.Dto.CategoryId);
            throw new KeyNotFoundException($"No se encontró la categoría con ID {request.Dto.CategoryId}");
        }

        // Validar que no exista una profesión con el mismo nombre
        var existingProfession = await _unitOfWork.ProfessionRepository.GetByNameAsync(request.Dto.ProfessionName);
        if (existingProfession != null)
        {
            _logger.LogWarning("⚠️ Ya existe una profesión con el nombre: {ProfessionName}", request.Dto.ProfessionName);
            throw new InvalidOperationException($"Ya existe una profesión con el nombre '{request.Dto.ProfessionName}'");
        }

        // Crear la entidad
        var profession = _mapper.Map<Domain.Entities.Profession>(request.Dto);
        
        await _unitOfWork.ProfessionRepository.AddAsync(profession);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("✅ Profesión creada exitosamente: {ProfessionId} - {ProfessionName}", 
            profession.ProfessionId, profession.ProfessionName);

        var response = _mapper.Map<ProfessionResponseDto>(profession);
        response.CategoryName = category.CategoryName;
        response.TotalSpecializations = 0; // Nueva profesión no tiene especializaciones
        
        return response;
    }
}
