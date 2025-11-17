namespace ProConnect_Backend.Application.DTOsResponse.AuthDTOs;

public class ChangePasswordResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime ChangedAt { get; set; }
}
