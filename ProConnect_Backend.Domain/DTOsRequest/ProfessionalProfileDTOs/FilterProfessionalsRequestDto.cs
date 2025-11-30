using System.ComponentModel.DataAnnotations;

namespace ProConnect_Backend.Domain.DTOsRequest.ProfessionalProfileDTOs;

public class FilterProfessionalsRequestDto
{
    // Filtros opcionales (Nullable uint)
    public uint? CategoryId { get; set; }
    public uint? ProfessionId { get; set; }
    public uint? SpecializationId { get; set; }

    // Paginación (Valores por defecto para evitar errores)
    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 10;
}