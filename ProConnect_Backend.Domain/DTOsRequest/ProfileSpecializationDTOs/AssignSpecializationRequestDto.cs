using System.ComponentModel.DataAnnotations;

namespace ProConnect_Backend.Domain.DTOsRequest.ProfileSpecializationDTOs;

public class AssignSpecializationRequestDto
{
    [Required(ErrorMessage = "El ID del perfil profesional es obligatorio")]
    public uint ProfileId { get; set; }

    [Required(ErrorMessage = "El ID de la especialización es obligatorio")]
    public uint SpecializationId { get; set; }
}
