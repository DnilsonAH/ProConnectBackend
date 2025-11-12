# 🌐 API Layer / Presentation Layer (Capa de Presentación)

## 📋 Descripción General

La **capa de API/Presentación** es el punto de entrada del sistema y maneja las peticiones HTTP. Esta capa:

- ✅ **Expone endpoints REST** para los clientes
- ✅ Maneja **peticiones y respuestas HTTP**
- ✅ **Valida datos de entrada** (ModelState)
- ✅ **Transforma DTOs ↔ JSON**
- ✅ Maneja **autenticación y autorización**
- ✅ Implementa **middleware** personalizado
- ✅ Configura **Swagger/OpenAPI**

**Dependencias**: Application Layer + Infrastructure Layer (solo para DI)

---

## 📁 Estructura de Carpetas

### 🗂️ **`Controllers/`**
**Propósito**: Controladores que exponen endpoints REST

**Contenido actual**:
- `AuthController.cs` - Endpoints de autenticación (Login, Register, Logout, GetUser)
- `UserController.cs` - Endpoints de gestión de usuarios

**Características**:
- ✅ Heredan de `ControllerBase`
- ✅ Usan atributos `[ApiController]` y `[Route]`
- ✅ Inyectan **Handlers** (no repositorios directamente)
- ✅ Validan `ModelState` automáticamente
- ✅ Retornan `IActionResult` (Ok, BadRequest, Unauthorized, etc.)

---

#### **`AuthController.cs`**
**Propósito**: Maneja autenticación y autorización

**Endpoints implementados**:

| Método | Ruta | Autenticación | Descripción |
|--------|------|---------------|-------------|
| POST | `/api/auth/login` | ❌ Pública | Inicio de sesión |
| POST | `/api/auth/register` | ❌ Pública | Registro de usuario |
| POST | `/api/auth/logout` | ✅ Requerida | Cerrar sesión |
| GET | `/api/auth/user` | ✅ Requerida | Info usuario autenticado |
| GET | `/api/auth/user/{id}` | ✅ Requerida | Info usuario por ID |

**Ejemplo**: Login Endpoint
```csharp
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly LoginCommandHandler _loginHandler;
    private readonly ILogger<AuthController> _logger;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        try
        {
            // 1. Validar ModelState (automático con [ApiController])
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("❌ Datos inválidos enviados a login");
                return BadRequest(new
                {
                    success = false,
                    message = "⚠️ Los datos enviados no son válidos.",
                    errors = ModelState
                });
            }

            // 2. Crear comando y delegar al handler
            var command = new LoginCommand(dto);
            var result = await _loginHandler.Handle(command);

            // 3. Manejar resultado
            if (result == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "🚫 Correo o contraseña incorrectos."
                });
            }

            // 4. Retornar respuesta exitosa
            _logger.LogInformation("✅ Usuario autenticado: {Email}", result.Email);
            return Ok(new
            {
                success = true,
                message = "🎉 Inicio de sesión exitoso.",
                data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "💥 Error durante login");
            return StatusCode(500, new
            {
                success = false,
                message = "💥 Error interno.",
                details = ex.Message
            });
        }
    }
}
```

**Responsabilidades del Controller**:
1. ✅ Recibir y validar la petición HTTP
2. ✅ Convertir JSON → DTO
3. ✅ Crear Command/Query
4. ✅ Llamar al Handler (Application Layer)
5. ✅ Convertir resultado → HTTP Response
6. ✅ Manejar errores y logging
7. ✅ Aplicar atributos de autorización `[Authorize]`

---

### 🗂️ **`Middleware/`**
**Propósito**: Middleware personalizado para procesamiento de peticiones

**Contenido actual**:
- `TokenValidationMiddleware.cs` - Valida tokens JWT contra blacklist

---

#### **`TokenValidationMiddleware.cs`**
**Propósito**: Intercepta peticiones y valida si el JWT está revocado

**Flujo**:
```
Request con Authorization: Bearer <token>
    ↓
TokenValidationMiddleware
    ↓
    ├─ Extrae el JTI del token
    ├─ Consulta JwtBlacklistRepository
    ├─ Si está en blacklist → 401 Unauthorized
    └─ Si NO está → continúa al siguiente middleware
    ↓
AuthenticationMiddleware (JWT estándar)
    ↓
AuthorizationMiddleware
    ↓
Controller
```

**Ejemplo**:
```csharp
public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TokenValidationMiddleware> _logger;

    public async Task InvokeAsync(HttpContext context, IUnitOfWork unitOfWork)
    {
        var authHeader = context.Request.Headers["Authorization"].ToString();

        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            var token = authHeader.Substring("Bearer ".Length).Trim();

            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);
                var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

                if (!string.IsNullOrEmpty(jti))
                {
                    // Verificar si está en blacklist
                    var isRevoked = await unitOfWork.JwtBlacklistRepository.IsTokenRevokedAsync(jti);

                    if (isRevoked)
                    {
                        _logger.LogWarning("🚫 Token revocado detectado");
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsJsonAsync(new
                        {
                            success = false,
                            message = "🚫 Token revocado. Por favor, inicia sesión nuevamente."
                        });
                        return; // No continúa
                    }
                }
            }
        }

        await _next(context); // Continúa al siguiente middleware
    }
}
```

**Características**:
- ✅ Se ejecuta ANTES de AuthenticationMiddleware
- ✅ Inyecta IUnitOfWork vía método (no constructor)
- ✅ Valida blacklist sin afectar validación JWT estándar
- ✅ Retorna 401 si el token está revocado

---

### 🗂️ **`Configuration/`**
**Propósito**: Configuración de servicios y Dependency Injection

**Contenido actual**:
- `ServiceRegistrationExtensions.cs` - Registra todos los servicios de la aplicación

---

#### **`ServiceRegistrationExtensions.cs`**
**Propósito**: Centraliza el registro de servicios en el contenedor de DI

**Servicios registrados**:

**1. DbContext + Repositorios + UnitOfWork**
```csharp
services.AddDbContext<ProConnectDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IJwtBlacklistRepository, JwtBlacklistRepository>();
// ... 11 repositorios más
services.AddScoped<IUnitOfWork, UnitOfWork>();
```

**2. Servicios de Infraestructura**
```csharp
services.AddScoped<IPasswordHasher, PasswordHasher>();
services.AddScoped<IJwtTokenService, JwtTokenService>();
```

**3. Handlers de Application**
```csharp
services.AddScoped<LoginCommandHandler>();
services.AddScoped<RegisterCommandHandler>();
services.AddScoped<GetUserByIdQueryHandler>();
services.AddScoped<LogoutCommandHandler>();
```

**4. Autenticación JWT**
```csharp
services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["JwtSettings:Issuer"],
        ValidAudience = configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero
    };
});
```

**5. Políticas de Autorización**
```csharp
services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ProfessionalOnly", policy => policy.RequireRole("Professional"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
    options.AddPolicy("AdminOrProfessional", policy => policy.RequireRole("Admin", "Professional"));
});
```

**6. Detección de Certificados SSL**
```csharp
var certPath = Path.Combine(solutionRoot, "ssl-certs");
bool hasCertificates = File.Exists(clientCertPath) && File.Exists(clientKeyPath) && File.Exists(serverCaPath);

if (hasCertificates)
{
    // SSL con certificados de cliente (Desarrollo)
    connectionString += $"SslMode=Required;SslCa={serverCaPath};SslCert={clientCertPath};SslKey={clientKeyPath};";
    Console.WriteLine("🛠️ Entorno: DESARROLLO");
}
else
{
    // SSL sin certificados de cliente (Producción)
    connectionString += "SslMode=Required;";
    Console.WriteLine("🚀 Entorno: PRODUCCIÓN");
}
```

---

### 🗂️ **`Program.cs`**
**Propósito**: Punto de entrada de la aplicación

**Responsabilidades**:
1. ✅ Cargar variables de entorno desde `.env`
2. ✅ Configurar servicios (llamando a `ServiceRegistrationExtensions`)
3. ✅ Configurar Application Layer (AutoMapper, MediatR)
4. ✅ Configurar middleware pipeline
5. ✅ Verificar conexión a base de datos
6. ✅ Configurar Swagger
7. ✅ Ejecutar la aplicación

**Pipeline de Middleware**:
```csharp
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication(); // JWT validation
app.UseMiddleware<TokenValidationMiddleware>(); // Custom blacklist check
app.UseAuthorization(); // Role-based authorization

app.MapControllers(); // Route endpoints

app.Run(); // Start listening
```

**Orden del pipeline**:
1. Swagger (solo desarrollo)
2. **Authentication** → valida JWT
3. **TokenValidationMiddleware** → valida blacklist
4. **Authorization** → verifica roles/policies
5. Controllers → ejecutan handlers

---

### 🗂️ **`Properties/`**
**Propósito**: Configuración de lanzamiento

**Contenido**:
- `launchSettings.json` - Configuración de perfiles de ejecución

---

### 🗂️ **`appsettings.json` / `appsettings.Development.json`**
**Propósito**: Configuración de la aplicación

**⚠️ EXCLUIDOS DEL REPOSITORIO** (en `.gitignore`)

**Estructura esperada**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Port=3306;Database=...;Uid=...;Pwd=...;"
  },
  "JwtSettings": {
    "SecretKey": "tu-secret-key-super-secreta-de-al-menos-32-caracteres",
    "Issuer": "ProConnectAPI",
    "Audience": "ProConnectClient",
    "ExpirationHours": "72"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

**Nota**: Actualmente se usan variables de entorno (`.env`) en lugar de appsettings

---

## 🎯 Formato de Respuestas REST

**Todas las respuestas siguen este formato consistente**:

### ✅ **Respuesta Exitosa**
```json
{
  "success": true,
  "message": "🎉 Inicio de sesión exitoso",
  "data": {
    "id": 1,
    "name": "Juan",
    "email": "juan@example.com",
    "role": "Client",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
  }
}
```

### ❌ **Respuesta de Error**
```json
{
  "success": false,
  "message": "🚫 Correo o contraseña incorrectos"
}
```

### ⚠️ **Validación Fallida**
```json
{
  "success": false,
  "message": "⚠️ Los datos enviados no son válidos",
  "errors": {
    "Email": ["El correo electrónico es requerido"],
    "Password": ["La contraseña debe tener al menos 6 caracteres"]
  }
}
```

---

## 🔒 Seguridad Implementada

### 1. **Autenticación JWT**
- Tokens firmados con HMAC SHA256
- Claims: `NameIdentifier`, `Email`, `Role`, `Jti` (unique ID)
- Expiración configurable (72 horas por defecto)
- Validación automática en cada request

### 2. **Blacklist de Tokens**
- Al hacer logout, el token se agrega a `jwt_blacklist`
- Middleware valida contra blacklist antes de autenticar
- Tokens revocados no pueden usarse aunque sean válidos

### 3. **Autorización por Roles**
- Políticas: `AdminOnly`, `ProfessionalOnly`, `UserOnly`, `AdminOrProfessional`
- Atributo `[Authorize]` en controllers/endpoints
- Ejemplo: `[Authorize(Policy = "AdminOnly")]`

### 4. **HTTPS/SSL**
- Conexión SSL a MySQL (Google Cloud SQL)
- Certificados opcionales (desarrollo vs producción)

---

## 📦 Dependencias (Paquetes NuGet)

**ASP.NET Core**:
- `Microsoft.AspNetCore.OpenApi` (9.0.0)
- `Swashbuckle.AspNetCore` (6.6.2) - Swagger UI

**Variables de Entorno**:
- `DotNetEnv` (3.1.1) - Cargar archivos .env

**Autenticación**:
- `Microsoft.AspNetCore.Authentication.JwtBearer` (9.0.0)

**Referencias de Proyecto**:
- `ProConnect_Backend.Application`
- `ProConnect_Backend.Infrastructure` (solo para DI)

---

## 🚀 Buenas Prácticas

1. ✅ **Controllers delgados** (solo coordinan, no implementan lógica)
2. ✅ **Validar ModelState** en cada endpoint
3. ✅ **Logging con emojis** para fácil identificación
4. ✅ **Respuestas consistentes** (success, message, data)
5. ✅ **Manejo de excepciones** con try-catch
6. ✅ **No exponer stack traces** en producción
7. ✅ **Usar [Authorize]** para endpoints protegidos
8. ✅ **Swagger documentado** para testing
9. ✅ **Variables de entorno** para secretos

---

## 🔄 Flujo Completo de una Petición

```
1. Cliente HTTP (POST /api/auth/login)
    ↓
2. Kestrel (servidor web)
    ↓
3. Middleware Pipeline
    ├─ Swagger (si desarrollo)
    ├─ Authentication (valida JWT)
    ├─ TokenValidationMiddleware (valida blacklist)
    └─ Authorization (valida roles)
    ↓
4. AuthController.Login()
    ├─ Valida ModelState
    ├─ Crea LoginCommand
    └─ Llama LoginCommandHandler
    ↓
5. LoginCommandHandler (Application Layer)
    ├─ Busca usuario (IUserRepository)
    ├─ Verifica contraseña (IPasswordHasher)
    ├─ Genera token (IJwtTokenService)
    └─ Retorna LoginResponseDto
    ↓
6. AuthController
    ├─ Convierte DTO → JSON
    └─ Retorna 200 OK
    ↓
7. Cliente HTTP recibe respuesta
```

---

## 🎓 Patrones Implementados

- ✅ **REST API** (Representational State Transfer)
- ✅ **MVC Pattern** (Model-View-Controller, sin View)
- ✅ **Dependency Injection**
- ✅ **Middleware Pipeline**
- ✅ **JWT Authentication**
- ✅ **Role-Based Authorization**

---

**Última actualización**: Noviembre 2025  
**Puerto**: http://localhost:5200  
**Swagger**: http://localhost:5200/swagger
