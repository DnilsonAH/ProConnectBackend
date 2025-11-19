using Microsoft.OpenApi.Models;
using ProConnect_Backend.Application.Configuration;
using ProConnect_Backend.Application.Mapping;
using ProConnect_Backend.Configuration;
using ProConnect_Backend.Middleware;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Logging;

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

// Validar y cargar configuraciones con variables de entorno
var dbServer = Environment.GetEnvironmentVariable("DB_SERVER");
var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
var dbDatabase = Environment.GetEnvironmentVariable("DB_DATABASE");
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

// Configuración JWT
var jwtSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
var jwtExpirationHours = Environment.GetEnvironmentVariable("JWT_EXPIRATION_HOURS");

// Validar que las variables críticas estén configuradas
if (string.IsNullOrEmpty(dbServer) || string.IsNullOrEmpty(dbDatabase) || 
    string.IsNullOrEmpty(dbUser) || string.IsNullOrEmpty(dbPassword) ||
    string.IsNullOrEmpty(jwtSecretKey) || string.IsNullOrEmpty(jwtIssuer) || 
    string.IsNullOrEmpty(jwtAudience))
{
    Console.WriteLine("❌ ERROR: Variables de entorno no configuradas correctamente");
    Console.WriteLine("Base de datos:");
    Console.WriteLine($"   DB_SERVER: {(string.IsNullOrEmpty(dbServer) ? "❌ NO CONFIGURADO" : "✅")}");
    Console.WriteLine($"   DB_PORT: {(string.IsNullOrEmpty(dbPort) ? "❌ NO CONFIGURADO (usando 3306)" : "✅")}");
    Console.WriteLine($"   DB_DATABASE: {(string.IsNullOrEmpty(dbDatabase) ? "❌ NO CONFIGURADO" : "✅")}");
    Console.WriteLine($"   DB_USER: {(string.IsNullOrEmpty(dbUser) ? "❌ NO CONFIGURADO" : "✅")}");
    Console.WriteLine($"   DB_PASSWORD: {(string.IsNullOrEmpty(dbPassword) ? "❌ NO CONFIGURADO" : "✅")}");
    Console.WriteLine("JWT:");
    Console.WriteLine($"   JWT_SECRET_KEY: {(string.IsNullOrEmpty(jwtSecretKey) ? "❌ NO CONFIGURADO" : "✅")}");
    Console.WriteLine($"   JWT_ISSUER: {(string.IsNullOrEmpty(jwtIssuer) ? "❌ NO CONFIGURADO" : "✅")}");
    Console.WriteLine($"   JWT_AUDIENCE: {(string.IsNullOrEmpty(jwtAudience) ? "❌ NO CONFIGURADO" : "✅")}");
    Console.WriteLine($"   JWT_EXPIRATION_HOURS: {(string.IsNullOrEmpty(jwtExpirationHours) ? "⚠️ NO CONFIGURADO (usando default)" : "✅")}");
    throw new InvalidOperationException("Variables de entorno requeridas no están configuradas");
}

// Sobrescribir configuraciones con variables de entorno
var fullConnectionString = $"Server={dbServer};" +
    $"Port={dbPort ?? "3306"};" +
    $"Database={dbDatabase};" +
    $"Uid={dbUser};" +
    $"Pwd={dbPassword};";

builder.Configuration["ConnectionStrings:DefaultConnection"] = fullConnectionString;

// Log para verificar la cadena de conexión (ofuscada)
Console.WriteLine("🔗 Cadena de conexión generada:");
Console.WriteLine($"   {fullConnectionString.Replace(dbPassword, "***PASSWORD***")}");
Console.WriteLine();

builder.Configuration["JwtSettings:SecretKey"] = jwtSecretKey;
builder.Configuration["JwtSettings:Issuer"] = jwtIssuer;
builder.Configuration["JwtSettings:Audience"] = jwtAudience;
builder.Configuration["JwtSettings:ExpirationHours"] = jwtExpirationHours ?? "24";

// Log de configuración cargada
Console.WriteLine("⚙️ Configuración cargada:");
Console.WriteLine($"   - Base de datos: {dbDatabase} en {dbServer}:{dbPort ?? "3306"}");
Console.WriteLine($"   - Usuario DB: {dbUser}");
Console.WriteLine($"   - Password configurado: {(!string.IsNullOrEmpty(dbPassword) ? $"✅ (longitud: {dbPassword.Length} caracteres)" : "❌ NO CONFIGURADO")}");
Console.WriteLine($"   - JWT Issuer: {jwtIssuer}");
Console.WriteLine($"   - JWT Audience: {jwtAudience}");
Console.WriteLine($"   - JWT SecretKey configurado: ✅ (longitud: {jwtSecretKey?.Length ?? 0} caracteres)");
Console.WriteLine($"   - JWT Expiration: {jwtExpirationHours ?? "24"} horas");
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

builder.Services.AddUserControllerServices(builder.Configuration);
builder.Services.AddApplicationServices();



/* +---------------------------------------------------------------------------------------------------------+
   |                                        Construcción de la aplicación                                    |
   +---------------------------------------------------------------------------------------------------------+*/
// Necesario para acceder al HttpContext en los servicios

// Necesario para acceder al HttpContext en los servicios
builder.Services.AddHttpContextAccessor();
// 3. Configurar Swagger/OpenAPI para que soporte JWT
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ArchitectureLAB10 API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Autorización JWT: Bearer)"
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

// Verificar conexión a la base de datos
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    
    try
    {
        var dbContext = services.GetRequiredService<ProConnect_Backend.Infrastructure.Data.ProConnectDbContext>();
        
        logger.LogInformation("🔄 Intentando conectar a la base de datos...");
        logger.LogInformation("📍 Servidor: {Server}", Environment.GetEnvironmentVariable("DB_SERVER"));
        logger.LogInformation("📊 Base de datos: {Database}", Environment.GetEnvironmentVariable("DB_DATABASE"));
        
        // Obtener la cadena de conexión actual para debug
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
    catch (MySqlConnector.MySqlException mysqlEx)
    {
        logger.LogError("❌ Error específico de MySQL:");
        logger.LogError("   - Código de error: {ErrorCode}", mysqlEx.ErrorCode);
        logger.LogError("   - Número de error: {Number}", mysqlEx.Number);
        logger.LogError("   - Mensaje: {Message}", mysqlEx.Message);
        logger.LogError("   - SqlState: {SqlState}", mysqlEx.SqlState);
        
        if (mysqlEx.InnerException != null)
        {
            logger.LogError("   - Error interno: {InnerMessage}", mysqlEx.InnerException.Message);
        }
        
        logger.LogError("💡 Verifica:");
        logger.LogError("   1. La IP del servidor es accesible desde esta máquina");
        logger.LogError("   2. Las credenciales de base de datos son correctas");
        logger.LogError("   3. El puerto está abierto en el firewall");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "❌ Error al conectar con la base de datos");
        logger.LogError("💡 Verifica las credenciales en el archivo .env");
        logger.LogError("🔍 Tipo de error: {ExceptionType}", ex.GetType().Name);
        logger.LogError("🔍 Detalles del error: {Message}", ex.Message);
        
        if (ex.InnerException != null)
        {
            logger.LogError("🔍 Error interno: {InnerMessage}", ex.InnerException.Message);
        }
        
        // No lanzar la excepción para que la aplicación pueda iniciar y mostrar el error en Swagger
        // throw; // Descomenta esta línea si quieres que la aplicación no inicie sin conexión DB
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// IMPORTANTE: UseCors debe estar ANTES de UseAuthentication y UseAuthorization
app.UseCors("AllowFrontend");

app.UseAuthentication(); // Middleware de autenticación
app.UseMiddleware<TokenValidationMiddleware>(); // Middleware de validación de tokens revocados
app.UseAuthorization(); // Middleware de autorización

app.MapControllers();

app.Run();