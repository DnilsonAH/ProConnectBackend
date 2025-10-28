# ProConnect_Backend.Infrastructure (Infraestructura)

Descripción

Implementa los detalles técnicos y la persistencia: adaptadores, repositorios, acceso a base de datos (DbContext), implementaciones del `UnitOfWork` y otras integraciones externas.

Responsabilidades principales

- Implementar `IUnitOfWork` y los repositorios definidos en el dominio.
- Proveer `DbContext` y mapeos EF Core (archivo: `Data/ProConnectDbContext.cs`).
- Contener adaptadores y utilidades para integración con servicios externos (pagos, notificaciones, etc.).
- Registrar implementaciones concretas para inyección de dependencias que la API consumirá.

Archivos y carpetas clave

- `Adapters/UnitOfWork.cs` — implementación del Unit of Work.
- `Adapters/Repositories/` — implementaciones concretas de repositorios.
- `Data/ProConnectDbContext.cs` — contexto de EF Core y configuración de entidades.
- `ProConnect_Backend.Infrastructure.csproj` — proyecto de infraestructura.

Cómo compilar

  dotnet build ProConnect_Backend.Infrastructure\ProConnect_Backend.Infrastructure.csproj

Notas

- Revisar cadenas de conexión en `appsettings` cuando se implementen migraciones.
- Añadir migraciones y aplicar a la base de datos usando EF Core CLI si procede:

  dotnet ef migrations add NombreMigracion --project ProConnect_Backend.Infrastructure --startup-project ProConnect_Backend
  dotnet ef database update --project ProConnect_Backend.Infrastructure --startup-project ProConnect_Backend

- Mantener aquí sólo detalles específicos de la tecnología y no lógica de negocio.

