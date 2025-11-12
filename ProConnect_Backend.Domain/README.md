# 🏛️ Domain Layer (Capa de Dominio)

## 📋 Descripción General

La **capa de dominio** es el corazón del sistema y contiene la lógica de negocio central. Esta capa:

- ✅ **NO tiene dependencias externas** (0 paquetes NuGet externos)
- ✅ Define las **entidades** del negocio
- ✅ Define los **contratos (interfaces)** que otras capas deben implementar
- ✅ Contiene las **reglas de negocio fundamentales**
- ✅ Es **independiente de frameworks** (EF Core, ASP.NET, etc.)

---

## 📁 Estructura de Carpetas

### 🗂️ **`Entities/`**
**Propósito**: Contiene las entidades del dominio (modelos de datos puros)

**Contenido actual**: 13 entidades
- `User.cs` - Usuario del sistema (Client, Professional, Admin)
- `Session.cs` - Sesiones entre clientes y profesionales
- `Payment.cs` - Pagos de sesiones
- `Review.cs` - Reseñas y calificaciones
- `JwtBlacklist.cs` - Tokens JWT revocados (logout)
- `ProfessionalProfile.cs` - Perfiles de profesionales
- `Profession.cs` - Profesiones disponibles
- `ProfessionCategory.cs` - Categorías de profesiones
- `Specialization.cs` - Especializaciones por profesión
- `Verification.cs` - Verificación de profesionales
- `VerificationDocument.cs` - Documentos de verificación
- `WeeklyAvailability.cs` - Disponibilidad semanal de profesionales
- `Scheduled.cs` - Citas programadas

**Características**:
- ❌ **SIN anotaciones de EF Core** (como `[Key]`, `[Column]`, etc.)
- ✅ **POCOs puros** (Plain Old CLR Objects)
- ✅ Solo propiedades y navigation properties
- ✅ Representan conceptos del negocio

**Ejemplo**:
```csharp
public partial class User
{
    public uint UserId { get; set; }
    public string FirstName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    // Navigation properties
    public virtual ICollection<Session> SessionClients { get; set; }
}
```

---

### 🗂️ **`Ports/IRepositories/`**
**Propósito**: Define los contratos (interfaces) para acceso a datos

**Contenido actual**: 14 interfaces
- `IGenericRepository<TEntity>` - Repositorio base genérico
- `IUserRepository` - Operaciones específicas de usuarios
- `IJwtBlacklistRepository` - Manejo de tokens revocados
- `ISessionRepository` - Gestión de sesiones
- `IPaymentRepository` - Operaciones de pagos
- `IReviewRepository` - Reseñas y calificaciones
- ... (9 repositorios más)

**Características**:
- ✅ Define **QUÉ** operaciones se necesitan, no **CÓMO** se implementan
- ✅ Permite cambiar la tecnología de persistencia sin afectar la lógica de negocio
- ✅ Facilita testing con mocks/stubs

**Ejemplo**:
```csharp
public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ExistsByEmailAsync(string email);
    Task<IEnumerable<User>> GetUsersByRoleAsync(string role);
}
```

---

### 🗂️ **`Ports/IServices/`**
**Propósito**: Define contratos para servicios de infraestructura

**Contenido actual**: 2 interfaces
- `IPasswordHasher` - Contrato para hasheo de contraseñas
- `IJwtTokenService` - Contrato para generación y validación de JWT

**Características**:
- ✅ Abstrae servicios técnicos (seguridad, tokens, etc.)
- ✅ El Domain define **QUÉ necesita**, Infrastructure define **CÓMO lo hace**
- ✅ Permite cambiar implementaciones (BCrypt → Argon2, JWT → OAuth)

**Ejemplo**:
```csharp
public interface IJwtTokenService
{
    string GenerateToken(uint userId, string email, string role);
    bool ValidateToken(string token);
    (string jti, uint userId, DateTime expiresAt)? ParseToken(string token);
}
```

---

### 🗂️ **`Ports/IUnitOfWork.cs`**
**Propósito**: Define el contrato del patrón Unit of Work

**Características**:
- ✅ Agrupa todos los repositorios en una sola interfaz
- ✅ Gestiona transacciones (commit/rollback)
- ✅ Garantiza que múltiples operaciones se ejecuten en la misma transacción

**Ejemplo**:
```csharp
public interface IUnitOfWork : IDisposable
{
    IUserRepository UserRepository { get; }
    IJwtBlacklistRepository JwtBlacklistRepository { get; }
    ISessionRepository SessionRepository { get; }
    // ... 11 repositorios más
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
```

---

### 🗂️ **`DTOsRequest/`**
**Propósito**: Define los contratos de entrada (Request DTOs)

**Contenido actual**:
- `AuthDTOs/`
  - `LoginRequestDTO.cs` - Datos para login (Email, Password)
  - `RegisterRequestDTO.cs` - Datos para registro (FirstName, Email, etc.)
  - `LogoutRequestDTO.cs` - Datos para logout (Token)

**Características**:
- ✅ Incluye **validaciones** con DataAnnotations
- ✅ Define **QUÉ datos se necesitan** para cada operación
- ✅ Protege la API de recibir datos innecesarios
- ✅ No expone las entidades directamente

**Ejemplo**:
```csharp
public class LoginRequestDto
{
    [Required(ErrorMessage = "El correo electrónico es requerido")]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
    public string Password { get; set; } = string.Empty;
}
```

---

## 🎯 Principios de la Capa de Dominio

### ✅ **Independencia Total**
- No referencia otras capas
- No depende de frameworks externos
- Es el núcleo más estable del sistema

### ✅ **Pura Lógica de Negocio**
- Contiene las reglas fundamentales
- Define conceptos del negocio real
- Es comprensible por expertos del dominio

### ✅ **Contratos, no Implementaciones**
- Define interfaces (`IUserRepository`)
- Otras capas implementan estas interfaces
- Permite flexibilidad y testability

### ✅ **Validación en DTOs**
- Los DTOs de request validan entrada
- Protegen contra datos inválidos
- Centraliza reglas de validación

---

## 🔄 Relación con Otras Capas

```
┌─────────────────────────────────────┐
│         DOMAIN LAYER                │
│   (0 dependencias externas)         │
│                                     │
│   - Entities (User, Session, etc.)  │
│   - Interfaces (IUserRepository)    │
│   - DTOs Request (LoginRequestDTO)  │
└─────────────────────────────────────┘
          ↑                ↑
          │                │
    depende de       depende de
          │                │
┌─────────────────┐  ┌──────────────────┐
│  APPLICATION    │  │ INFRASTRUCTURE   │
│  (Handlers)     │  │ (Repositories)   │
└─────────────────┘  └──────────────────┘
```

---

## 📚 Referencias

- **Clean Architecture** by Robert C. Martin
- **Domain-Driven Design** by Eric Evans
- **Dependency Inversion Principle** (SOLID)

---

## 🚀 Buenas Prácticas

1. ✅ Mantener el Domain **sin dependencias externas**
2. ✅ Usar **interfaces** para todos los servicios externos
3. ✅ **No incluir lógica de infraestructura** (SQL, HTTP, etc.)
4. ✅ Entidades deben ser **POCOs puros**
5. ✅ DTOs con **validaciones explícitas**
6. ✅ Nombrar interfaces con prefijo `I` (IUserRepository)
7. ✅ Agrupar DTOs por módulo/funcionalidad

---

**Última actualización**: Noviembre 2025  
**Dependencias externas**: Ninguna (0 paquetes NuGet)
