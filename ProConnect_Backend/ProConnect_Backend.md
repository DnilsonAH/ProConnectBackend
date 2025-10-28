# ProConnect_Backend (API)

Descripción

Este proyecto contiene la capa de presentación / API de ProConnect. Expone los endpoints HTTP que consumen los clientes (web, móvil) y orquesta las llamadas hacia la capa de aplicación (Application) para cumplir con los casos de uso.

Responsabilidades principales

- Definir controladores y rutas (endpoints REST). 
- Validación de entrada básica y manejo de errores HTTP.
- Configuración de middleware (autenticación, autorización, CORS, logging, excepciones).
- Cargar la configuración (appsettings.json, launchSettings) y enlazar servicios en Program.cs.
- Orquestar llamadas a la capa Application y devolver respuestas formateadas.

Archivos y carpetas clave

- `Program.cs` — punto de arranque; registra servicios y middleware.
- `appsettings.json`, `appsettings.Development.json` — configuración de entornos.
- `Api/Controllers/` — controladores, por ejemplo `UserController.cs`.
- `ProConnect_Backend.csproj` — definición del proyecto.

Cómo ejecutar (local)

1. Desde la raíz del repo o la carpeta de la solución, restaurar dependencias y construir:

   dotnet restore
   dotnet build

2. Ejecutar la API (desde `ProConnect_Backend`):

   dotnet run --project .\ProConnect_Backend.csproj

(Opción: abrir `ProConnect_Backend.http` si está presente para llamadas de ejemplo.)

Puntos de integración

- Depende/invoca la capa `ProConnect_Backend.Application` para la lógica de negocio.
- Consume servicios/infrastructure a través de interfaces registradas en DI (UnitOfWork, Repositories).

Notas y recomendaciones

- Mantener los controladores ligeros; la lógica de negocio debe residir en `Application`.
- Añadir documentación OpenAPI/Swagger si no está presente para facilitar uso por frontend.

