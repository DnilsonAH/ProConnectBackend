# Sistema de Autenticación JWT - ProConnect

## Descripción General

Se ha implementado un sistema completo de autenticación y autorización basado en JWT (JSON Web Tokens) siguiendo la arquitectura hexagonal del proyecto.

## Roles Implementados

El sistema maneja tres roles de usuario:
- **User**: Rol por defecto asignado al registrarse
- **Professional**: Rol para profesionales de la plataforma
- **Admin**: Rol de administrador con permisos especiales

## Endpoints Implementados

### 1. Registro de Usuario
**POST** `/api/auth/register`

Crea una nueva cuenta con el rol "User" por defecto.

**Request Body:**
```json
{
  "name": "Juan Pérez",
  "email": "juan@example.com",
  "password": "Password123",
  "phoneNumber": "+51987654321"
}
```

**Response (200 OK):**
```json
{
  "userId": 1,
  "name": "Juan Pérez",
  "email": "juan@example.com",
  "roles": ["User"],
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2024-10-30T10:30:00Z"
}
```

### 2. Login
**POST** `/api/auth/login`

Inicia sesión y obtiene un token JWT.

**Request Body:**
```json
{
  "email": "juan@example.com",
  "password": "Password123"
}
```

**Response (200 OK):**
```json
{
  "userId": 1,
  "name": "Juan Pérez",
  "email": "juan@example.com",
  "roles": ["User"],
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2024-10-30T10:30:00Z"
}
```

### 3. Cambiar Rol de Usuario
**PUT** `/api/auth/change-role`

Cambia o agrega un rol a un usuario. Requiere autenticación.

**Headers:**
```
Authorization: Bearer {token}
```

**Request Body:**
```json
{
  "userId": 1,
  "roleName": "Professional"
}
```

**Response (200 OK):**
```json
{
  "message": "Rol 'Professional' asignado correctamente al usuario"
}
```

## Configuración JWT

La configuración JWT se encuentra en `appsettings.json`:

```json
{
  "JwtSettings": {
    "SecretKey": "ProConnect-SuperSecretKey-2024-MinLength32Characters!",
    "Issuer": "ProConnectAPI",
    "Audience": "ProConnectClient",
    "ExpirationHours": "24"
  }
}
```

### ⚠️ Importante en Producción
- **Cambiar** la `SecretKey` por una clave segura y única
- **Almacenar** la clave en variables de entorno o Azure Key Vault
- **No** subir las claves a repositorios públicos

## Uso del Token

Para usar endpoints protegidos, incluye el token en el header:

```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## Políticas de Autorización

Se han configurado las siguientes políticas:
- `AdminOnly`: Solo usuarios con rol Admin
- `ProfessionalOnly`: Solo usuarios con rol Professional  
- `UserOnly`: Solo usuarios con rol User
- `AdminOrProfessional`: Usuarios con rol Admin o Professional

### Ejemplo de uso en Controllers:

```csharp
[Authorize(Policy = "AdminOnly")]
[HttpGet("admin-data")]
public IActionResult GetAdminData()
{
    // Solo accesible por Admin
}

[Authorize(Roles = "Admin,Professional")]
[HttpGet("professional-data")]
public IActionResult GetProfessionalData()
{
    // Accesible por Admin o Professional
}
```

## Arquitectura Implementada

### Domain Layer
- **Interfaces de Puertos**: `IJwtService`, `IPasswordHasher`
- **Repositorios extendidos**: Métodos específicos para autenticación

### Application Layer
- **DTOs**: `RegisterRequestDto`, `LoginRequestDto`, `AuthResponseDto`, `ChangeRoleRequestDto`
- **Servicios**: `AuthService` (lógica de negocio de autenticación)
- **Interfaces**: `IAuthService`

### Infrastructure Layer
- **Implementaciones**:
  - `JwtService`: Generación de tokens JWT
  - `PasswordHasher`: Hash y verificación de contraseñas con BCrypt
- **Repositorios**: Implementación de métodos específicos

### API Layer
- **Controller**: `AuthController` con los 3 endpoints
- **Configuración**: JWT Bearer Authentication en `Program.cs`

## Paquetes NuGet Agregados

- `Microsoft.AspNetCore.Authentication.JwtBearer` (v9.0.0)
- `System.IdentityModel.Tokens.Jwt` (v8.2.1)
- `BCrypt.Net-Next` (v4.0.3)

## Inicialización de la Base de Datos

Antes de usar el sistema, ejecuta el script SQL:

```bash
mysql -h b9rtb4siikyh6jpsdi8m-mysql.services.clever-cloud.com -P 21439 -u ulf3n4vjarw5vp27 -p < InitializeRoles.sql
```

O ejecuta manualmente el contenido de `InitializeRoles.sql` en tu gestor de base de datos.

## Pruebas con Postman/Thunder Client

### 1. Registrar Usuario
```
POST http://localhost:5000/api/auth/register
Content-Type: application/json

{
  "name": "Test User",
  "email": "test@example.com",
  "password": "Test123456",
  "phoneNumber": "123456789"
}
```

### 2. Login
```
POST http://localhost:5000/api/auth/login
Content-Type: application/json

{
  "email": "test@example.com",
  "password": "Test123456"
}
```

### 3. Cambiar Rol (con token)
```
PUT http://localhost:5000/api/auth/change-role
Content-Type: application/json
Authorization: Bearer {TOKEN_AQUI}

{
  "userId": 1,
  "roleName": "Professional"
}
```

## Flujo de Autenticación

1. **Usuario se registra** → Recibe token con rol "User"
2. **Usuario hace login** → Recibe token con sus roles actuales
3. **Admin/Usuario cambia rol** → Se asigna nuevo rol
4. **Usuario hace login nuevamente** → Recibe token con roles actualizados
5. **Usuario accede a recursos protegidos** → Sistema verifica token y roles

## Seguridad

- **Contraseñas**: Hash con BCrypt (salt automático)
- **Tokens**: Firmados con HMAC-SHA256
- **Expiración**: 24 horas (configurable)
- **Validación**: Issuer, Audience, Lifetime y SigningKey

## Próximos Pasos Recomendados

1. Implementar refresh tokens para renovación automática
2. Agregar rate limiting para prevenir ataques de fuerza bruta
3. Implementar recuperación de contraseña
4. Agregar logs de auditoría para cambios de roles
5. Implementar verificación de email en el registro
6. Agregar políticas de contraseña más estrictas

## Soporte

Para cualquier duda o problema, revisar la documentación de cada capa en los archivos `.md` correspondientes.
