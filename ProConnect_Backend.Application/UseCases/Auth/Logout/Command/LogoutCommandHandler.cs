// TODO: Descomentar después del scaffolding cuando las entidades estén generadas
// NOTA: JWT NO DEBERÍA ESTAR EN APPLICATION - Mover a Infrastructure como servicio
// using System.IdentityModel.Tokens.Jwt;
// using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.Logout.Command;

public class LogoutCommandHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public LogoutCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(LogoutCommand command, CancellationToken cancellationToken = default)
    {
        // TODO: Refactorizar - La lógica de JWT debe estar en Infrastructure
        // Application solo debe recibir el userId/jti ya parseado desde un servicio
        return await Task.FromResult(true);
        
        // try
        // {
        //     var handler = new JwtSecurityTokenHandler();
        //     var jwtToken = handler.ReadJwtToken(command.Token);
        //
        //     var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        //     var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
        //
        //     if (string.IsNullOrEmpty(jti))
        //     {
        //         return false; // Token no tiene JTI
        //     }
        //
        //     if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        //     {
        //         return false; // Token no tiene user ID válido
        //     }
        //
        //     // Verificar si el token ya está revocado
        //     var isAlreadyRevoked = await _unitOfWork.RevokedTokenRepository.IsTokenRevokedAsync(jti, cancellationToken);
        //     if (isAlreadyRevoked)
        //     {
        //         return true; // Ya estaba revocado
        //     }
        //
        //     // Agregar token a la blacklist
        //     var revokedToken = new RevokedToken
        //     {
        //         TokenJti = jti,
        //         UserId = userId,
        //         RevokedAt = DateTime.UtcNow,
        //         ExpiresAt = jwtToken.ValidTo
        //     };
        //
        //     await _unitOfWork.RevokedTokenRepository.AddAsync(revokedToken, cancellationToken);
        //     await _unitOfWork.SaveChangesAsync(cancellationToken);
        //
        //     // Limpiar tokens expirados (opcional, puede ejecutarse en background)
        //     await _unitOfWork.RevokedTokenRepository.RemoveExpiredTokensAsync(cancellationToken);
        //     await _unitOfWork.SaveChangesAsync(cancellationToken);
        //
        //     return true;
        // }
        // catch
        // {
        //     return false;
        // }
    }
}

