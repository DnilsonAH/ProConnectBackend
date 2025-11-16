# 📚 API Documentation Index

Documentación de todos los endpoints implementados en ProConnect Backend.

## 📂 Controladores Disponibles

### Autenticación y Usuarios
- **[Auth.md](Auth.md)** - Autenticación y gestión de sesiones (4 endpoints)
- **[User.md](User.md)** - Gestión de información de usuarios (3 endpoints)

### Profesiones y Especializaciones
- **[ProfessionCategory.md](ProfessionCategory.md)** - Gestión de categorías de profesión (5 endpoints)
- **[Profession.md](Profession.md)** - Gestión de profesiones (6 endpoints)
- **[Specialization.md](Specialization.md)** - Gestión de especializaciones (6 endpoints)
- **[ProfileSpecialization.md](ProfileSpecialization.md)** - Asignación de especializaciones a perfiles (4 endpoints)

---

## 🎯 Orden Jerárquico de Creación

Para crear datos correctamente, sigue este orden:

1. **ProfessionCategory** → Crear categorías primero
2. **Profession** → Requiere `categoryId` existente
3. **Specialization** → Requiere `professionId` existente
4. **ProfileSpecialization** → Requiere `profileId` y `specializationId` existentes

---

## 🔒 Restricciones de Eliminación

- **ProfessionCategory**: No se puede eliminar si tiene profesiones asociadas
- **Profession**: No se puede eliminar si tiene especializaciones asociadas
- **Specialization**: No se puede eliminar si tiene perfiles asociados

---

## 📝 Formato de Respuestas

Todas las respuestas siguen el formato:

```json
{
  "success": true/false,
  "message": "Mensaje descriptivo",    // Opcional
  "data": { ... }                      // Opcional
}
```

---

## 🚨 Códigos HTTP Principales

- **200 OK** - Operación exitosa
- **201 Created** - Recurso creado exitosamente
- **400 Bad Request** - Datos inválidos o validación fallida
- **404 Not Found** - Recurso no encontrado
- **409 Conflict** - Violación de reglas de negocio
- **500 Internal Server Error** - Error del servidor
