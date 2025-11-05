using Microsoft.OpenApi.Models;
using ProConnect_Backend.Application.Configuration;
using ProConnect_Backend.Application.Mapping;
using ProConnect_Backend.Configuration;
var builder = WebApplication.CreateBuilder(args);


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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication(); // Middleware de autenticación
app.UseAuthorization(); // Middleware de autorización

app.MapControllers();

app.Run();