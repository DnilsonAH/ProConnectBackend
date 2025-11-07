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

- `Program.cs` — punto de arranque; registra servicios, middleware y configuración de base de datos MySQL con SSL.
- `appsettings.json`, `appsettings.Development.json` — configuración de entornos (usa variables de entorno).
- `Controllers/` — controladores REST (actualmente: `UserController.cs`, `SpecialtyController.cs`, `ExampleProtectedController.cs`).
- `Configuration/ServiceRegistrationExtensions.cs` — registro de servicios de DI, autenticación JWT y configuración de DbContext.
- `ProConnect_Backend.csproj` — definición del proyecto.

Cómo ejecutar (local)

1. **Configurar variables de entorno**: Crear archivo `.env` en la raíz con:
   ```
   DB_SERVER=tu_servidor
   DB_PORT=3306
   DB_DATABASE=proconnect_db
   DB_USER=tu_usuario
   DB_PASSWORD=tu_password
   JWT_SECRET_KEY=tu_clave_secreta
   JWT_ISSUER=ProConnect
   JWT_AUDIENCE=ProConnect
   JWT_EXPIRATION_HOURS=24
   ```

2. Restaurar dependencias y construir:
   ```
   dotnet restore
   dotnet build
   ```

3. Ejecutar la API:
   ```
   dotnet run --project .\ProConnect_Backend.csproj
   ```

4. Acceder a Swagger UI en: `https://localhost:7000/swagger` (puerto puede variar)

Puntos de integración

- Depende/invoca la capa `ProConnect_Backend.Application` para la lógica de negocio.
- Consume servicios/infrastructure a través de interfaces registradas en DI (UnitOfWork, Repositories).

Notas y recomendaciones

- Mantener los controladores ligeros; la lógica de negocio debe residir en `Application`.
- Añadir documentación OpenAPI/Swagger si no está presente para facilitar uso por frontend.

