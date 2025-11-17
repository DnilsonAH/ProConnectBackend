# 🎯 Application Layer

**Propósito**: Orquesta casos de uso mediante CQRS pattern con MediatR.

**Dependencias**: 
- ✅ Domain (interfaces y DTOs Request)
- ✅ AutoMapper (mapeo DTO ↔ Entity)
- ✅ MediatR (patrón CQRS - opcional actualmente)

---

## 📁 Inventario de Archivos

### **UseCases/Auth/** (6 archivos)
Login:
- `LoginCommand.cs` - Comando con LoginRequestDto
- `LoginCommandHandler.cs` - Valida credenciales, genera JWT, retorna LoginResponseDTO

Register:
- `RegisterCommand.cs` - Comando con RegisterRequestDto
- `RegisterCommandHandler.cs` - Valida unicidad email, hashea password, crea usuario con rol "Client"

Logout:
- `LogoutCommand.cs` - Comando con token
- `LogoutCommandHandler.cs` - Extrae JTI, agrega a blacklist

### **UseCases/User/** (2 archivos)
GetUserById:
- `GetUserByIdQuery.cs` - Query con userId
- `GetUserByIdQueryHandler.cs` - Busca usuario, retorna GetUserInfoResponseDTO

### **DTOsResponse/** (3 archivos)
Objetos de salida:
- `LoginResponseDTO` - UserId, Email, Token
- `RegisterResponseDTO` - UserId, Email, FirstName, etc.
- `GetUserInfoResponseDTO` - UserId, Email, FirstName, SecondName, FirstSurname, SecondSurname, Role, CreatedAt

### **Mapping/**
- `AutoMapping.cs` - Perfil con mapeos Entity → DTO Response (mapea FirstSurname→LastName, PhoneNumber→Phone)

### **Configuration/**
- `ApplicationServicesExtensions.cs` - Registra AutoMapper y handlers en DI

---

## 🎯 Patrón CQRS

**Commands** (modifican estado):
- `LoginCommand` → Genera token
- `RegisterCommand` → Crea usuario
- `LogoutCommand` → Revoca token

**Queries** (solo lectura):
- `GetUserByIdQuery` → Consulta usuario

**Handlers**:
- Inyectan `IUnitOfWork`, `IPasswordHasher`, `IJwtTokenService` (interfaces de Domain)
- NO conocen EF Core ni implementaciones concretas
- Usan AutoMapper para transformar entities → DTOs Response

---

## 🔗 Flujo de ejecución

```
API Controller → Command/Query → Handler → IUnitOfWork → Repository (Infrastructure)
                                         ↓
                                   AutoMapper → DTO Response
```

---

## 🔗 Usado por

- ✅ **API Layer** (inyecta y ejecuta Handlers)
