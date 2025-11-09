using ProConnect_Backend.Domain.DTOsRequest.AuthDtos;

namespace ProConnect_Backend.Application.UseCases.Users.Command;

public class RegisterCommand
{
    public RegisterRequestDto Dto { get; }

    public RegisterCommand(RegisterRequestDto dto)
    {
        Dto = dto;
    }
}
