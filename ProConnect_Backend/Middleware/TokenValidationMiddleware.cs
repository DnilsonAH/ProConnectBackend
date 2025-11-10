using System.IdentityModel.Tokens.Jwt;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Middleware;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TokenValidationMiddleware> _logger;

    public TokenValidationMiddleware(RequestDelegate next, ILogger<TokenValidationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IUnitOfWork unitOfWork)
    {
        var authHeader = context.Request.Headers["Authorization"].ToString();

        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            var token = authHeader.Substring("Bearer ".Length).Trim();

            try
            {
                var handler = new JwtSecurityTokenHandler();
                
                if (handler.CanReadToken(token))
                {
                    var jwtToken = handler.ReadJwtToken(token);
                    var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

                    if (!string.IsNullOrEmpty(jti))
                    {
                        // Verificar si el token está en la blacklist
                        var isRevoked = await unitOfWork.RevokedTokenRepository.IsTokenRevokedAsync(jti);

                        if (isRevoked)
                        {
                            _logger.LogWarning("🚫 Token revocado detectado. JTI: {Jti}", jti);
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
                            {
                                success = false,
                                message = "🚫 Token revocado. Por favor, inicia sesión nuevamente."
                            }));
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al validar token en middleware");
                // Continuar con el siguiente middleware, la validación JWT estándar manejará el error
            }
        }

        await _next(context);
    }
}

