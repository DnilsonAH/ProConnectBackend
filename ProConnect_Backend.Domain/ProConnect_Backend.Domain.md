# ProConnect_Backend.Domain (Dominio)

Descripción

Contiene las entidades del dominio, objetos de valor y las interfaces que modelan el corazón del negocio. Representa los conceptos fundamentales: Usuario, Perfil Profesional, Citas, Pagos, Verificaciones, Roles, etc.

Responsabilidades principales

- Definir entidades y sus propiedades (reglas de invariantes simples dentro de las entidades cuando aplique).
- Definir interfaces (puertas/contratos) que otras capas implementarán o usarán (por ejemplo `IUnitOfWork`, contratos de repositorios en `Ports/IRepositories`).
- Mantener el modelo del dominio agnóstico a frameworks y detalles de infraestructura.

Archivos y carpetas clave

- `Entities/` — definiciones de entidad (ej.: `User.cs`, `ProfessionalProfile.cs`, `Consultation.cs`, `Payment.cs`, `Verification.cs`, `Availability.cs`, `Review.cs`, `VideoCall.cs`).
- `Ports/IUnitOfWork.cs` — contrato del Unit of Work.
- `Ports/IRepositories/` — interfaces de repositorios.
- `ProConnect_Backend.Domain.csproj` — proyecto del dominio.

Cómo compilar

  dotnet build ProConnect_Backend.Domain\ProConnect_Backend.Domain.csproj

Buenas prácticas

- Mantener las entidades libres de dependencias de frameworks (POCOs).
- Colocar lógica de negocio compleja en el dominio cuando sea necesario (agregados, invariantes), no en la infraestructura.

