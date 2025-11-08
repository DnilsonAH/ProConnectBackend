using ProConnect_Backend.Application.DTOsResponse.LoginDTOs;

namespace ProConnect_Backend.Application.UseCases.Login.Command;

public class LoginCommand
{
    public LoginRequestDto Dto { get; }

    public LoginCommand(LoginRequestDto dto)
    {
        Dto = dto;
    }
}