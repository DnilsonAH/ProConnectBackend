using System.ComponentModel.DataAnnotations;

namespace ProConnect_Backend.Domain.DTOsRequest.ProfessionalProfileDTOs;

public class CreateProfessionalProfileDto
{
    [Required]
    [MaxLength(1000)]
    public string Experience { get; set; } = null!;

    [Required]
    [MaxLength(1000)]
    public string Presentation { get; set; } = null!;

    public List<uint> SpecializationIds { get; set; } = new();
}
