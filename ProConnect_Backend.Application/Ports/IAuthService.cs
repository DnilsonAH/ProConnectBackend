using ProConnect_Backend.Application.DTOs.AuthDtos;

namespace ProConnect_Backend.Application.Ports;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerDto);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto loginDto);
    Task<bool> ChangeUserRoleAsync(uint userId, string roleName);
}
