using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Core.Mapping;
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
   |                               Registro de Repositorios y Servicios                                      |
   +---------------------------------------------------------------------------------------------------------+*/
// Agregar registros de repositorios | Emjemplo: builder.Services.AddScoped<IUserRepository, UserRepository>();

// Agregar registros de servicios | Ejemplo: builder.Services.AddScoped<IUserService, UserService>();




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