using System.Text;
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
        // 1. Configuración del DbContext (MySQL)
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ProConnectDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                mySqlOptions => mySqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null))
        );
        // Registro de UnitOfWrok
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        // Los repositorios específicos son creados por el UoW,
        // pero si necesitaras inyectar uno directamente, lo registras aquí:
        
        /*esto asi o que cree repositorio internanmente en el unitofwork con el dbcontext:
         public class UnitOfWork : IUnitOfWork
        {
            private readonly ProConnectDbContext _dbContext;

            public IAvailabilityRepository AvailabilityRepository { get; }
            public IUserRepository UserRepository { get; }

            public UnitOfWork(ProConnectDbContext dbContext)
            {
                _dbContext = dbContext;
                AvailabilityRepository = new AvailabilityRepository(_dbContext);
                UserRepository = new UserRepository(_dbContext);
                // y así con los demás
            }

            public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
                => await _dbContext.SaveChangesAsync(cancellationToken);

            public void Dispose()
            {
                _dbContext.Dispose();
                GC.SuppressFinalize(this);
            }
        }
         */
        services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();
        services.AddScoped<IConsultationRepository, ConsultationRepository>();
        services.AddScoped<IDistributionRepository, DistributionRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IProfessionalPaymentInfoRepository, ProfessionalPaymentInfoRepository>();
        services.AddScoped<IProfessionalProfileRepository, ProfessionalProfileRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<IVerificationDocumentRepository, VerificationDocumentRepository>();
        services.AddScoped<IVerificationRepository, VerificationRepository>();
        services.AddScoped<IVideoCallRepository, VideoCallRepository>();

        
        // 3. Registro de Servicios (Adaptadores) servicios de terceros JWT, Hasheo de contraseñas
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtService, JwtService>();
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
      
        
        return services;
    }
}
