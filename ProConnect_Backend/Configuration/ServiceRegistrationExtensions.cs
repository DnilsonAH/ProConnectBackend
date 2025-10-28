using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProConnect_Backend.Domain.Ports;
using ProConnect_Backend.Infrastructure.Adapters;
using ProConnect_Backend.Infrastructure.Data;

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
        // services.AddScoped<IUserRepository, UserRepository>();
        
        // 3. Registro de Servicios (Adaptadores) servicios de terceros JWT, Hasheo de contraseñas
        //services.AddScoped<IPasswordHasher, PasswordHasher>();
        //services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        
        // 4. Configuración de Autenticación JWT
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]!))
                };
            });

        
        
        return services;
    }
}