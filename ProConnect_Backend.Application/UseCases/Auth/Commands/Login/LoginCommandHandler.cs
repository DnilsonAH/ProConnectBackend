using MediatR;
using ProConnect_Backend.Application.DTOsResponse.LoginDTOs;
using ProConnect_Backend.Domain.Ports;
using ProConnect_Backend.Domain.Ports.IServices;

namespace ProConnect_Backend.Application.UseCases.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(
        IUnitOfWork unitOfWork, 
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponseDto?> Handle(LoginCommand request, CancellationToken cancellationToken = default)
    {
        // Buscar usuario por email
        var user = await _unitOfWork.UserRepository.GetByEmailAsync(request.LoginDto.Email);
        
        if (user == null)
        {
            return null; // Usuario no encontrado
        }

        // Verificar contraseña
        var isValidPassword = _passwordHasher.VerifyPassword(request.LoginDto.Password, user.PasswordHash);
        
        if (!isValidPassword)
        {
            return null; // Contraseña incorrecta
        }

        // Generar JWT Token usando servicio de Infrastructure
        var token = _jwtTokenService.GenerateToken(user.UserId, user.Email, user.Role);

        return new LoginResponseDto
        {
            Id = user.UserId,
            Name = user.FirstName, // Solo FirstName por ahora
            Email = user.Email,
            Role = user.Role,
            Token = token
        };
    }
}
