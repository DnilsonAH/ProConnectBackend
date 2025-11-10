using Microsoft.OpenApi.Models;
using ProConnect_Backend.Application.Configuration;
using ProConnect_Backend.Application.Mapping;
using ProConnect_Backend.Configuration;
using ProConnect_Backend.Middleware;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Logging;

// Buscar .env en varios lugares (project root, parent, Infrastructure)
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
    Console.WriteLine("✅ Variables de entorno cargadas exitosamente");
}
else
{
    Console.WriteLine($"⚠️ Advertencia: No se encontró el archivo .env en rutas buscadas:");
    foreach (var p in candidatePaths) Console.WriteLine($"   - {p}");
    Console.WriteLine("💡 La aplicación usará las variables de entorno del sistema");
}

var builder = WebApplication.CreateBuilder(args);

// Validar y cargar configuraciones con variables de entorno
var dbServer = Environment.GetEnvironmentVariable("DB_SERVER");
var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
var dbDatabase = Environment.GetEnvironmentVariable("DB_DATABASE");
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

// Validar que las variables críticas estén configuradas
if (string.IsNullOrEmpty(dbServer) || string.IsNullOrEmpty(dbDatabase) || 
    string.IsNullOrEmpty(dbUser) || string.IsNullOrEmpty(dbPassword))
{
    Console.WriteLine("❌ ERROR: Variables de entorno de base de datos no configuradas correctamente");
    Console.WriteLine($"   DB_SERVER: {(string.IsNullOrEmpty(dbServer) ? "❌ NO CONFIGURADO" : "✅")}");
    Console.WriteLine($"   DB_PORT: {(string.IsNullOrEmpty(dbPort) ? "❌ NO CONFIGURADO" : "✅")}");
    Console.WriteLine($"   DB_DATABASE: {(string.IsNullOrEmpty(dbDatabase) ? "❌ NO CONFIGURADO" : "✅")}");
    Console.WriteLine($"   DB_USER: {(string.IsNullOrEmpty(dbUser) ? "❌ NO CONFIGURADO" : "✅")}");
    Console.WriteLine($"   DB_PASSWORD: {(string.IsNullOrEmpty(dbPassword) ? "❌ NO CONFIGURADO" : "✅")}");
    throw new InvalidOperationException("Variables de entorno de base de datos requeridas no están configuradas");
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

// Configurar certificados SSL para Google Cloud SQL
builder.Configuration["SslCertificates:ClientCert"] = Environment.GetEnvironmentVariable("DB_CLIENT_CERT");
builder.Configuration["SslCertificates:ClientKey"] = Environment.GetEnvironmentVariable("DB_CLIENT_KEY");
builder.Configuration["SslCertificates:ServerCa"] = Environment.GetEnvironmentVariable("DB_SERVER_CA");

builder.Configuration["JwtSettings:SecretKey"] = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
builder.Configuration["JwtSettings:Issuer"] = Environment.GetEnvironmentVariable("JWT_ISSUER");
builder.Configuration["JwtSettings:Audience"] = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
builder.Configuration["JwtSettings:ExpirationHours"] = Environment.GetEnvironmentVariable("JWT_EXPIRATION_HOURS");

// Log de configuración cargada
Console.WriteLine("⚙️ Configuración cargada:");
Console.WriteLine($"   - Base de datos: {dbDatabase} en {dbServer}:{dbPort ?? "3306"}");
Console.WriteLine($"   - Usuario DB: {dbUser}");
Console.WriteLine($"   - Password configurado: {(!string.IsNullOrEmpty(dbPassword) ? $"✅ (longitud: {dbPassword.Length} caracteres)" : "❌ NO CONFIGURADO")}");
Console.WriteLine($"   - JWT Issuer: {Environment.GetEnvironmentVariable("JWT_ISSUER")}");
Console.WriteLine($"   - JWT Audience: {Environment.GetEnvironmentVariable("JWT_AUDIENCE")}");
Console.WriteLine($"   - JWT Expiration: {Environment.GetEnvironmentVariable("JWT_EXPIRATION_HOURS")} horas");
Console.WriteLine($"   - Certificados SSL: {(!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DB_CLIENT_CERT")) ? "Configurados ✅" : "No configurados ⚠️")}");
Console.WriteLine();


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
            logger.LogInformation("🔒 Conexión SSL: Habilitada (Google Cloud SQL)");
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
        logger.LogError("   1. Los certificados SSL son válidos y no han expirado");
        logger.LogError("   2. La IP del servidor es accesible desde esta máquina");
        logger.LogError("   3. El usuario tiene permisos para conectarse con SSL");
        logger.LogError("   4. El puerto 3306 está abierto en el firewall");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "❌ Error al conectar con la base de datos");
        logger.LogError("💡 Verifica las credenciales y certificados SSL en el archivo .env");
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

app.UseAuthentication(); // Middleware de autenticación
app.UseMiddleware<TokenValidationMiddleware>(); // Middleware de validación de tokens revocados
app.UseAuthorization(); // Middleware de autorización

app.MapControllers();

app.Run();