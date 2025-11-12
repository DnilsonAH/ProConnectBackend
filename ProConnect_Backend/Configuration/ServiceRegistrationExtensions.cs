using System.Text;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProConnect_Backend.Domain.Ports;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Domain.Ports.IServices;
using ProConnect_Backend.Infrastructure.Adapters;
using ProConnect_Backend.Infrastructure.Adapters.Repositories;
using ProConnect_Backend.Infrastructure.Data;
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
        
        // Buscar certificados SSL en la carpeta ssl-certs del proyecto
        var solutionRoot = Directory.GetParent(Directory.GetCurrentDirectory())?.FullName 
                          ?? Directory.GetCurrentDirectory();
        var certPath = Path.Combine(solutionRoot, "ssl-certs");
        
        // Asegurar que la carpeta existe
        Directory.CreateDirectory(certPath);
        
        var clientCertPath = Path.Combine(certPath, "client-cert.pem");
        var clientKeyPath = Path.Combine(certPath, "client-key.pem");
        var serverCaPath = Path.Combine(certPath, "server-ca.pem");
        
        // Verificar si existen los 3 certificados
        bool hasCertificates = File.Exists(clientCertPath) && 
                              File.Exists(clientKeyPath) && 
                              File.Exists(serverCaPath);
        
        // Asegurar que la cadena de conexión base termine con punto y coma
        if (!connectionString.EndsWith(";"))
        {
            connectionString += ";";
        }
        
        if (hasCertificates)
        {
            // Conexión SSL con certificados de cliente (autenticación mutua TLS)
            connectionString += $"SslMode=Required;SslCa={serverCaPath};SslCert={clientCertPath};SslKey={clientKeyPath};";
            Console.WriteLine("🔐 Certificados SSL encontrados - Usando autenticación con certificados de cliente");
            Console.WriteLine($"   📁 Ruta: {certPath}");
            Console.WriteLine($"   ✅ client-cert.pem");
            Console.WriteLine($"   ✅ client-key.pem");
            Console.WriteLine($"   ✅ server-ca.pem");
            Console.WriteLine($"   🛠️ Entorno: DESARROLLO (Development)");
        }
        else
        {
            // Conexión SSL sin certificados de cliente (solo cifrado)
            connectionString += "SslMode=Required;";
            Console.WriteLine("⚠️ Certificados SSL no encontrados - Usando SSL sin certificados de cliente");
            Console.WriteLine($"   📁 Buscado en: {certPath}");
            Console.WriteLine($"   💡 Coloca los certificados ahí si deseas autenticación con certificados");
            Console.WriteLine($"   🚀 Entorno: PRODUCCIÓN (Production)");
        }
        
        // 2. Configuración del DbContext con MySQL y SSL
        services.AddDbContext<ProConnectDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                mySqlOptions => {
                    mySqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
                })
        );
        
        // Registro de Repositorios (13 repositorios específicos)
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJwtBlacklistRepository, JwtBlacklistRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IProfessionalProfileRepository, ProfessionalProfileRepository>();
        services.AddScoped<IProfessionRepository, ProfessionRepository>();
        services.AddScoped<IProfessionCategoryRepository, ProfessionCategoryRepository>();
        services.AddScoped<ISpecializationRepository, SpecializationRepository>();
        services.AddScoped<IVerificationRepository, VerificationRepository>();
        services.AddScoped<IVerificationDocumentRepository, VerificationDocumentRepository>();
        services.AddScoped<IWeeklyAvailabilityRepository, WeeklyAvailabilityRepository>();
        services.AddScoped<IScheduledRepository, ScheduledRepository>();
        
        // Registro de UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        // 3. Registro de Servicios de Infraestructura (JWT, Hasheo de contraseñas)
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        
        // Registro de Handlers de MediatR (Commands y Queries)
        services.AddScoped<ProConnect_Backend.Application.UseCases.Login.Command.LoginCommandHandler>();
        services.AddScoped<ProConnect_Backend.Application.UseCases.Users.Command.RegisterCommandHandler>();
        services.AddScoped<ProConnect_Backend.Application.UseCases.Users.Query.GetUserByIdQueryHandler>();
        services.AddScoped<ProConnect_Backend.Application.UseCases.Logout.Command.LogoutCommandHandler>();
        
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
