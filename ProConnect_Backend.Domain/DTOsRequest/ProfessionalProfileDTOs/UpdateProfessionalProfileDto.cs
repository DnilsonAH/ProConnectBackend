using System.ComponentModel.DataAnnotations;

namespace ProConnect_Backend.Domain.DTOsRequest.ProfessionalProfileDTOs;

public class UpdateProfessionalProfileDto
{
    [MaxLength(1000)]
    public string? Experience { get; set; }

    [MaxLength(1000)]
    public string? Presentation { get; set; }

    public List<uint>? SpecializationIds { get; set; }
}
