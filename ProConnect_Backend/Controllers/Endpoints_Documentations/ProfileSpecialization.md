# ProfileSpecialization API

## 🔗 POST /api/profilespecialization/assign
Asigna una especialización a un perfil profesional

**Request Body:**
```json
{
  "profileId": 1,           // Requerido. Debe existir el perfil
  "specializationId": 1     // Requerido. Debe existir la especialización
}
```

**Response (200):**
```json
{
  "success": true,
  "message": "Especialización asignada exitosamente",
  "data": {
    "profileSpecializationId": 1,
    "profileId": 1,
    "specializationId": 1,
    "specializationName": "Frontend Developer",
    "professionName": "Desarrollador de Software",
    "createdAt": "2025-11-16T10:30:00Z"
  }
}
```

---

## 🔓 DELETE /api/profilespecialization/remove
Remueve una especialización de un perfil profesional

**Query Params:**
- `profileId` (uint) - Requerido
- `specializationId` (uint) - Requerido

**Ejemplo:**
```
DELETE /api/profilespecialization/remove?profileId=1&specializationId=1
```

**Response (200):**
```json
{
  "success": true,
  "message": "Especialización removida exitosamente"
}
```

---

## 👤 GET /api/profilespecialization/profile/{profileId}
Obtiene todas las especializaciones de un perfil

**URL Params:**
- `profileId` (uint) - Requerido

**Response (200):**
```json
{
  "success": true,
  "data": [
    {
      "profileSpecializationId": 1,
      "profileId": 1,
      "specializationId": 1,
      "specializationName": "Frontend Developer",
      "professionName": "Desarrollador de Software",
      "createdAt": "2025-11-16T10:30:00Z"
    }
  ]
}
```

---

## 🎯 GET /api/profilespecialization/specialization/{specializationId}
Obtiene todos los perfiles que tienen una especialización

**URL Params:**
- `specializationId` (uint) - Requerido

**Response (200):**
```json
{
  "success": true,
  "data": [
    {
      "profileSpecializationId": 1,
      "profileId": 1,
      "specializationId": 1,
      "specializationName": "Frontend Developer",
      "professionName": "Desarrollador de Software",
      "createdAt": "2025-11-16T10:30:00Z"
    }
  ]
}
```

---

## 🚨 Errores Comunes

**400 Bad Request:**
```json
{
  "success": false,
  "message": "Datos inválidos",
  "errors": { "ProfileId": ["El ID del perfil profesional es obligatorio"] }
}
```

**404 Not Found:**
```json
{
  "success": false,
  "message": "No se encontró el perfil profesional con ID 1"
}
```

**409 Conflict:**
```json
{
  "success": false,
  "message": "La especialización ya está asignada a este perfil profesional"
}
```
