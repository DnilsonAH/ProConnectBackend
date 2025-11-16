using System.ComponentModel.DataAnnotations;

namespace ProConnect_Backend.Domain.DTOsRequest.ProfessionCategoryDTOs;

public class UpdateProfessionCategoryRequestDto
{
    [Required(ErrorMessage = "El nombre de la categoría es obligatorio")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
    public string CategoryName { get; set; } = null!;

    [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
    public string? Description { get; set; }
}
