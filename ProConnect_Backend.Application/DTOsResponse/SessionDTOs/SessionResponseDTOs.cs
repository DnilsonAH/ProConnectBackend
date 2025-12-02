namespace ProConnect_Backend.Application.DTOsResponse.SessionDTOs;

public class SessionResponseDTOs
{
    public uint SessionId { get; set; }
    
    public String ProfessionalName { get; set; }
    
    public String ClientName { get; set; }
    
    public String NameSpecialty { get; set; } 
    
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
    
    public string Status { get; set; } = null!;

}