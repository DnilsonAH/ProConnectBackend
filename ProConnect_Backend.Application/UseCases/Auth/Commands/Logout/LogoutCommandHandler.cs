using MediatR;
using ProConnect_Backend.Domain.Ports;
using ProConnect_Backend.Domain.Ports.IServices;

namespace ProConnect_Backend.Application.UseCases.Auth.Commands.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;

    public LogoutCommandHandler(IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Parsear token usando servicio de Infrastructure
            var tokenData = _jwtTokenService.ParseToken(request.Token);
            
            if (tokenData == null)
            {
                return false; // Token inválido
            }

            var (jti, userId, expiresAt) = tokenData.Value;

            // Verificar si el token ya está revocado
            var isAlreadyRevoked = await _unitOfWork.JwtBlacklistRepository.IsTokenRevokedAsync(jti);
            if (isAlreadyRevoked)
            {
                return true; // Ya estaba revocado
            }

            // Agregar token a la blacklist
            await _unitOfWork.JwtBlacklistRepository.AddTokenAsync(jti, userId, expiresAt);

            // Limpiar tokens expirados (opcional)
            await _unitOfWork.JwtBlacklistRepository.RemoveExpiredTokensAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }
}

