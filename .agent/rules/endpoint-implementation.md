---
trigger: model_decision
description: Debe ser usado cuando siempre que se requiera añadir un Endpoint o verificar la logica de un Endpoint
---

# Prompt para Implementar Funcionalidad con Clean Architecture

## Funcionalidad: [NOMBRE_FUNCIONALIDAD]

**Endpoint:** `[MÉTODO HTTP] /api/[ruta]`

---

## Requisitos Funcionales

- [Descripción breve de lo que debe hacer]
- [Validaciones o reglas de negocio específicas]
- [Si requiere autenticación/autorización: especificar roles]
- [Relaciones con otras entidades si aplica]

---

## Implementación por Capas (Clean Architecture)

### **Domain Layer** (Capa de Dominio)

#### DTOs de Request

- **Ubicación:** `ProConnect_Backend.Domain/DTOsRequest/[Módulo]DTOs/`
- **Archivos a crear:**
  - `Create[Entidad]Dto.cs` - Para operaciones POST
  - `Update[Entidad]Dto.cs` - Para operaciones PUT/PATCH
- **Requisitos:**
  - Incluir `using System.ComponentModel.DataAnnotations;`
  - Aplicar validaciones con DataAnnotations (`[Required]`, `[MaxLength]`, `[Range]`, etc.)
  - Usar tipos nullables (`?`) solo cuando el campo sea opcional
  - Inicializar strings con `= null!;` para campos requeridos

#### 🔌 Interfaces de Repositorio

- **Ubicación:** `ProConnect_Backend.Domain/Ports/IRepositories/`
- **Archivo:** `I[Entidad]Repository.cs`
- **Requisitos:**
  - Heredar de `IGenericRepository<[Entidad]>`
  - Agregar solo métodos específicos de negocio (consultas complejas, validaciones)
  - Usar `Task<>` para métodos asíncronos
  - Documentar con comentarios XML cada método personalizado

#### Interfaces de Servicios (si aplica)

- **Ubicación:** `ProConnect_Backend.Domain/Ports/IServices/`
- **Ejemplos:** `IEmailService.cs`, `IStorageService.cs`

---

### **Application Layer** (Capa de Aplicación)

#### DTOs de Response

- **Ubicación:** `ProConnect_Backend.Application/DTOsResponse/[Módulo]DTOs/`
- **Archivo:** `[Entidad]ResponseDto.cs`
- **Requisitos:**
  - Incluir todas las propiedades que el cliente necesita
  - NO exponer propiedades sensibles (passwords, tokens internos)
  - Usar tipos apropiados (string para TimeOnly formateado)

#### Commands y Queries (CQRS)

- **Ubicación:** `ProConnect_Backend.Application/UseCases/[Módulo]/`
- **Estructura de carpetas:**
  ```
  [Módulo]/
  ├── Commands/
  │   ├── Create[Entidad]/
  │   │   ├── Create[Entidad]Command.cs
  │   │   └── Create[Entidad]Handler.cs
  │   ├── Update[Entidad]/
  │   │   ├── Update[Entidad]Command.cs
  │   │   └── Update[Entidad]Handler.cs
  │   └── Delete[Entidad]/
  │       ├── Delete[Entidad]Command.cs
  │       └── Delete[Entidad]Handler.cs
  └── Queries/
      ├── Get[Entidad]ById/
      │   ├── Get[Entidad]ByIdQuery.cs
      │   └── Get[Entidad]ByIdHandler.cs
      └── GetAll[Entidad]s/
          ├── GetAll[Entidad]sQuery.cs
          └── GetAll[Entidad]sHandler.cs
  ```

#### Requisitos para Commands/Queries

- Usar pattern `record` con sintaxis: `public record Create[Entidad]Command([Dto] Dto) : IRequest<[Response]ResponseDto>;`
- Importar `MediatR`, DTOs de Response y Request

#### Requisitos CRÍTICOS para Handlers

**CreateHandler - Dependencias obligatorias:**

- `IUnitOfWork` (NO repositorio directo)
- `IMapper`
- `ILogger<Handler>`
- `IHttpContextAccessor` (si necesita autenticación)

**CreateHandler - Flujo de implementación:**

1. Registrar inicio con `_logger.LogInformation()`
2. Validar autenticación (si aplica): verificar `User`, extraer `ClaimTypes.NameIdentifier` y `ClaimTypes.Role`
3. Validar relaciones: verificar que entidades relacionadas existan con `_unitOfWork.[Related]Repository.GetByIdAsync()`
4. Validaciones de negocio personalizadas (unicidad, rangos, etc.)
5. Mapear DTO → Entity con `_mapper.Map<Entity>(dto)`
6. Guardar: `await _unitOfWork.[Entidad]Repository.AddAsync(entity)`
7. **CRÍTICO:** `await _unitOfWork.SaveChangesAsync()`
8. Registrar éxito con `_logger.LogInformation()`
9. Mapear Entity → ResponseDto con `_mapper.Map<ResponseDto>(entity)`
10. Agregar propiedades calculadas si es necesario

**UpdateHandler - Flujo específico:**

1. Obtener entidad existente con `GetByIdAsync()`, lanzar `KeyNotFoundException` si no existe
2. Validar permisos: Admin o dueño del recurso
3. Mapear cambios con `_mapper.Map(dto, entity)`
4. Actualizar: `_unitOfWork.[Entidad]Repository.Update(entity)`
5. **CRÍTICO:** `await _unitOfWork.SaveChangesAsync()`

**DeleteHandler - Flujo específico:**

1. Obtener entidad, validar existencia y permisos
2. Eliminar: `_unitOfWork.[Entidad]Repository.Delete(entity)`
3. **CRÍTICO:** `await _unitOfWork.SaveChangesAsync()`

**QueryHandler - Flujo simplificado:**

- Solo necesita `IUnitOfWork` e `IMapper`
- Obtener datos y mapear con `_mapper.Map<ResponseDto>(entity)`

#### AutoMapper Configuration

- **Ubicación:** `ProConnect_Backend.Application/Mapping/AutoMapping.cs`
- **Requisitos:**
  - Crear sección con comentario `// MAPEOS PARA [MÓDULO]`
  - Mapeo Create: `CreateMap<Create[Entidad]Dto, [Entidad]>()` ignorando ID y navegaciones
  - Mapeo Update: `CreateMap<Update[Entidad]Dto, [Entidad]>()` si difiere del Create
  - Mapeo Response: `CreateMap<[Entidad], [Entidad]ResponseDto>()` formateando fechas/horas con `ToString()`

---

### **Infrastructure Layer** (Capa de Infraestructura)

#### Repository Implementation

- **Ubicación:** `ProConnect_Backend.Infrastructure/Adapters/Repositories/`
- **Archivo:** `[Entidad]Repository.cs`
- **Requisitos:**
  - Heredar de `GenericRepository<[Entidad]>` e implementar `I[Entidad]Repository`
  - Constructor recibe `ProConnectDbContext` y lo pasa a `base(dbContext)`
  - Implementar solo métodos específicos de negocio definidos en la interface
  - Usar `Include()` para eager loading de relaciones si es necesario
  - Usar `ToListAsync()` para operaciones asíncronas

#### Entity Configuration (Fluent API)

- **Ubicación:** `ProConnect_Backend.Infrastructure/Data/Configurations/`
- **Archivo:** `[Entidad]Configuration.cs`
- **Solo si es una nueva entidad**

#### Service Implementation (si aplica)

- **Ubicación:** `ProConnect_Backend.Infrastructure/Services/`

#### Actualizar UnitOfWork

- **Archivo:** `ProConnect_Backend.Infrastructure/Adapters/UnitOfWork.cs`
- **Si es un nuevo repositorio:**
  - Agregar propiedad: `public I[Entidad]Repository [Entidad]Repository { get; }`
  - Agregar parámetro al constructor: `I[Entidad]Repository [entidad]Repository`
  - Asignar en constructor: `[Entidad]Repository = [entidad]Repository;`

---

### **API Layer** (Capa de Presentación)

#### Controller

- **Ubicación:** `ProConnect_Backend/Controllers/`
- **Archivo:** `[Entidad]Controller.cs`
- **Requisitos generales:**

  - Heredar de `ControllerBase` con atributos `[ApiController]` y `[Route]`
  - Inyectar `IMediator` e `ILogger<Controller>`
  - Agregar comentarios XML `/// <summary>` a cada endpoint
  - Retornar formato consistente: `{ success, message, data }`

- **Endpoint CREATE [HttpPost]:**

  - `[Authorize]` o rol específico según requisitos
  - Validar `ModelState.IsValid`, retornar `BadRequest` si falla
  - Crear Command, enviar con `_mediator.Send()`
  - Retornar `CreatedAtAction(nameof(GetById), new { id }, response)`
  - Catch: `UnauthorizedAccessException` → 403, `KeyNotFoundException` → 404, `InvalidOperationException` → 400, `Exception` → 500

- **Endpoint GET BY ID [HttpGet("{id}")]:**

  - `[AllowAnonymous]` o `[Authorize]` según requisitos
  - Crear Query, enviar con `_mediator.Send()`
  - Verificar si result es null, retornar `NotFound`
  - Retornar `Ok` con data

- **Endpoint GET ALL [HttpGet]:**

  - Similar a GetById pero sin validación de null
  - Retornar lista en `{ success: true, data: [] }`

- **Endpoint UPDATE [HttpPut("{id}")]:**

  - `[Authorize]`
  - Validar que `id == dto.[Entidad]Id`
  - Validar `ModelState.IsValid`
  - Catch adicionales: `KeyNotFoundException`, `UnauthorizedAccessException`

- **Endpoint DELETE [HttpDelete("{id}")]:**
  - `[Authorize]`
  - Command solo recibe `id`
  - Retornar `Ok` con mensaje de éxito
  - Catch: `KeyNotFoundException`, `UnauthorizedAccessException`, `InvalidOperationException`

#### Registro de Servicios

- **Archivo:** `ProConnect_Backend/Configuration/ServiceRegistrationExtensions.cs`
- **Solo si es necesario registrar servicios personalizados**
- MediatR registra automáticamente los handlers

---

## Checklist de Implementación

### Domain Layer

- [ ] DTOs de Request creados con validaciones DataAnnotations
- [ ] Interface de Repository actualizada (si necesita métodos específicos)
- [ ] Interface de Service creada (si aplica)

### Application Layer

- [ ] DTOs de Response creados
- [ ] Commands/Queries implementados con pattern `record`
- [ ] **Handlers usan `IUnitOfWork`** (no repositorio directo)
- [ ] **Handlers usan `IMapper`** para mappings
- [ ] **Handlers incluyen `ILogger`** para logging
- [ ] **Handlers llaman `await _unitOfWork.SaveChangesAsync()`** después de modificaciones
- [ ] Validaciones de negocio implementadas (verificar entidades relacionadas existen)
- [ ] Validaciones de autorización implementadas (Admin o dueño)
- [ ] AutoMapping.cs actualizado con todos los mappings necesarios

### Infrastructure Layer

- [ ] Repository implementado con métodos específicos
- [ ] UnitOfWork actualizado (si es nuevo repositorio)
- [ ] Services implementados (si aplica)

### API Layer

- [ ] Controller creado con todos los endpoints CRUD
- [ ] Incluye endpoint `GetById` además de consultas específicas
- [ ] Manejo de excepciones correcto (UnauthorizedAccess, KeyNotFound, InvalidOperation)
- [ ] Respuestas consistentes con formato `{ success, message, data }`
- [ ] Atributos de autorización correctos
- [ ] Validación de ModelState
- [ ] Logging de errores

### Documentación

- [ ] Endpoint documentado en `ProConnect_Backend/Controllers/Endpoints_Documentations/`

---

## Errores Comunes a EVITAR

1. **NO** inyectar `I[Entidad]Repository` directamente en handlers → Usar `IUnitOfWork`
2. **NO** crear DTOs/Entities manualmente → Usar `IMapper`
3. **NO** olvidar `await _unitOfWork.SaveChangesAsync()` después de Add/Update/Delete
4. **NO** olvidar incluir `ILogger` en handlers
5. **NO** exponer entidades del dominio directamente → Siempre usar DTOs de Response
6. **NO** crear un CRUD incompleto → Implementar GetById además de consultas específicas
7. **NO** olvidar validar que entidades relacionadas existan antes de crear
8. **NO** olvidar validaciones de negocio (StartTime < EndTime, unicidad, etc.)
9. **NO** olvidar verificar permisos (Admin o dueño del recurso)
10. **NO** olvidar agregar mappings bidireccionales en AutoMapper

---

## Referencias del Proyecto

- **Ejemplo de implementación correcta:** `Specialization` (usa IUnitOfWork, IMapper, ILogger)
- **Patrones de Controller:** Ver `SpecializationController.cs`
- **Patrones de Handler:** Ver `CreateSpecializationHandler.cs`
- **Patrones de AutoMapper:** Ver sección de Specialization en `AutoMapping.cs`

---

## Documentación Final

Una vez completada la implementación, documentar en:
**`ProConnect_Backend/Controllers/Endpoints_Documentations/[Entidad]Controller.md`**

Incluir:

- Descripción de cada endpoint
- Métodos HTTP y rutas
- Request/Response examples
- Códigos de estado HTTP
- Reglas de autorización
- Validaciones aplicadas

---
