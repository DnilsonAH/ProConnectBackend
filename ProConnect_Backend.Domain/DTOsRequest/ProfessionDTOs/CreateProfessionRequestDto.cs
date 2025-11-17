using System.ComponentModel.DataAnnotations;

namespace ProConnect_Backend.Domain.DTOsRequest.ProfessionDTOs;

public class CreateProfessionRequestDto
{
    [Required(ErrorMessage = "El ID de la categoría es obligatorio")]
    public uint CategoryId { get; set; }

    [Required(ErrorMessage = "El nombre de la profesión es obligatorio")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
    public string ProfessionName { get; set; } = null!;

    [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
    public string? Description { get; set; }
}
