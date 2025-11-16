# 🌐 API Layer / Presentation Layer

**Propósito**: Punto de entrada HTTP, expone endpoints REST.

**Dependencias**:
- ✅ Application (inyecta Handlers)
- ✅ Infrastructure (solo para registro en DI)

**Puerto**: http://localhost:5200

---

## 📁 Inventario de Archivos

### **Controllers/** (6 archivos)

#### **AuthController.cs**
Autenticación (Login, Register, Logout)

#### **UserController.cs**
Gestión de usuarios (GetCurrentUser, GetById, Update)

#### **ProfessionCategoryController.cs**
CRUD de categorías de profesión (Create, GetAll, GetById, Update, Delete)

#### **ProfessionController.cs**
CRUD de profesiones (Create, GetAll, GetById, GetByCategory, Update, Delete)

#### **SpecializationController.cs**
CRUD de especializaciones (Create, GetAll, GetById, GetByProfession, Update, Delete)

#### **ProfileSpecializationController.cs**
Asignación de especializaciones a perfiles (Assign, Remove, GetByProfile, GetBySpecialization)

### **Middleware/**
- `TokenValidationMiddleware.cs` - Valida JWT contra blacklist ANTES de AuthenticationMiddleware

### **Configuration/**
- `ServiceRegistrationExtensions.cs` - Registra en DI:
  - DbContext (MySQL con SSL)
  - 14 Repositories (User, JwtBlacklist, Session, Payment, Review, ProfessionalProfile, Profession, ProfessionCategory, Specialization, ProfileSpecialization, Verification, VerificationDocument, WeeklyAvailability, Scheduled)
  - UnitOfWork
  - 2 Services (PasswordHasher, JwtTokenService)
  - Handlers (MediatR auto-registra todos los handlers)
  - JWT Authentication
  - AutoMapper

### **API_Documentation/**
- `README.md` - Índice general de endpoints
- `ProfessionCategory.md` - Documentación del controlador
- `Profession.md` - Documentación del controlador
- `Specialization.md` - Documentación del controlador
- `ProfileSpecialization.md` - Documentación del controlador

### **Otros**
- `Program.cs` - Pipeline: Auth → TokenValidation → Authorization → Controllers
- `appsettings.json` - Configuración (actualmente usa .env)
- `Properties/launchSettings.json` - Configuración de launch (puerto 5200)

---

## 🔒 Seguridad

1. **JWT Bearer Authentication**: Claims (UserId, Email, Role, Jti)
2. **Token Blacklist**: Middleware valida revocación
3. **Authorization Policies**: 4 políticas por rol (Client, Professional, Admin, ClientOrProfessional)
4. **BCrypt Password Hashing**: WorkFactor 12
5. **SSL Connection**: Google Cloud SQL con certificados opcionales

---

## 🛠️ Middleware Pipeline

```
Request
  ↓
Authentication (JWT validation)
  ↓
TokenValidationMiddleware (blacklist check)
  ↓
Authorization (roles)
  ↓
Controllers
```

---

## 🔗 Swagger

Documentación disponible en: http://localhost:5200/swagger
