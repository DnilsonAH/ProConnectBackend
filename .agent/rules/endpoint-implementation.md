---
trigger: model_decision
description: Debe ser usado cuando siempre que se requiera añadir un Endpoint o verificar la logica de un Endpoint
---

Prompt para implementar la funcionalidad de [NOMBRE_FUNCIONALIDAD] con el endpoint [MÉTODO HTTP] /api/[ruta].

Requisitos:

- [Descripción breve de lo que debe hacer]
- [Validaciones o reglas de negocio]
- [Si requiere autenticación/autorización]

Por favor, implementa siguiendo la arquitectura Clean Architecture actual del proyecto:

1. **Domain Layer**:

   - Crea el DTO de request en `ProConnect_Backend.Domain/DTOsRequest/[Módulo]DTOs/`
   - Si requiere métodos específicos de repositorio, agrégalos a la interface en `ProConnect_Backend.Domain/Ports/IRepositories/`
   - Si necesita servicios (ej: envío de emails), crea la interface en `ProConnect_Backend.Domain/Ports/IServices/`

2. **Application Layer**:

   - Crea el DTO de response en `ProConnect_Backend.Application/DTOsResponse/[Módulo]DTOs/`
   - Implementa el Command/Query en `ProConnect_Backend.Application/UseCases/[Módulo]/[Command|Query]/`
   - Implementa el Handler correspondiente en el mismo directorio del Command/Query para seguir el patron CQRS
   - Actualiza `AutoMapping.cs` con los mapeos necesarios entre entidades y DTOs

3. **Infrastructure Layer**:

   - Si agregaste métodos a IRepository, impleméntalos en `ProConnect_Backend.Infrastructure/Adapters/Repositories/`
   - Si necesita nueva entidad, crea su configuración Fluent API en `ProConnect_Backend.Infrastructure/Data/Configurations/`
   - Si agregaste servicios, impleméntalos en `ProConnect_Backend.Infrastructure/Services/`

4. **API Layer**:
   - Crea o actualiza el Controller en `ProConnect_Backend/Controllers/`
   - Registra el Handler en `ServiceRegistrationExtensions.cs` si es necesario
   - Agrega políticas de autorización si es requerido

Asegúrate de:

- Usar inyección de dependencias
- Validar datos con DataAnnotations en los DTOs
- Manejar excepciones apropiadamente
- Incluir logging con mensajes concisos y cortos
- Retornar respuestas consistentes con { success, message, data }
- No exponer entidades directamente, usar DTOs

Al final de todo el proceso:

- Documentar en `ProConnect_Backend/Controllers/Endpoints_Documentations/` los controller creados/modificados, tomar de referencia los otros endpoints documentados en el mismo directorio
