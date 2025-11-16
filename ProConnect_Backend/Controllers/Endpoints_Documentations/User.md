# User API

## 👤 GET /api/user/me
Obtiene la información del usuario autenticado actual

**Headers:**
- `Authorization: Bearer {token}` - Requerido

**Request:** Sin parámetros

**Response (200):**
```json
{
  "success": true,
  "data": {
    "userId": 1,
    "firstName": "Juan",
    "lastName": "Pérez",
    "email": "juan@example.com",
    "role": "Client",
    "phone": "+51999999999",
    "country": null,              // Opcional
    "registrationDate": "2025-11-16T10:30:00Z"
  }
}
```

---

## 🔍 GET /api/user/{id}
Obtiene la información de un usuario específico por su ID

**URL Params:**
- `id` (uint) - Requerido

**Response (200):**
```json
{
  "success": true,
  "data": {
    "userId": 1,
    "firstName": "Juan",
    "lastName": "Pérez",
    "email": "juan@example.com",
    "role": "Client",
    "phone": "+51999999999",
    "country": null,              // Opcional
    "registrationDate": "2025-11-16T10:30:00Z"
  }
}
```

---

## ✏️ PUT /api/user/{id}
Actualiza la información de un usuario (requiere autenticación)

**Headers:**
- `Authorization: Bearer {token}` - Requerido

**URL Params:**
- `id` (uint) - Requerido. El usuario solo puede actualizar su propia información (excepto Admin)

**Request Body:**
```json
{
  "firstName": "string",        // Opcional
  "firstSurname": "string",     // Opcional
  "secondSurname": "string",    // Opcional
  "phoneNumber": "string",      // Opcional
  "email": "string"             // Opcional. Formato email válido
}
```

**Response (200):**
```json
{
  "success": true,
  "message": "Usuario actualizado exitosamente",
  "data": {
    "userId": 1,
    "firstName": "Juan Carlos",
    "lastName": "Pérez García",
    "email": "juan.carlos@example.com",
    "role": "Client",
    "phone": "+51988888888",
    "country": null,
    "registrationDate": "2025-11-16T10:30:00Z"
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
  "errors": { "Email": ["El formato del email no es válido"] }
}
```

**401 Unauthorized:**
```json
{
  "success": false,
  "message": "Token inválido: no contiene información del usuario"
}
```

**403 Forbidden:**
```json
{
  "success": false,
  "message": "No tienes permisos para actualizar este usuario"
}
```

**404 Not Found:**
```json
{
  "success": false,
  "message": "Usuario no encontrado"
}
```

---

## 📝 Notas

- El endpoint `/api/user/me` obtiene automáticamente el usuario desde el token JWT
- Solo el propio usuario o un Admin pueden actualizar la información de un usuario
- El campo `country` actualmente no existe en la entidad User (retorna null)
