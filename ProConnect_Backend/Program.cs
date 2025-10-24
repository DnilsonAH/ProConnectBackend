using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Core.Mapping;
using ProConnect_Backend.Core.Repositories;
using ProConnect_Backend.Core.Repositories.Interfaces;
using ProConnect_Backend.Infrastructure.Data;

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
   |                          Registro de UnitOfWork, Repositorios y Servicios                               |
   +---------------------------------------------------------------------------------------------------------+*/
// Agregar registro de UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // Registrar UnitOfWork para inyección de dependencias

// Agregar registros de repositorios | Emjemplo: builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();
builder.Services.AddScoped<IConsultationRepository, ConsultationRepository>();
builder.Services.AddScoped<IDistributionRepository, DistributionRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IProfessionalPaymentInfoRepository, ProfessionalPaymentInfoRepository>();
builder.Services.AddScoped<IProfessionalProfileRepository, ProfessionalProfileRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<IVerificationDocumentRepository, VerificationDocumentRepository>();
builder.Services.AddScoped<IVerificationRepository, VerificationRepository>();
builder.Services.AddScoped<IVideoCallRepository, VideoCallRepository>();

// Agregar registros de servicios | Ejemplo: builder.Services.AddScoped<IUserService, UserService>();
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
app.MapControllers();

app.Run();