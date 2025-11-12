# 🎯 Application Layer (Capa de Aplicación)

## 📋 Descripción General

La **capa de aplicación** orquesta la lógica de negocio y coordina el flujo de datos entre capas. Esta capa:

- ✅ **Depende SOLO del Domain** (no de Infrastructure)
- ✅ Implementa **casos de uso** (use cases) del sistema
- ✅ Orquesta operaciones usando **repositorios y servicios**
- ✅ Transforma **entidades → DTOs** de respuesta
- ✅ Usa **MediatR** para implementar patrón CQRS

---

## 📁 Estructura de Carpetas

### 🗂️ **`UseCases/`**
**Propósito**: Contiene los casos de uso organizados por módulo

**Estructura actual**:
```
UseCases/
├── Auth/
│   ├── Login/
│   │   └── Command/
│   │       ├── LoginCommand.cs
│   │       └── LoginCommandHandler.cs
│   ├── Register/
│   │   └── Command/
│   │       ├── RegisterCommand.cs
│   │       └── RegisterCommandHandler.cs
│   └── Logout/
│       └── Command/
│           ├── LogoutCommand.cs
│           └── LogoutCommandHandler.cs
└── User/
    └── Query/
        ├── GetUserByIdQuery.cs
        └── GetUserByIdQueryHandler.cs
```

**Patrones**:
- **Command**: Operaciones que modifican estado (Register, Login, Logout)
- **Query**: Operaciones de solo lectura (GetUserById)

---

#### **Commands (Comandos)**
**Propósito**: Representan acciones que modifican el estado del sistema

**Ejemplo**: `LoginCommand.cs`
```csharp
public record LoginCommand(LoginRequestDto LoginDto) : IRequest<LoginResponseDto?>;
```

**Características**:
- ✅ Inmutables (`record`)
- ✅ Implementan `IRequest<TResponse>` de MediatR
- ✅ Encapsulan los datos necesarios para la operación
- ✅ No contienen lógica, solo datos

---

#### **Handlers (Manejadores)**
**Propósito**: Implementan la lógica de negocio de los Commands/Queries

**Ejemplo**: `LoginCommandHandler.cs`
```csharp
public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public async Task<LoginResponseDto?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // 1. Buscar usuario
        var user = await _unitOfWork.UserRepository.GetByEmailAsync(request.LoginDto.Email);
        
        // 2. Validar contraseña
        var isValid = _passwordHasher.VerifyPassword(request.LoginDto.Password, user.PasswordHash);
        
        // 3. Generar token
        var token = _jwtTokenService.GenerateToken(user.UserId, user.Email, user.Role);
        
        // 4. Retornar DTO
        return new LoginResponseDto { Id = user.UserId, Token = token, ... };
    }
}
```

**Características**:
- ✅ Implementan `IRequestHandler<TCommand, TResponse>`
- ✅ Usan **inyección de dependencias**
- ✅ Solo dependen de **interfaces del Domain** (IUnitOfWork, IPasswordHasher)
- ✅ Transforman entidades → DTOs
- ✅ No conocen detalles de infraestructura (EF Core, BCrypt, etc.)

**Responsabilidades**:
1. **Validar reglas de negocio**
2. **Coordinar servicios y repositorios**
3. **Orquestar el flujo de la operación**
4. **Mapear resultados a DTOs**

---

### 🗂️ **`DTOsResponse/`**
**Propósito**: Define los contratos de salida (Response DTOs)

**Contenido actual**:
```
DTOsResponse/
├── AuthDTOs/
│   ├── LoginResponseDTO.cs
│   └── RegisterResponseDTO.cs
└── UserDTOs/
    └── GetUserInfoResponseDTO.cs
```

**Características**:
- ✅ Define **QUÉ datos se devuelven** al cliente
- ✅ No expone entidades directamente
- ✅ Protege información sensible (ej: no devuelve PasswordHash)
- ✅ Puede combinar datos de múltiples entidades

**Ejemplo**: `LoginResponseDTO.cs`
```csharp
public class LoginResponseDto
{
    public uint Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
```

---

### 🗂️ **`Mapping/`**
**Propósito**: Configura mapeos entre entidades y DTOs usando AutoMapper

**Contenido actual**:
- `AutoMapping.cs` - Profile de AutoMapper

**Características**:
- ✅ Centraliza las transformaciones Entidad ↔ DTO
- ✅ Reduce código repetitivo
- ✅ Maneja propiedades con nombres diferentes

**Ejemplo**: `AutoMapping.cs`
```csharp
public class AutoMapping : Profile
{
    public AutoMapping()
    {
        // User → LoginResponseDto
        CreateMap<User, LoginResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.Token, opt => opt.Ignore());

        // RegisterRequestDto → User
        CreateMap<RegisterRequestDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Se asigna en el handler
            .ForMember(dest => dest.Role, opt => opt.Ignore()); // Se asigna "Client" por defecto
    }
}
```

**Mapeos configurados**:
- `RegisterRequestDto` → `User` (para crear usuario)
- `User` → `LoginResponseDto` (respuesta de login)
- `User` → `RegisterResponseDto` (respuesta de registro)
- `User` → `GetUserInfoResponseDto` (información de usuario)

---

### 🗂️ **`Configuration/`**
**Propósito**: Configuración de servicios de Application Layer

**Contenido actual**:
- `ApplicationServicesExtensions.cs` - Registro de servicios

**Características**:
- ✅ Registra AutoMapper
- ✅ Registra MediatR
- ✅ Configura servicios de aplicación

**Ejemplo**:
```csharp
public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapping));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        return services;
    }
}
```

---

## 🎯 Principios de la Capa de Aplicación

### ✅ **Independencia de Infrastructure**
- No conoce EF Core, SQL, BCrypt, JWT
- Solo usa interfaces del Domain
- Permite cambiar tecnologías sin afectar lógica

### ✅ **Patrón CQRS (Command Query Responsibility Segregation)**
- **Commands**: Modifican estado (POST, PUT, DELETE)
- **Queries**: Solo lectura (GET)
- Separación clara de responsabilidades

### ✅ **Orquestación, no Implementación**
```csharp
// ✅ CORRECTO: Application orquesta usando interfaces
var user = await _unitOfWork.UserRepository.GetByEmailAsync(email);
var isValid = _passwordHasher.VerifyPassword(password, user.PasswordHash);

// ❌ INCORRECTO: Application no debe tener lógica de infraestructura
var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
var isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
```

### ✅ **DTOs en vez de Entidades**
```csharp
// ✅ CORRECTO: Retorna DTO
return new LoginResponseDto { Id = user.UserId, Name = user.FirstName, ... };

// ❌ INCORRECTO: Retorna entidad directamente
return user;
```

---

## 🔄 Flujo de Ejecución

```
Controller (API Layer)
    ↓
    ↓ Crea Command/Query
    ↓
Handler (Application Layer)
    ↓
    ├→ IUnitOfWork.Repository (Domain Interface)
    │       ↓
    │       └→ UserRepository (Infrastructure Implementation)
    │
    ├→ IPasswordHasher (Domain Interface)
    │       ↓
    │       └→ PasswordHasher (Infrastructure Implementation)
    │
    └→ IJwtTokenService (Domain Interface)
            ↓
            └→ JwtTokenService (Infrastructure Implementation)
    ↓
    ↓ Mapea Entidad → DTO
    ↓
Retorna DTO al Controller
```

---

## 🔄 Relación con Otras Capas

```
┌─────────────────────────────────────┐
│         DOMAIN LAYER                │
│   (Interfaces + Entidades)          │
└─────────────────────────────────────┘
          ↑
          │ depende de
          │
┌─────────────────────────────────────┐
│      APPLICATION LAYER              │
│                                     │
│   - UseCases (Commands/Queries)     │
│   - Handlers (lógica orquestación)  │
│   - DTOs Response                   │
│   - AutoMapper                      │
└─────────────────────────────────────┘
          ↑
          │ usan
          │
┌─────────────────────────────────────┐
│         API LAYER                   │
│   (Controllers)                     │
└─────────────────────────────────────┘
```

---

## 📦 Dependencias

**Paquetes NuGet**:
- `AutoMapper` - Mapeo automático de objetos
- `AutoMapper.Extensions.Microsoft.DependencyInjection` - Integración con DI
- `MediatR` - Patrón Mediator para CQRS
- `ProConnect_Backend.Domain` (referencia de proyecto)

---

## 🚀 Buenas Prácticas

1. ✅ **Un Handler por caso de uso** (Single Responsibility)
2. ✅ **Solo depender de interfaces del Domain**
3. ✅ **Usar DTOs para entrada/salida**
4. ✅ **No exponer entidades directamente**
5. ✅ **Handlers deben ser delgados** (orquestar, no implementar)
6. ✅ **Usar AutoMapper para transformaciones**
7. ✅ **Validaciones en DTOs (Domain), lógica en Handlers**
8. ✅ **Organizar por módulo/funcionalidad**

---

## 🎓 Patrones Implementados

- ✅ **CQRS** (Command Query Responsibility Segregation)
- ✅ **Mediator Pattern** (vía MediatR)
- ✅ **DTO Pattern** (Data Transfer Object)
- ✅ **Dependency Injection**
- ✅ **Repository Pattern** (interfaces del Domain)

---

**Última actualización**: Noviembre 2025  
**Dependencias**: Domain Layer + AutoMapper + MediatR
