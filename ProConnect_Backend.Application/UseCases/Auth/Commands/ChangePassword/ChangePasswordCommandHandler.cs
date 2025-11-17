using MediatR;
using ProConnect_Backend.Application.DTOsResponse.AuthDTOs;
using ProConnect_Backend.Domain.Ports;
using ProConnect_Backend.Domain.Ports.IServices;

namespace ProConnect_Backend.Application.UseCases.Auth.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ChangePasswordResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public ChangePasswordCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<ChangePasswordResponseDto> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        // Obtener el usuario
        var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);

        if (user == null)
        {
            return new ChangePasswordResponseDto
            {
                Success = false,
                Message = "Usuario no encontrado"
            };
        }

        // Verificar que la contraseña actual sea correcta
        var isCurrentPasswordValid = _passwordHasher.VerifyPassword(
            request.RequestDto.CurrentPassword, 
            user.PasswordHash);

        if (!isCurrentPasswordValid)
        {
            return new ChangePasswordResponseDto
            {
                Success = false,
                Message = "La contraseña actual es incorrecta"
            };
        }

        // Verificar que la nueva contraseña sea diferente a la actual
        var isSamePassword = _passwordHasher.VerifyPassword(
            request.RequestDto.NewPassword, 
            user.PasswordHash);

        if (isSamePassword)
        {
            return new ChangePasswordResponseDto
            {
                Success = false,
                Message = "La nueva contraseña debe ser diferente a la actual"
            };
        }

        // Actualizar la contraseña
        user.PasswordHash = _passwordHasher.HashPassword(request.RequestDto.NewPassword);
        _unitOfWork.UserRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Opcional: Invalidar todos los tokens JWT anteriores del usuario
        // (Revocar todas las sesiones activas para forzar nuevo login)
        var userTokens = await _unitOfWork.JwtBlacklistRepository.GetTokensByUserIdAsync(user.UserId);
        // Esta funcionalidad depende de si quieres forzar logout en todos los dispositivos

        return new ChangePasswordResponseDto
        {
            Success = true,
            Message = "Contraseña actualizada exitosamente",
            ChangedAt = DateTime.UtcNow
        };
    }
}
