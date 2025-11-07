# ProConnect_Backend.Application (Capa de Aplicación)

Descripción

Esta capa contiene los casos de uso de la aplicación, DTOs, mappings y la lógica que coordina operaciones entre el dominio y la infraestructura. Es la capa que implementa las reglas de negocio específicas y prepara/transforma datos para la API.

Responsabilidades principales

- Implementar casos de uso y servicios de aplicación.
- Definir DTOs (en `DTOs/`) y mapeos entre entidades y DTOs.
- Proveer interfaces que la capa API consume (por ejemplo, servicios que realizan operaciones de usuario, disponibilidad, pagos, etc.).
- Orquestar transacciones a través de `IUnitOfWork` cuando corresponda.

Archivos y carpetas clave

- `DTOsResponse/` — clases de transferencia de datos para respuestas (ej: `SpecialityDtos/`).
- `UseCases/` — casos de uso organizados por módulo (ej: `Specialty/`).
- `Mapping/AutoMapping.cs` — configuración de AutoMapper para mapeos entre entidades y DTOs.
- `Configuration/ApplicationServicesExtensions.cs` — registro de servicios de aplicación (AutoMapper, MediatR).
- `ProConnect_Backend.Application.csproj` — proyecto de la capa.

Cómo probar y ejecutar

- Esta es una librería de clases; se ejecuta indirectamente al levantar la API.
- Para compilar:

  dotnet build ProConnect_Backend.Application\ProConnect_Backend.Application.csproj

Integración

- Consumida por `ProConnect_Backend` (API).
- Utiliza contratos del dominio (entidades) y persistencia a través de interfaces definidas en `Infrastructure` (repositorios/UnitOfWork).

Buenas prácticas

- Mantener DTOs y mapeos centralizados.
- No incluir lógica de acceso a datos aquí; usar repositorios/UnitOfWork de `Infrastructure`.

