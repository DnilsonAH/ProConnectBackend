using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.DTOsRequest.AuthDtos;
using ProConnect_Backend.Infrastructure.Data;

namespace ProConnect_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
	private readonly ProConnectDbContext _db;
	private readonly ILogger<UserController> _logger;
	private readonly PasswordHasher<User> _passwordHasher;

	public UserController(ProConnectDbContext db, ILogger<UserController> logger)
	{
		_db = db;
		_logger = logger;
		_passwordHasher = new PasswordHasher<User>();
	}

	/// <summary>
	/// Registra un nuevo usuario.
	/// - Valida el DTO
	/// - Verifica que el email no exista
	/// - Hashea la contraseña y persiste la entidad User
	/// </summary>
	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		// Verificación de email único
		var exists = await _db.Users.AnyAsync(u => u.Email == dto.Email);
		if (exists)
			return Conflict(new { message = "El correo ya está en uso." });

		// Mapear y preparar entidad
		var user = new User
		{
			Name = dto.Name,
			Email = dto.Email,
			PhoneNumber = dto.PhoneNumber,
			// Asumimos rol por defecto 'Client' para nuevos registros
			Role = "Client"
		};

		user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

		_db.Users.Add(user);
		await _db.SaveChangesAsync();

		_logger.LogInformation("Usuario registrado: {Email} (id: {Id})", user.Email, user.UserId);

		var result = new { Id = user.UserId, user.Email, user.Name, user.PhoneNumber, user.Role };

		return CreatedAtAction(nameof(GetById), new { id = user.UserId }, result);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(uint id)
	{
		var user = await _db.Users
			.AsNoTracking()
			.Where(u => u.UserId == id)
			.Select(u => new { u.UserId, u.Email, u.Name, u.PhoneNumber, u.Role, u.PhotoUrl, u.RegistrationDate })
			.SingleOrDefaultAsync();

		if (user == null) return NotFound();

		return Ok(user);
	}
}