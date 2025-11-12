# 🌐 API Layer / Presentation Layer

**Propósito**: Punto de entrada HTTP, expone endpoints REST.

**Dependencias**:
- ✅ Application (inyecta Handlers)
- ✅ Infrastructure (solo para registro en DI)

**Puerto**: http://localhost:5200

---

## 📁 Inventario de Archivos

### **Controllers/** (2 archivos)

#### **AuthController.cs**
Autenticación:
- `POST /api/auth/login` ❌ Pública - Retorna JWT
- `POST /api/auth/register` ❌ Pública - Crea usuario con rol "Client"
- `POST /api/auth/logout` ✅ JWT Required - Revoca token

#### **UserController.cs**
Gestión de usuarios:
- `GET /api/user/me` ✅ JWT Required - Info usuario autenticado
- `GET /api/user/{id}` ❌ Pública - Info usuario por ID
- `PUT /api/user/{id}` ✅ JWT Required - Actualizar usuario (TODO)

### **Middleware/**
- `TokenValidationMiddleware.cs` - Valida JWT contra blacklist ANTES de AuthenticationMiddleware

### **Configuration/**
- `ServiceRegistrationExtensions.cs` - Registra en DI:
  - DbContext (MySQL con SSL)
  - 13 Repositories
  - UnitOfWork
  - 2 Services (PasswordHasher, JwtTokenService)
  - 4 Handlers (Login, Register, Logout, GetUserById)
  - JWT Authentication
  - AutoMapper

### **Otros**
- `Program.cs` - Pipeline: Auth → TokenValidation → Authorization → Controllers
- `appsettings.json` - Configuración (actualmente usa .env)
- `Properties/launchSettings.json` - Configuración de launch (puerto 5200)

---

## 🔗 Endpoints Resumen

| Método | Ruta | Auth | Descripción |
|--------|------|------|-------------|
| POST | `/api/auth/login` | ❌ | Login (retorna JWT) |
| POST | `/api/auth/register` | ❌ | Registro |
| POST | `/api/auth/logout` | ✅ | Logout (revoca token) |
| GET | `/api/user/me` | ✅ | Usuario autenticado |
| GET | `/api/user/{id}` | ❌ | Usuario por ID |
| PUT | `/api/user/{id}` | ✅ | Actualizar usuario (TODO) |

---

## 🎯 Formato de Respuesta Estándar

**Success**:
```json
{
  "success": true,
  "message": "🎉 Mensaje descriptivo",
  "data": { ... }
}
```

**Error**:
```json
{
  "success": false,
  "message": "❌ Descripción del error"
}
```

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
