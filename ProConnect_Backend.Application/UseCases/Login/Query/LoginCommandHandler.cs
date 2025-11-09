using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProConnect_Backend.Application.DTOsResponse.LoginDTOs;
using ProConnect_Backend.Application.UseCases.Login.Command;
using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports.IRepositories;

namespace ProConnect_Backend.Application.UseCases.Login.Query;

public class LoginCommandHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _config;
    private readonly PasswordHasher<User> _passwordHasher;

    public LoginCommandHandler(IUserRepository userRepository, IConfiguration config)
    {
        _userRepository = userRepository;
        _config = config;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<LoginResponseDto?> Handle(LoginCommand command)
    {
        var dto = command.Dto;

        // Buscar usuario por email
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null) return null;

        // Validar contraseña
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        if (result == PasswordVerificationResult.Failed)
            return null;

        // Generar token JWT
        var token = GenerateJwtToken(user);

        return new LoginResponseDto
        {
            Id = user.UserId,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            Token = token
        };
    }

    private string GenerateJwtToken(User user)
    {
        var secretKey = _config["JwtSettings:SecretKey"] ?? throw new Exception("Falta JwtSettings:SecretKey en configuración");
        var issuer = _config["JwtSettings:Issuer"] ?? "ProConnectAPI";
        var audience = _config["JwtSettings:Audience"] ?? "ProConnectClient";
        var expirationHours = int.Parse(_config["JwtSettings:ExpirationHours"] ?? "24");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("name", user.Name)
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(expirationHours),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}