# Profession API

## 📝 POST /api/profession
Crea una nueva profesión

**Request Body:**
```json
{
  "categoryId": 1,              // Requerido. Debe existir la categoría
  "professionName": "string",   // Requerido. Min: 3, Max: 100 caracteres
  "description": "string"       // Opcional. Max: 500 caracteres
}
```

**Response (201):**
```json
{
  "success": true,
  "message": "Profesión creada exitosamente",
  "data": {
    "professionId": 1,
    "categoryId": 1,
    "categoryName": "Tecnología",
    "professionName": "Desarrollador de Software",
    "description": "Desarrolla aplicaciones y sistemas",
    "totalSpecializations": 0
  }
}
```

---

## 📋 GET /api/profession
Obtiene todas las profesiones

**Request:** Sin parámetros

**Response (200):**
```json
{
  "success": true,
  "data": [
    {
      "professionId": 1,
      "categoryId": 1,
      "categoryName": "Tecnología",
      "professionName": "Desarrollador de Software",
      "description": "Desarrolla aplicaciones y sistemas",
      "totalSpecializations": 3
    }
  ]
}
```

---

## 🔍 GET /api/profession/{id}
Obtiene una profesión por su ID

**URL Params:**
- `id` (uint) - Requerido

**Response (200):**
```json
{
  "success": true,
  "data": {
    "professionId": 1,
    "categoryId": 1,
    "categoryName": "Tecnología",
    "professionName": "Desarrollador de Software",
    "description": "Desarrolla aplicaciones y sistemas",
    "totalSpecializations": 3
  }
}
```

---

## 📂 GET /api/profession/category/{categoryId}
Obtiene todas las profesiones de una categoría

**URL Params:**
- `categoryId` (uint) - Requerido

**Response (200):**
```json
{
  "success": true,
  "data": [
    {
      "professionId": 1,
      "categoryId": 1,
      "categoryName": "Tecnología",
      "professionName": "Desarrollador de Software",
      "description": "Desarrolla aplicaciones y sistemas",
      "totalSpecializations": 3
    }
  ]
}
```

---

## ✏️ PUT /api/profession/{id}
Actualiza una profesión existente

**URL Params:**
- `id` (uint) - Requerido

**Request Body:**
```json
{
  "categoryId": 1,              // Requerido. Debe existir la categoría
  "professionName": "string",   // Requerido. Min: 3, Max: 100 caracteres
  "description": "string"       // Opcional. Max: 500 caracteres
}
```

**Response (200):**
```json
{
  "success": true,
  "message": "Profesión actualizada exitosamente",
  "data": {
    "professionId": 1,
    "categoryId": 1,
    "categoryName": "Tecnología",
    "professionName": "Ingeniero de Software",
    "description": "Nueva descripción",
    "totalSpecializations": 3
  }
}
```

---

## 🗑️ DELETE /api/profession/{id}
Elimina una profesión (solo si no tiene especializaciones asociadas)

**URL Params:**
- `id` (uint) - Requerido

**Response (200):**
```json
{
  "success": true,
  "message": "Profesión eliminada exitosamente"
}
```

---

## 🚨 Errores Comunes

**400 Bad Request:**
```json
{
  "success": false,
  "message": "Datos inválidos",
  "errors": { "ProfessionName": ["El nombre de la profesión es obligatorio"] }
}
```

**404 Not Found:**
```json
{
  "success": false,
  "message": "Profesión no encontrada"
}
```

**409 Conflict:**
```json
{
  "success": false,
  "message": "Ya existe una profesión con el nombre 'Desarrollador de Software'"
}
```
