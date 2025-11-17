# ProfessionCategory API

## 📝 POST /api/professioncategory
Crea una nueva categoría de profesión

**Request Body:**
```json
{
  "categoryName": "string",     // Requerido. Min: 3, Max: 100 caracteres
  "description": "string"       // Opcional. Max: 500 caracteres
}
```

**Response (201):**
```json
{
  "success": true,
  "message": "Categoría creada exitosamente",
  "data": {
    "categoryId": 1,
    "categoryName": "Tecnología",
    "description": "Profesiones relacionadas con tecnología",
    "totalProfessions": 0
  }
}
```

---

## 📋 GET /api/professioncategory
Obtiene todas las categorías de profesión

**Request:** Sin parámetros

**Response (200):**
```json
{
  "success": true,
  "data": [
    {
      "categoryId": 1,
      "categoryName": "Tecnología",
      "description": "Profesiones relacionadas con tecnología",
      "totalProfessions": 5
    }
  ]
}
```

---

## 🔍 GET /api/professioncategory/{id}
Obtiene una categoría por su ID

**URL Params:**
- `id` (uint) - Requerido

**Response (200):**
```json
{
  "success": true,
  "data": {
    "categoryId": 1,
    "categoryName": "Tecnología",
    "description": "Profesiones relacionadas con tecnología",
    "totalProfessions": 5
  }
}
```

---

## ✏️ PUT /api/professioncategory/{id}
Actualiza una categoría existente

**URL Params:**
- `id` (uint) - Requerido

**Request Body:**
```json
{
  "categoryName": "string",     // Requerido. Min: 3, Max: 100 caracteres
  "description": "string"       // Opcional. Max: 500 caracteres
}
```

**Response (200):**
```json
{
  "success": true,
  "message": "Categoría actualizada exitosamente",
  "data": {
    "categoryId": 1,
    "categoryName": "Tecnología Avanzada",
    "description": "Nueva descripción",
    "totalProfessions": 5
  }
}
```

---

## 🗑️ DELETE /api/professioncategory/{id}
Elimina una categoría (solo si no tiene profesiones asociadas)

**URL Params:**
- `id` (uint) - Requerido

**Response (200):**
```json
{
  "success": true,
  "message": "Categoría eliminada exitosamente"
}
```

---

## 🚨 Errores Comunes

**400 Bad Request:**
```json
{
  "success": false,
  "message": "Datos inválidos",
  "errors": { "CategoryName": ["El nombre de la categoría es obligatorio"] }
}
```

**404 Not Found:**
```json
{
  "success": false,
  "message": "Categoría no encontrada"
}
```

**409 Conflict:**
```json
{
  "success": false,
  "message": "Ya existe una categoría con el nombre 'Tecnología'"
}
```
