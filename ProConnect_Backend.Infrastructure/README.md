# ⚙️ Infrastructure Layer

**Propósito**: Implementa interfaces de Domain con tecnologías concretas.

**Dependencias**:
- ✅ Domain (implementa interfaces)
- ✅ EF Core 9.0.10 (ORM)
- ✅ Pomelo.EntityFrameworkCore.MySql 9.0.0 (provider MySQL)
- ✅ BCrypt.Net-Next 4.0.3 (hashing passwords)
- ✅ System.IdentityModel.Tokens.Jwt 8.2.1 (JWT)

---

## 📁 Inventario de Archivos

### **Data/**
- `ProConnectDbContext.cs` - DbContext con 13 DbSets + SSL configuration

### **Data/Configurations/** (13 archivos)
Fluent API configurations (mantiene Domain puro):
- `UserConfiguration.cs`
- `SessionConfiguration.cs`
- `PaymentConfiguration.cs`
- `ReviewConfiguration.cs`
- `JwtBlacklistConfiguration.cs`
- `ProfessionalProfileConfiguration.cs`
- `ProfessionConfiguration.cs`
- `ProfessionCategoryConfiguration.cs`
- `SpecializationConfiguration.cs`
- `VerificationConfiguration.cs`
- `VerificationDocumentConfiguration.cs`
- `WeeklyAvailabilityConfiguration.cs`
- `ScheduledConfiguration.cs`

### **Adapters/Repositories/** (14 archivos)
Implementaciones de `IRepositories`:
- `GenericRepository<T>` - CRUD base (GetAll, GetById, Add, Update, Delete, SaveChanges)
- `UserRepository` - Implementa `IUserRepository` + `GetByEmailAsync`
- `JwtBlacklistRepository` - Implementa `IJwtBlacklistRepository` + `IsTokenRevokedAsync`
- 11 repositorios específicos más

### **Adapters/**
- `UnitOfWork.cs` - Implementa `IUnitOfWork` con 13 repositorios + transacciones

### **Services/** (2 archivos)
Implementaciones de `IServices`:
- `PasswordHasher.cs` - Implementa `IPasswordHasher` usando BCrypt
- `JwtTokenService.cs` - Implementa `IJwtTokenService` (genera, valida, parsea JWT con claims: UserId, Email, Role, Jti)

---

## 🔧 Configuración SSL

**ProConnectDbContext** detecta certificados en carpeta `ssl-certs/`:
- Con certificados client → SSL con autenticación mutua (desarrollo)
- Sin certificados → SSL-only mode (producción)

---

## 🔗 Usado por

- ✅ **Application Layer** (usa implementaciones vía DI)
- ✅ **API Layer** (registra servicios en DI)
