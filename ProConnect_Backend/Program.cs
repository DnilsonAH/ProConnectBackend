using Microsoft.OpenApi.Models;
using ProConnect_Backend.Application.Configuration;
using ProConnect_Backend.Application.Mapping;
using ProConnect_Backend.Configuration;
using ProConnect_Backend.Middleware;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Logging;

// ==============================================================================
// 0. CONFIGURACIÓN DE ENTORNO LOCAL
// ==============================================================================

// Intentar cargar .env solo en desarrollo local (no en Docker/Fly.io)
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
var isRunningInContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER")?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false;
var isLocalDevelopment = !isRunningInContainer;

if (isLocalDevelopment)
{
    var candidatePaths = new[]
    {
        Path.Combine(Directory.GetCurrentDirectory(), ".env"),
        Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.FullName, ".env"),
        Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.FullName, "ProConnect_Backend.Infrastructure", ".env")
    };

    var envPath = candidatePaths.FirstOrDefault(File.Exists);

    if (!string.IsNullOrEmpty(envPath))
    {
        Console.WriteLine($"📂 Cargando archivo .env desde: {envPath}");
        Env.Load(envPath);
        Console.WriteLine("✅ Variables de entorno cargadas exitosamente desde .env");
    }
    else
    {
        Console.WriteLine($"⚠️ Advertencia: No se encontró el archivo .env en desarrollo local");
        Console.WriteLine("Rutas buscadas:");
        foreach (var p in candidatePaths) Console.WriteLine($"   - {p}");
    }
}
else
{
    Console.WriteLine($"🌐 Entorno: {environment} - Usando variables de entorno del sistema (Fly.io secrets)");
}

var builder = WebApplication.CreateBuilder(args);

// ==============================================================================
// 1. CARGA DE VARIABLES DE ENTORNO (Lectura cruda)
// ==============================================================================

// Base de Datos
var dbServer = Environment.GetEnvironmentVariable("DB_SERVER");
var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
var dbDatabase = Environment.GetEnvironmentVariable("DB_DATABASE");
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

// JWT
var jwtSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
var jwtExpirationHours = Environment.GetEnvironmentVariable("JWT_EXPIRATION_HOURS");

// ZegoCloud (CORRECCIÓN AQUÍ: Usamos SERVER_SECRET, no APP_SIGN)
var zegoAppId = Environment.GetEnvironmentVariable("ZEGOCLOUD_APP_ID");
var zegoServerSecret = Environment.GetEnvironmentVariable("ZEGOCLOUD_SERVER_SECRET"); 

// ==============================================================================
// 2. VALIDACIÓN DE CONFIGURACIÓN CRÍTICA
// ==============================================================================

if (string.IsNullOrEmpty(dbServer) || string.IsNullOrEmpty(dbDatabase) || 
    string.IsNullOrEmpty(dbUser) || string.IsNullOrEmpty(dbPassword) ||
    string.IsNullOrEmpty(jwtSecretKey) || string.IsNullOrEmpty(jwtIssuer) || 
    string.IsNullOrEmpty(jwtAudience) ||
    string.IsNullOrEmpty(zegoAppId) || string.IsNullOrEmpty(zegoServerSecret)) 
{
    Console.WriteLine("❌ ERROR: Variables de entorno no configuradas correctamente");
    
    // Log de diagnóstico
    Console.WriteLine("Base de datos:");
    Console.WriteLine($"   DB_SERVER: {(string.IsNullOrEmpty(dbServer) ? "❌" : "✅")}");
    Console.WriteLine($"   DB_DATABASE: {(string.IsNullOrEmpty(dbDatabase) ? "❌" : "✅")}");
    Console.WriteLine("JWT:");
    Console.WriteLine($"   JWT_SECRET_KEY: {(string.IsNullOrEmpty(jwtSecretKey) ? "❌" : "✅")}");
    Console.WriteLine("ZegoCloud:");
    Console.WriteLine($"   ZEGOCLOUD_APP_ID: {(string.IsNullOrEmpty(zegoAppId) ? "❌ NO CONFIGURADO" : "✅")}");
    // Validamos ServerSecret en lugar de AppSign
    Console.WriteLine($"   ZEGOCLOUD_SERVER_SECRET: {(string.IsNullOrEmpty(zegoServerSecret) ? "❌ NO CONFIGURADO" : "✅")}");
    
    throw new InvalidOperationException("Variables de entorno requeridas no están configuradas");
}

// ==============================================================================
// 3. MAPEO DE ENTORNO A CONFIGURACIÓN DE .NET
// ==============================================================================

// Sobrescribir Connection Strings
var fullConnectionString = $"Server={dbServer};" +
    $"Port={dbPort ?? "3306"};" +
    $"Database={dbDatabase};" +
    $"Uid={dbUser};" +
    $"Pwd={dbPassword};";

builder.Configuration["ConnectionStrings:DefaultConnection"] = fullConnectionString;

// Sobrescribir Configuración JWT
builder.Configuration["JwtSettings:SecretKey"] = jwtSecretKey;
builder.Configuration["JwtSettings:Issuer"] = jwtIssuer;
builder.Configuration["JwtSettings:Audience"] = jwtAudience;
builder.Configuration["JwtSettings:ExpirationHours"] = jwtExpirationHours ?? "24";

// Sobrescribir Configuración ZegoCloud (Aquí ocurre la magia del puente)
// El servicio ZegoCloudService busca "ZegoCloud:AppId", así que se lo damos aquí.
builder.Configuration["ZegoCloud:AppId"] = zegoAppId;
// El servicio busca "ZegoCloud:ServerSecret", le asignamos tu variable ZEGOCLOUD_SERVER_SECRET
builder.Configuration["ZegoCloud:ServerSecret"] = zegoServerSecret;

// Log de configuración cargada (solo info segura)
Console.WriteLine("⚙️ Configuración cargada:");
Console.WriteLine($"   - Base de datos: {dbDatabase} en {dbServer}");
Console.WriteLine($"   - JWT Issuer: {jwtIssuer}");
Console.WriteLine($"   - Zego AppID: {zegoAppId}"); 
// Mostramos longitud para verificar que cargó algo, pero no el secreto
Console.WriteLine($"   - Zego Secret: ✅ (Longitud: {zegoServerSecret?.Length ?? 0} caracteres)");
Console.WriteLine();


// Configuración CORS para permitir peticiones desde el frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Permite enviar cookies y autenticación
    });
});

// Registro de servicios generales
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registramos nuestros servicios de capas (Infraestructura, Aplicación, etc.)
builder.Services.AddUserControllerServices(builder.Configuration);
builder.Services.AddApplicationServices();

/* +---------------------------------------------------------------------------------------------------------+
   |                                        Construcción de la aplicación                                    |
   +---------------------------------------------------------------------------------------------------------+*/

// Necesario para acceder al HttpContext en los servicios
builder.Services.AddHttpContextAccessor();

// 3. Configurar Swagger/OpenAPI para que soporte JWT
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ProConnect API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Autorización JWT: Bearer {token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build(); // Construir la aplicación

// Verificar conexión a la base de datos al iniciar
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    
    try
    {
        var dbContext = services.GetRequiredService<ProConnect_Backend.Infrastructure.Data.ProConnectDbContext>();
        
        logger.LogInformation("🔄 Intentando conectar a la base de datos...");
        
        // Obtener la cadena de conexión actual para debug (sin contraseña)
        var connectionString = dbContext.Database.GetDbConnection().ConnectionString;
        logger.LogInformation("🔗 Cadena de conexión (sin password): {ConnectionString}", 
            connectionString?.Replace(dbPassword ?? "", "***"));
        
        // Intentar abrir la conexión
        var canConnect = await dbContext.Database.CanConnectAsync();
        
        if (canConnect)
        {
            logger.LogInformation("✅ Conexión a la base de datos establecida exitosamente");
        }
        else
        {
            logger.LogWarning("⚠️ No se pudo verificar la conexión a la base de datos");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "❌ Error al conectar con la base de datos");
        // throw; // Descomentar en producción estricta
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
/*var now = DateTime.UtcNow;
Console.WriteLine($"Hora del servidor {now}"); */
// IMPORTANTE: UseCors debe estar ANTES de UseAuthentication y UseAuthorization
app.UseCors("AllowFrontend");

app.UseAuthentication(); // Middleware de autenticación
app.UseMiddleware<TokenValidationMiddleware>(); // Middleware de validación de tokens revocados
app.UseAuthorization(); // Middleware de autorización

app.MapControllers();

app.Run();