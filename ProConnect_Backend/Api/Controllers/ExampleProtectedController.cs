using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ProConnect_Backend.Api.Controllers;

/// <summary>
/// Ejemplo de controller con endpoints protegidos por JWT y roles
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ExampleProtectedController : ControllerBase
{
    /// <summary>
    /// Endpoint público - no requiere autenticación
    /// </summary>
    [HttpGet("public")]
    [AllowAnonymous]
    public IActionResult GetPublicData()
    {
        return Ok(new { message = "Este endpoint es público" });
    }

    /// <summary>
    /// Endpoint protegido - requiere cualquier usuario autenticado
    /// </summary>
    [HttpGet("protected")]
    [Authorize]
    public IActionResult GetProtectedData()
    {
        // Obtener información del usuario del token
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userName = User.FindFirst(ClaimTypes.Name)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        
        return Ok(new 
        { 
            message = "Este endpoint requiere autenticación",
            userId,
            userName,
            email
        });
    }

    /// <summary>
    /// Endpoint solo para Admin
    /// </summary>
    [HttpGet("admin-only")]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult GetAdminData()
    {
        return Ok(new { message = "Este endpoint es solo para Admin" });
    }

    /// <summary>
    /// Endpoint solo para Professional
    /// </summary>
    [HttpGet("professional-only")]
    [Authorize(Policy = "ProfessionalOnly")]
    public IActionResult GetProfessionalData()
    {
        return Ok(new { message = "Este endpoint es solo para Professional" });
    }

    /// <summary>
    /// Endpoint para Admin o Professional
    /// </summary>
    [HttpGet("admin-or-professional")]
    [Authorize(Policy = "AdminOrProfessional")]
    public IActionResult GetAdminOrProfessionalData()
    {
        return Ok(new { message = "Este endpoint es para Admin o Professional" });
    }

    /// <summary>
    /// Otra forma de proteger por roles (múltiples roles)
    /// </summary>
    [HttpGet("multiple-roles")]
    [Authorize(Roles = "Admin,Professional")]
    public IActionResult GetMultipleRolesData()
    {
        // Verificar el rol específico del usuario actual
        if (User.IsInRole("Admin"))
        {
            return Ok(new { message = "Acceso como Admin" });
        }
        else if (User.IsInRole("Professional"))
        {
            return Ok(new { message = "Acceso como Professional" });
        }

        return Forbid();
    }

    /// <summary>
    /// Endpoint que verifica roles manualmente
    /// </summary>
    [HttpGet("manual-role-check")]
    [Authorize]
    public IActionResult ManualRoleCheck()
    {
        // Obtener todos los roles del usuario
        var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

        // Verificación manual de roles
        if (!roles.Contains("Admin") && !roles.Contains("Professional"))
        {
            return Forbid(); // 403 Forbidden
        }

        return Ok(new 
        { 
            message = "Acceso permitido",
            userRoles = roles
        });
    }

    /// <summary>
    /// Ejemplo de endpoint que retorna información del usuario autenticado
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    public IActionResult GetCurrentUser()
    {
        var claims = User.Claims.Select(c => new 
        { 
            c.Type, 
            c.Value 
        }).ToList();

        return Ok(new
        {
            userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            name = User.FindFirst(ClaimTypes.Name)?.Value,
            email = User.FindFirst(ClaimTypes.Email)?.Value,
            roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList(),
            allClaims = claims
        });
    }

    /// <summary>
    /// Ejemplo de POST protegido
    /// </summary>
    [HttpPost("create-resource")]
    [Authorize(Roles = "Admin,Professional")]
    public IActionResult CreateResource([FromBody] object data)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        // Lógica para crear un recurso
        return Ok(new 
        { 
            message = "Recurso creado exitosamente",
            createdBy = userId
        });
    }

    /// <summary>
    /// Ejemplo de DELETE solo para Admin
    /// </summary>
    [HttpDelete("delete-resource/{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteResource(int id)
    {
        // Solo los Admin pueden eliminar
        return Ok(new { message = $"Recurso {id} eliminado exitosamente" });
    }
}
