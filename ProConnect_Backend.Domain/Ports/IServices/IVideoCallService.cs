namespace ProConnect_Backend.Domain.Ports.IServices;

public interface IVideoCallService
{
    /// <summary>
    /// Genera un token de acceso para la videollamada.
    /// </summary>
    /// <param name="userId">ID del usuario que se conectará</param>
    /// <param name="roomId">ID de la sesión o sala (SessionId)</param>
    /// <returns>Token cifrado para el SDK de frontend</returns>
    string GenerateCallToken(string userId, string roomId);
}