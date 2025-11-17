namespace ProConnect_Backend.Application.DTOsResponse.ProfessionCategoryDTOs;

public class ProfessionCategoryResponseDto
{
    public uint CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public string? Description { get; set; }
    public int TotalProfessions { get; set; }
}
