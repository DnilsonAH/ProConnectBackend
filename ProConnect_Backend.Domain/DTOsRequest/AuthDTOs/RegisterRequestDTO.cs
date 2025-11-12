using System.ComponentModel.DataAnnotations;

namespace ProConnect_Backend.Domain.DTOsRequest.AuthDtos;

public class RegisterRequestDto
{
    [Required(ErrorMessage = "El primer nombre es requerido")]
    [MinLength(2, ErrorMessage = "El primer nombre debe tener al menos 2 caracteres")]
    [MaxLength(100, ErrorMessage = "El primer nombre no puede exceder 100 caracteres")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El segundo nombre es requerido")]
    [MinLength(2, ErrorMessage = "El segundo nombre debe tener al menos 2 caracteres")]
    [MaxLength(100, ErrorMessage = "El segundo nombre no puede exceder 100 caracteres")]
    public string SecondName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El primer apellido es requerido")]
    [MinLength(2, ErrorMessage = "El primer apellido debe tener al menos 2 caracteres")]
    [MaxLength(100, ErrorMessage = "El primer apellido no puede exceder 100 caracteres")]
    public string FirstSurname { get; set; } = string.Empty;

    [Required(ErrorMessage = "El segundo apellido es requerido")]
    [MinLength(2, ErrorMessage = "El segundo apellido debe tener al menos 2 caracteres")]
    [MaxLength(100, ErrorMessage = "El segundo apellido no puede exceder 100 caracteres")]
    public string SecondSurname { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo electrónico es requerido")]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
    [MaxLength(150, ErrorMessage = "El correo no puede exceder 150 caracteres")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
    public string Password { get; set; } = string.Empty;

    [Phone(ErrorMessage = "El formato del teléfono no es válido")]
    [MaxLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
    public string? PhoneNumber { get; set; }

    [MaxLength(255, ErrorMessage = "La URL de la foto no puede exceder 255 caracteres")]
    public string? PhotoUrl { get; set; }
}
