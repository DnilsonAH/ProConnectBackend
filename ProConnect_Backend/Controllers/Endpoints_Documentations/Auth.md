# Auth API

## 🔐 POST /api/auth/login
Autentica un usuario y genera un token JWT

**Request Body:**
```json
{
  "email": "string",        // Requerido. Formato email válido
  "password": "string"      // Requerido
}
```

**Response (200):**
```json
{
  "success": true,
  "message": "🎉 Inicio de sesión exitoso. ¡Bienvenido/a de nuevo!",
  "data": {
    "id": 1,
    "name": "Juan",
    "email": "juan@example.com",
    "role": "Client",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
  }
}
```

---

## 📝 POST /api/auth/register
Registra un nuevo usuario en el sistema

**Request Body:**
```json
{
  "firstName": "string",        // Requerido
  "firstSurname": "string",     // Requerido
  "secondSurname": "string",    // Opcional
  "email": "string",            // Requerido. Formato email válido
  "password": "string",         // Requerido. Min: 6 caracteres
  "phoneNumber": "string",      // Opcional
  "role": "string"              // Opcional. Valores: "Client", "Professional", "Admin". Default: "Client"
}
```

**Response (201):**
```json
{
  "success": true,
  "data": {
    "userId": 1,
    "firstName": "Juan",
    "lastName": "Pérez",
    "email": "juan@example.com",
    "role": "Client",
    "phoneNumber": "+51999999999",
    "registrationDate": "2025-11-16T10:30:00Z",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
  }
}
```

---

## 👋 POST /api/auth/logout
Cierra la sesión del usuario y revoca el token JWT

**Headers:**
- `Authorization: Bearer {token}` - Requerido

**Request Body:** Sin body

**Response (200):**
```json
{
  "success": true,
  "message": "👋 Sesión cerrada exitosamente"
}
```

---

## 🔒 POST /api/auth/change-password
Cambia la contraseña del usuario autenticado

**Headers:**
- `Authorization: Bearer {token}` - Requerido

**Request Body:**
```json
{
  "currentPassword": "string",  // Requerido. Contraseña actual
  "newPassword": "string"       // Requerido. Min: 6 caracteres
}
```

**Response (200):**
```json
{
  "success": true,
  "message": "🔒 Contraseña actualizada exitosamente",
  "data": {
    "changedAt": "2025-11-16T10:35:00Z"
  }
}
```

---

## 🚨 Errores Comunes

**400 Bad Request:**
```json
{
  "success": false,
  "message": "⚠️ Los datos enviados no son válidos.",
  "errors": { "Email": ["El campo Email es obligatorio"] }
}
```

**401 Unauthorized:**
```json
{
  "success": false,
  "message": "🚫 Correo o contraseña incorrectos. Inténtalo nuevamente."
}
```

**400 Bad Request (Change Password):**
```json
{
  "success": false,
  "message": "La contraseña actual es incorrecta"
}
```

**401 Unauthorized (Token inválido):**
```json
{
  "success": false,
  "message": "Token inválido: no contiene información del usuario"
}
```
