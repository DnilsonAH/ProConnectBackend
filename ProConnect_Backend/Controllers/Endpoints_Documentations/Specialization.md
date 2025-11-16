# Specialization API

## 📝 POST /api/specialization
Crea una nueva especialización

**Request Body:**
```json
{
  "professionId": 1,              // Requerido. Debe existir la profesión
  "specializationName": "string", // Requerido. Min: 3, Max: 100 caracteres
  "description": "string"         // Requerido. Min: 10, Max: 500 caracteres
}
```

**Response (201):**
```json
{
  "success": true,
  "message": "Especialización creada exitosamente",
  "data": {
    "specializationId": 1,
    "professionId": 1,
    "professionName": "Desarrollador de Software",
    "specializationName": "Frontend Developer",
    "description": "Especializado en desarrollo de interfaces",
    "totalProfiles": 0
  }
}
```

---

## 📋 GET /api/specialization
Obtiene todas las especializaciones

**Request:** Sin parámetros

**Response (200):**
```json
{
  "success": true,
  "data": [
    {
      "specializationId": 1,
      "professionId": 1,
      "professionName": "Desarrollador de Software",
      "specializationName": "Frontend Developer",
      "description": "Especializado en desarrollo de interfaces",
      "totalProfiles": 5
    }
  ]
}
```

---

## 🔍 GET /api/specialization/{id}
Obtiene una especialización por su ID

**URL Params:**
- `id` (uint) - Requerido

**Response (200):**
```json
{
  "success": true,
  "data": {
    "specializationId": 1,
    "professionId": 1,
    "professionName": "Desarrollador de Software",
    "specializationName": "Frontend Developer",
    "description": "Especializado en desarrollo de interfaces",
    "totalProfiles": 5
  }
}
```

---

## 📂 GET /api/specialization/profession/{professionId}
Obtiene todas las especializaciones de una profesión

**URL Params:**
- `professionId` (uint) - Requerido

**Response (200):**
```json
{
  "success": true,
  "data": [
    {
      "specializationId": 1,
      "professionId": 1,
      "professionName": "Desarrollador de Software",
      "specializationName": "Frontend Developer",
      "description": "Especializado en desarrollo de interfaces",
      "totalProfiles": 5
    }
  ]
}
```

---

## ✏️ PUT /api/specialization/{id}
Actualiza una especialización existente

**URL Params:**
- `id` (uint) - Requerido

**Request Body:**
```json
{
  "professionId": 1,              // Requerido. Debe existir la profesión
  "specializationName": "string", // Requerido. Min: 3, Max: 100 caracteres
  "description": "string"         // Requerido. Min: 10, Max: 500 caracteres
}
```

**Response (200):**
```json
{
  "success": true,
  "message": "Especialización actualizada exitosamente",
  "data": {
    "specializationId": 1,
    "professionId": 1,
    "professionName": "Desarrollador de Software",
    "specializationName": "Full Stack Developer",
    "description": "Nueva descripción actualizada",
    "totalProfiles": 5
  }
}
```

---

## 🗑️ DELETE /api/specialization/{id}
Elimina una especialización (solo si no tiene perfiles asociados)

**URL Params:**
- `id` (uint) - Requerido

**Response (200):**
```json
{
  "success": true,
  "message": "Especialización eliminada exitosamente"
}
```

---

## 🚨 Errores Comunes

**400 Bad Request:**
```json
{
  "success": false,
  "message": "Datos inválidos",
  "errors": { "SpecializationName": ["El nombre de la especialización es obligatorio"] }
}
```

**404 Not Found:**
```json
{
  "success": false,
  "message": "Especialización no encontrada"
}
```

**409 Conflict:**
```json
{
  "success": false,
  "message": "Ya existe una especialización con el nombre 'Frontend Developer'"
}
```
