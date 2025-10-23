using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Core.Mapping;
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
   |                               Registro de UnitOfWork y Servicios                                        |
   +---------------------------------------------------------------------------------------------------------+*/
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // Registrar UnitOfWork para inyección de dependencias
// Agregar registros de repositorios | Esto en caso de ser necesario pero el unitofwork ya los maneja todos los repositorios


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