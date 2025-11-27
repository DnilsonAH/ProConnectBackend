namespace ProConnect_Backend.Application.DTOsResponse.ProfessionalProfileDTOs;

// Respuesta paginada
public class PagedProfessionalResponseDto
{
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public IEnumerable<ProfessionalSearchResultDto> Professionals { get; set; } = new List<ProfessionalSearchResultDto>();
}