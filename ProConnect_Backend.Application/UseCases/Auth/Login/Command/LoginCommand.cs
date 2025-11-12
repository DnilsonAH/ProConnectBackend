// TODO: Descomentar después del scaffolding cuando los DTOs estén creados
// using ProConnect_Backend.Application.DTOsResponse.LoginDTOs;

namespace ProConnect_Backend.Application.UseCases.Login.Command;

public class LoginCommand
{
    public object? Dto { get; } // Temporal hasta que LoginRequestDto esté disponible

    public LoginCommand(object dto)
    {
        Dto = dto;
    }
}