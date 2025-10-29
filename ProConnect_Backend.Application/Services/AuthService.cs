using ProConnect_Backend.Application.DTOs.AuthDtos;
using ProConnect_Backend.Application.Ports;
using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public AuthService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IJwtService jwtService)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerDto)
    {
        // Verificar si el email ya existe
        if (await _unitOfWork.UserRepository.ExistsByEmailAsync(registerDto.Email))
        {
            throw new InvalidOperationException("El email ya está registrado");
        }

        // Crear el nuevo usuario
        var user = new User
        {
            Name = registerDto.Name,
            Email = registerDto.Email,
            PasswordHash = _passwordHasher.HashPassword(registerDto.Password),
            PhoneNumber = registerDto.PhoneNumber,
            RegistrationDate = DateTime.UtcNow
        };

        await _unitOfWork.UserRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        // Asignar rol "User" por defecto
        var userRole = await _unitOfWork.RoleRepository.GetByNameAsync("User");
        if (userRole == null)
        {
            throw new InvalidOperationException("El rol 'User' no existe en la base de datos");
        }

        var assignUserRole = new UserRole
        {
            UserId = user.UserId,
            RoleId = userRole.RoleId,
            AssignedDate = DateTime.UtcNow
        };

        await _unitOfWork.UserRoleRepository.AddAsync(assignUserRole);
        await _unitOfWork.SaveChangesAsync();

        // Generar token
        var roles = new List<string> { "User" };
        var token = _jwtService.GenerateToken(user, roles);

        return new AuthResponseDto
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            Roles = roles,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddHours(24)
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto loginDto)
    {
        // Buscar el usuario por email
        var user = await _unitOfWork.UserRepository.GetByEmailAsync(loginDto.Email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        // Verificar la contraseña
        if (!_passwordHasher.VerifyPassword(loginDto.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        // Obtener los roles del usuario
        var userRoles = await _unitOfWork.UserRoleRepository.GetUserRolesByUserIdAsync(user.UserId);
        var roles = new List<string>();

        foreach (var userRole in userRoles)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(Guid.Parse(userRole.RoleId.ToString()));
            if (role != null)
            {
                roles.Add(role.RoleName);
            }
        }

        // Generar token
        var token = _jwtService.GenerateToken(user, roles);

        return new AuthResponseDto
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            Roles = roles,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddHours(24)
        };
    }

    public async Task<bool> ChangeUserRoleAsync(uint userId, string roleName)
    {
        // Verificar que el usuario existe
        var user = await _unitOfWork.UserRepository.GetByIdAsync(Guid.Parse(userId.ToString()));
        if (user == null)
        {
            throw new InvalidOperationException("Usuario no encontrado");
        }

        // Verificar que el rol existe
        var role = await _unitOfWork.RoleRepository.GetByNameAsync(roleName);
        if (role == null)
        {
            throw new InvalidOperationException($"El rol '{roleName}' no existe");
        }

        // Obtener roles actuales del usuario
        var currentUserRoles = await _unitOfWork.UserRoleRepository.GetUserRolesByUserIdAsync(userId);

        // Verificar si el usuario ya tiene el rol
        var alreadyHasRole = false;
        foreach (var userRole in currentUserRoles)
        {
            var currentRole = await _unitOfWork.RoleRepository.GetByIdAsync(Guid.Parse(userRole.RoleId.ToString()));
            if (currentRole?.RoleName == roleName)
            {
                alreadyHasRole = true;
                break;
            }
        }

        if (alreadyHasRole)
        {
            return true; // Ya tiene el rol, no hacer nada
        }

        // Asignar el nuevo rol
        var newUserRole = new UserRole
        {
            UserId = userId,
            RoleId = role.RoleId,
            AssignedDate = DateTime.UtcNow
        };

        await _unitOfWork.UserRoleRepository.AddAsync(newUserRole);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
