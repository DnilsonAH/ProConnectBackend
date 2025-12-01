namespace ProConnect_Backend.Application.DTOsResponse.SessionDTOs;

public class ConnectionDataResponseDto
{
    public uint AppId { get; set; }
    public string AppSign { get; set; } = string.Empty; // Vacío por seguridad
    public string Token { get; set; } = string.Empty;
    public string RoomId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
}