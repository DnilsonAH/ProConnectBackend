using System.ComponentModel.DataAnnotations;

namespace ProConnect_Backend.Domain.DTOsRequest.SpecializationDTOs;

public class CreateSpecializationRequestDto
{
    [Required(ErrorMessage = "El ID de la profesión es obligatorio")]
    public uint ProfessionId { get; set; }

    [Required(ErrorMessage = "El nombre de la especialización es obligatorio")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
    public string SpecializationName { get; set; } = null!;

    [Required(ErrorMessage = "La descripción es obligatoria")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "La descripción debe tener entre 10 y 500 caracteres")]
    public string Description { get; set; } = null!;
}
