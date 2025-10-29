using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ProConnect_Backend.Application.Mapping;
using ProConnect_Backend.Application.Ports;
using ProConnect_Backend.Application.Services;
using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports;
using ProConnect_Backend.Domain.Ports.IRepositories;
using ProConnect_Backend.Infrastructure.Adapters;
using ProConnect_Backend.Infrastructure.Adapters.Repositories;
using ProConnect_Backend.Infrastructure.Data;
using ProConnect_Backend.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Obtener la cadena de conexión desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

/* +---------------------------------------------------------------------------------------------------------+
   |                                        Registro de servicios                                            |
   +---------------------------------------------------------------------------------------------------------+*/
builder.Services.AddControllers(); // Agregar servicios de controladores
builder.Services.AddEndpointsApiExplorer(); // Agregar servicios para explorar endpoints
builder.Services.AddSwaggerGen(); // Agregar servicios para generar Swagger

builder.Services.AddAutoMapper(typeof(AutoMapping).Assembly); //Registrar AutoMapper y escanear el ensamblado donde se encuentra AutoMapping

builder.Services.AddDbContext<ProConnectDbContext>(options =>
{
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure()
    );
});

/* +---------------------------------------------------------------------------------------------------------+
   |                                    Configuración de JWT Authentication                                  |
   +---------------------------------------------------------------------------------------------------------+*/
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey no configurada");

builder.Services.AddAuthentication(options =>
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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ProfessionalOnly", policy => policy.RequireRole("Professional"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
    options.AddPolicy("AdminOrProfessional", policy => policy.RequireRole("Admin", "Professional"));
});



/* +---------------------------------------------------------------------------------------------------------+
   |                          Registro de UnitOfWork, Repositorios y Servicios                               |
   +---------------------------------------------------------------------------------------------------------+*/
// Agregar registro de UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // Registrar UnitOfWork para inyección de dependencias

// Agregar registros de repositorios
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
/*builder.Services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();
builder.Services.AddScoped<IConsultationRepository, ConsultationRepository>();
builder.Services.AddScoped<IDistributionRepository, DistributionRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IProfessionalPaymentInfoRepository, ProfessionalPaymentInfoRepository>();
builder.Services.AddScoped<IProfessionalProfileRepository, ProfessionalProfileRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
builder.Services.AddScoped<IVerificationDocumentRepository, VerificationDocumentRepository>();
builder.Services.AddScoped<IVerificationRepository, VerificationRepository>();
builder.Services.AddScoped<IVideoCallRepository, VideoCallRepository>();*/

// Agregar registros de servicios de aplicación
builder.Services.AddScoped<IAuthService, AuthService>();

// Agregar registros de servicios de infraestructura
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

/*
 *builder.Services.AddScoped<IAvailabilityService, AvailabilityService>();
 *builder.Services.AddScoped<IConsultationService, ConsultationService>();
 *builder.Services.AddScoped<IDistributionService, DistributionService>();
 *builder.Services.AddScoped<INotificationService, NotificationService>();
 *builder.Services.AddScoped<IPaymentService, PaymentService>();
 *builder.Services.AddScoped<IProfessionalPaymentInfoService, ProfessionalPaymentInfoService>();
 *builder.Services.AddScoped<IProfessionalProfileService, ProfessionalProfileService>();
 *builder.Services.AddScoped<IReviewService, ReviewService>();
 *builder.Services.AddScoped<IRoleService, RoleService>();
 *builder.Services.AddScoped<ISpecialtyService, SpecialtyService>();
 *builder.Services.AddScoped<IUserService, UserService>();
 *builder.Services.AddScoped<IUserRoleService, UserRoleService>();
 *builder.Services.AddScoped<IVerificationDocumentService, VerificationDocumentService>();
 *builder.Services.AddScoped<IVerificationService, VerificationService>();
 *builder.Services.AddScoped<IVideoCallService, VideoCallService>();
*/



/* +---------------------------------------------------------------------------------------------------------+
   |                                        Construcción de la aplicación                                    |
   +---------------------------------------------------------------------------------------------------------+*/
var app = builder.Build(); // Construir la aplicación

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication(); // Middleware de autenticación
app.UseAuthorization(); // Middleware de autorización

app.MapControllers();

app.Run();