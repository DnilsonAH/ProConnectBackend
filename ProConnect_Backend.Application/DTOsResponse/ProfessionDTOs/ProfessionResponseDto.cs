namespace ProConnect_Backend.Application.DTOsResponse.ProfessionDTOs;

public class ProfessionResponseDto
{
    public uint ProfessionId { get; set; }
    public uint CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public string ProfessionName { get; set; } = null!;
    public string? Description { get; set; }
    public int TotalSpecializations { get; set; }
}
