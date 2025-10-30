using ProConnect_Backend.Domain.DTOsRequest.AuthDtos;

namespace ProConnect_Backend.Domain.Ports.IServices;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerDto);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto loginDto);
    Task<bool> ChangeUserRoleAsync(uint userId, string roleName);
}
