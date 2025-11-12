# 🏛️ Domain Layer

**Propósito**: Núcleo de la aplicación con lógica de negocio pura y contratos.

**Dependencias**: ✅ NINGUNA (0 referencias externas)

---

## 📁 Inventario de Archivos

### **Entities/** (13 archivos)
POCOs sin anotaciones de EF Core:
- `User.cs` - Usuarios del sistema
- `Session.cs` - Sesiones activas
- `Payment.cs` - Pagos
- `Review.cs` - Reseñas/calificaciones
- `JwtBlacklist.cs` - Tokens revocados
- `ProfessionalProfile.cs` - Perfiles profesionales
- `Profession.cs` - Profesiones
- `ProfessionCategory.cs` - Categorías
- `Specialization.cs` - Especializaciones
- `Verification.cs` - Verificaciones
- `VerificationDocument.cs` - Documentos de verificación
- `WeeklyAvailability.cs` - Disponibilidad semanal
- `Scheduled.cs` - Citas agendadas

### **Ports/IRepositories/** (14 interfaces)
Contratos para persistencia:
- `IGenericRepository<T>` - Operaciones CRUD genéricas
- `IUserRepository` - Usuario específico
- `IJwtBlacklistRepository` - Blacklist + método `IsTokenRevokedAsync`
- 11 repositorios específicos más

### **Ports/IServices/** (2 interfaces)
Contratos para servicios externos:
- `IPasswordHasher` - Hash y verificación de contraseñas
- `IJwtTokenService` - Generación, validación y parsing de JWT

### **Ports/**
- `IUnitOfWork.cs` - Transacciones + acceso a todos los repositorios

### **DTOsRequest/** (3 DTOs)
Objetos de entrada con validaciones:
- `LoginRequestDto` - Email + Password (Required)
- `RegisterRequestDto` - FirstName, SecondName, FirstSurname, SecondSurname, Email, Password, PhoneNumber (Required)
- `LogoutRequestDto` - Token (Required)

---

## 🎯 Principios

1. **Independencia total**: Domain no conoce EF Core, ASP.NET, ni tecnologías externas
2. **Contratos, no implementaciones**: Solo interfaces (Ports)
3. **Entidades puras**: Sin `[Key]`, `[Column]`, `[ForeignKey]`
4. **Flujo de dependencias**: Todas las capas dependen de Domain, nunca al revés

---

## 🔗 Usado por

- ✅ **Application** (depende de Domain)
- ✅ **Infrastructure** (implementa interfaces de Domain)
- ✅ **API** (usa DTOs de Domain vía Application)
