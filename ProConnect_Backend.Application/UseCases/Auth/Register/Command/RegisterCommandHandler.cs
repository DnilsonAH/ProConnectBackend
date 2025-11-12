using MediatR;
using ProConnect_Backend.Application.DTOsResponse.AuthDTOs;
using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports;
using ProConnect_Backend.Domain.Ports.IServices;

namespace ProConnect_Backend.Application.UseCases.Users.Command;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public RegisterCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<RegisterResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var dto = request.RegisterDto;

        // Verificar si el email ya existe
        var existingUser = await _unitOfWork.UserRepository.GetByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("El correo electrónico ya está registrado");
        }

        // Crear nueva entidad User
        var user = new User
        {
            FirstName = dto.FirstName,
            FirstSurname = dto.LastName,
            Email = dto.Email,
            PasswordHash = _passwordHasher.HashPassword(dto.Password),
            Role = "User", // Rol por defecto
            PhoneNumber = dto.Phone,
            RegistrationDate = DateTime.UtcNow
        };

        // Guardar en la base de datos
        await _unitOfWork.UserRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Generar token JWT
        var token = _jwtTokenService.GenerateToken(user.UserId, user.Email, user.Role);

        return new RegisterResponseDto
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.FirstSurname,
            Email = user.Email,
            Role = user.Role,
            RegistrationDate = user.RegistrationDate,
            Token = token
        };
    }
}
