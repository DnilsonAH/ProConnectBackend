using System.ComponentModel.DataAnnotations;

namespace ProConnect_Backend.Domain.DTOsRequest.AuthDtos;

public class RegisterRequestDto
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "La contraseña es requerida")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres")]
    public string Password { get; set; } = null!;

    [Phone(ErrorMessage = "El formato del teléfono no es válido")]
    public string? PhoneNumber { get; set; }
}
