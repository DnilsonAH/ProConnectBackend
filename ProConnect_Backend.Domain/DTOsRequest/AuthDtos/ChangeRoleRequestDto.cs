using System.ComponentModel.DataAnnotations;

namespace ProConnect_Backend.Domain.DTOsRequest.AuthDtos;

public class ChangeRoleRequestDto
{
    [Required(ErrorMessage = "El ID del usuario es requerido")]
    public uint UserId { get; set; }

    [Required(ErrorMessage = "El nombre del rol es requerido")]
    [RegularExpression("^(Admin|Professional|User)$", ErrorMessage = "El rol debe ser Admin, Professional o User")]
    public string RoleName { get; set; } = null!;
}
