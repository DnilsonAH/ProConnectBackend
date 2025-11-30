# ProfessionalProfile API

## 📝 POST /api/ProfessionalProfile

Crea un nuevo perfil profesional para el usuario autenticado.

**Headers:**

- `Authorization: Bearer {token}` - Requerido

**Request Body:**

```json
{
  "experience": "string", // Requerido. Max: 1000 caracteres
  "presentation": "string", // Requerido. Max: 1000 caracteres
  "specializationIds": [1, 2] // Opcional. Lista de IDs de especializaciones
}
```

**Response (201):**

```json
{
  "success": true,
  "message": "Perfil profesional creado exitosamente",
  "data": {
    "profileId": 1,
    "userId": 1,
    "experience": "string",
    "presentation": "string",
    "specializations": [
      {
        "specializationId": 1,
        "specializationName": "Desarrollo Web",
        "professionName": "Ingeniería de Software",
        "totalProfiles": 10
      },
      {
        "specializationId": 2,
        "specializationName": "Backend",
        "professionName": "Ingeniería de Software",
        "totalProfiles": 5
      }
    ]
  }
}
```

---

## ✏️ PUT /api/ProfessionalProfile/{id}

Actualiza un perfil profesional existente.

**Headers:**

- `Authorization: Bearer {token}` - Requerido

**Request Body:**

```json
{
  "experience": "string", // Opcional. Max: 1000 caracteres
  "presentation": "string", // Opcional. Max: 1000 caracteres
  "specializationIds": [1, 3] // Opcional. Reemplaza la lista anterior
}
```

**Response (200):**

```json
{
  "success": true,
  "message": "Perfil profesional actualizado exitosamente",
  "data": {
    "profileId": 1,
    "userId": 1,
    "experience": "string",
    "presentation": "string",
    "specializations": [
      {
        "specializationId": 1,
        "specializationName": "Desarrollo Web",
        "professionName": "Ingeniería de Software",
        "totalProfiles": 10
      },
      {
        "specializationId": 3,
        "specializationName": "Cloud Computing",
        "professionName": "Ingeniería de Software",
        "totalProfiles": 8
      }
    ]
  }
}
```

---

## 🔍 GET /api/ProfessionalProfile/{id}

Obtiene los detalles de un perfil profesional por su ID.

**Response (200):**

```json
{
  "success": true,
  "data": {
    "profileId": 1,
    "userId": 1,
    "experience": "string",
    "presentation": "string",
    "specializations": [
      {
        "specializationId": 1,
        "specializationName": "Desarrollo Web",
        "professionName": "Ingeniería de Software",
        "totalProfiles": 10
      }
    ]
  }
}
```

---

## 📋 GET /api/ProfessionalProfile

Obtiene todos los perfiles profesionales.

**Response (200):**

```json
{
  "success": true,
  "data": [
    {
      "profileId": 1,
      "userId": 1,
      "experience": "string",
      "presentation": "string",
      "specializations": []
    },
    {
      "profileId": 2,
      "userId": 2,
      "experience": "string",
      "presentation": "string",
      "specializations": []
    }
  ]
}
```

---

## 🗑️ DELETE /api/ProfessionalProfile/{id}

Elimina un perfil profesional.

**Headers:**

- `Authorization: Bearer {token}` - Requerido

**Response (200):**

```json
{
  "success": true,
  "message": "Perfil profesional eliminado exitosamente"
}
```

---

## 🔎 GET /api/ProfessionalProfile/search

Busca profesionales con filtros y paginación.

**Headers:**

- `Authorization: Bearer {token}` - Requerido

**Query Parameters:**

- `categoryId` (uint, opcional): ID de la categoría.
- `professionId` (uint, opcional): ID de la profesión.
- `specializationId` (uint, opcional): ID de la especialización.
- `page` (int, opcional): Número de página (default: 1).
- `pageSize` (int, opcional): Tamaño de página (default: 10).

**Response (200):**

```json
{
  "success": true,
  "message": "Resultados obtenidos exitosamente",
  "data": {
    "items": [
      {
        "profileId": 1,
        "userId": 1,
        "firstName": "Juan",
        "firstSurname": "Perez",
        "secondSurname": "Gomez",
        "photoUrl": "http://example.com/photo.jpg",
        "experience": "string",
        "presentation": "string",
        "specializations": [
          {
            "specializationId": 1,
            "specializationName": "Desarrollo Web",
            "professionName": "Ingeniería de Software",
            "totalProfiles": 10
          }
        ]
      }
    ],
    "totalCount": 1,
    "page": 1,
    "pageSize": 10,
    "totalPages": 1
  }
}
```

---

## 🚨 Errores Comunes

**400 Bad Request:**

```json
{
  "success": false,
  "message": "Datos inválidos",
  "errors": { "Experience": ["El campo Experience es obligatorio"] }
}
```

**401 Unauthorized:**

```json
{
  "success": false,
  "message": "Usuario no autenticado o ID inválido"
}
```

**403 Forbidden:**

```json
{
  "success": false,
  "message": "No tiene permisos para modificar este perfil"
}
```

**404 Not Found:**

```json
{
  "success": false,
  "message": "Perfil profesional no encontrado"
}
```

**500 Internal Server Error:**

```json
{
  "success": false,
  "message": "Error interno al crear el perfil"
}
```
