using System.Text;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProConnect_Backend.Application.UseCases.Login.Query;
// using ProConnect_Backend.Domain.Interfaces; (not used)
using ProConnect_Backend.Domain.Ports;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Adapters;
using ProConnect_Backend.Infrastructure.Adapters.Repositories;
using ProConnect_Backend.Infrastructure.Data;
// using ProConnect_Backend.Infrastructure.Repositories; (not present)
/*using ProConnect_Backend.Domain.Ports.IServices;
using ProConnect_Backend.Infrastructure.Adapters;
using ProConnect_Backend.Infrastructure.Adapters.Repositories;
using ProConnect_Backend.Infrastructure.Data;*/
using ProConnect_Backend.Infrastructure.Services;

namespace ProConnect_Backend.Configuration;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddUserControllerServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // 1. Configuración del DbContext (MySQL con SSL para Google Cloud SQL)
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("ConnectionString 'DefaultConnection' no está configurada");
        
        // Obtener certificados SSL desde la configuración
        var clientCert = configuration["SslCertificates:ClientCert"];
        var clientKey = configuration["SslCertificates:ClientKey"];
        var serverCa = configuration["SslCertificates:ServerCa"];
        
        // Crear directorio temporal para los certificados si no existe
        var certPath = Path.Combine(Path.GetTempPath(), "mysql_certs");
        Directory.CreateDirectory(certPath);
        
        // Escribir certificados a archivos temporales
        var clientCertPath = Path.Combine(certPath, "client-cert.pem");
        var clientKeyPath = Path.Combine(certPath, "client-key.pem");
        var serverCaPath = Path.Combine(certPath, "server-ca.pem");
        
        if (!string.IsNullOrEmpty(clientCert) && !string.IsNullOrEmpty(clientKey) && !string.IsNullOrEmpty(serverCa))
        {
            try
            {
                File.WriteAllText(clientCertPath, clientCert);
                File.WriteAllText(clientKeyPath, clientKey);
                File.WriteAllText(serverCaPath, serverCa);
                
                Console.WriteLine("🔐 Certificados SSL escritos en archivos temporales");
                Console.WriteLine($"   - Ruta certificados: {certPath}");
                
                // Asegurar que la cadena de conexión base termine con punto y coma
                if (!connectionString.EndsWith(";"))
                {
                    connectionString += ";";
                }
                
                // Construir connection string con SSL
                connectionString += $"SslMode=Required;SslCa={serverCaPath};SslCert={clientCertPath};SslKey={clientKeyPath};";
                Console.WriteLine("✅ Conexión SSL configurada correctamente");
                Console.WriteLine($"🔍 Longitud cadena de conexión: {connectionString.Length} caracteres");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al escribir certificados SSL: {ex.Message}");
                throw;
            }
        }
        else
        {
            Console.WriteLine("⚠️ Certificados SSL no configurados - Conexión sin SSL");
        }
        
        services.AddDbContext<ProConnectDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                mySqlOptions => {
                    mySqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
                })
        );
        // Registro de UnitOfWrok
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        // Los repositorios específicos son creados por el UoW,
        //services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IScheduledRepository, ScheduledRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IVerificationDocumentRepository, VerificationDocumentRepository>();
        services.AddScoped<IVerificationRepository, VerificationRepository>();
        services.AddScoped<IWeeklyAvailabilityRepository, WeeklyAvailabilityRepository>();
        services.AddScoped<LoginCommandHandler>();
        // Register user command/query handlers
        services.AddScoped<ProConnect_Backend.Application.UseCases.Users.Command.RegisterCommandHandler>();
        services.AddScoped<ProConnect_Backend.Application.UseCases.Users.Query.GetUserByIdQueryHandler>();        // 3. Registro de Servicios (Adaptadores) servicios de terceros JWT, Hasheo de contraseñas

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        
        // 4. Configuración de Autenticación JWT
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey no configurada");

        
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
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });
        // Políticas de autorización
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            options.AddPolicy("ProfessionalOnly", policy => policy.RequireRole("Professional"));
            options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
            options.AddPolicy("AdminOrProfessional", policy => policy.RequireRole("Admin", "Professional"));
        });
       // services[]
        
        return services;
    }
}
