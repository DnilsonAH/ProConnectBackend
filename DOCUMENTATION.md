# ProConnect — Documentación del Proyecto

Esta documentación proporciona una visión completa y práctica del proyecto ProConnect (backend), incluyendo configuración, variables de entorno, endpoints del API, ejemplos de peticiones/respuestas y recomendaciones para desarrollo y despliegue.

> Nota: El contenido se generó analizando los archivos clave del proyecto (`Program.cs`, `appsettings.json`) y los controladores en `ProConnect_Backend/Controllers`.

---

## Índice

- Resumen del proyecto
- Tecnologías usadas
- Configuración y variables de entorno
  - `.env.example` sugerido
  - Campos en `appsettings.json`
- Cómo ejecutar localmente (Windows / cmd.exe)
- Docker / Despliegue (Notas)
- Seguridad y Autenticación (JWT)
- ZEGOCLOUD (videoconsultas)
- CORS y Swagger
- Endpoints del API (resumen por controlador)
- Manejo de errores y códigos HTTP
- Buenas prácticas y próximos pasos

---

## Resumen del proyecto

ProConnect es el backend de una aplicación de gestión de profesionales y videoconsultas. Está estructurado en capas (API / Application / Domain / Infrastructure). Usa MediatR para patrones CQRS/mediator y Entity Framework Core para acceso a datos.

---

## Tecnologías principales

- .NET 7+ (API Web)
- C#
- MediatR (patrón Mediator)
- Entity Framework Core (contexto `ProConnectDbContext` en Infrastructure)
- ZEGOCLOUD (servicio de video, se utiliza AppId + ServerSecret)
- Swagger/OpenAPI integrado

---

## Configuración y variables de entorno

El proyecto carga variables desde el sistema y, en desarrollo local, intenta cargar un archivo `.env` (busca en 3 rutas relativas). Las variables críticas que debe proveer el entorno son:

Variables críticas requeridas (según `Program.cs`):

- DB_SERVER
- DB_PORT (opcional, por defecto 3306)
- DB_DATABASE
- DB_USER
- DB_PASSWORD
- JWT_SECRET_KEY
- JWT_ISSUER
- JWT_AUDIENCE
- JWT_EXPIRATION_HOURS (opcional, por defecto 24)
- ZEGOCLOUD_APP_ID
- ZEGOCLOUD_SERVER_SECRET

Además, `appsettings.json` contiene la sección `JwtSettings` y `ZEGOCLOUD` que son sobrescritas por variables de entorno en `Program.cs`.

Ejemplo sugerido de `.env.example` (no incluyas secretos reales en el repositorio):

```env
# Base de datos
DB_SERVER=localhost
DB_PORT=3306
DB_DATABASE=proconnect_db
DB_USER=proconnect_user
DB_PASSWORD=ChangeMe123!

# JWT
JWT_SECRET_KEY=tu_clave_super_secreta_larga
JWT_ISSUER=ProConnectIssuer
JWT_AUDIENCE=ProConnectAudience
JWT_EXPIRATION_HOURS=24

# ZEGOCLOUD (videoconsultas)
ZEGOCLOUD_APP_ID=123456789
ZEGOCLOUD_SERVER_SECRET=tu_zego_server_secret
```

Campos en `appsettings.json` observados:

- Logging
- AllowedHosts
- ConnectionStrings:DefaultConnection (se sobrescribe en runtime)
- JwtSettings (SecretKey, Issuer, Audience, ExpirationHours)
- ZEGOCLOUD (ZEGOCLOUD_APP_ID, ZEGOCLOUD_APP_SIGN, ZEGOCLOUD_SERVER_SECRET) — Nota: `Program.cs` usa `ZegoCloud:AppId` y `ZegoCloud:ServerSecret`.

---

## Cómo ejecutar localmente (Windows / cmd.exe)

Requisitos previos:

- .NET SDK instalado (versión compatible con el proyecto)
- MySQL o la base de datos configurada según variables de entorno
- Variables de entorno cargadas o un archivo `.env` en una de las rutas que `Program.cs` busca

Desde la raíz del repo, en una consola `cmd.exe`:

```cmd
REM Construir solución
dotnet build ProConnect_Backend.sln

REM Ejecutar el proyecto API (desde la carpeta principal del proyecto Web)
cd ProConnect_Backend
dotnet run
```

Notas:
- `Program.cs` intentará cargar `.env` en desarrollo. Si no lo encuentra, advertirá por consola.
- Swagger se habilita automáticamente en entornos de desarrollo; accede a `http://localhost:{PORT}/swagger`.

---

## Docker / Despliegue (Notas rápidas)

- `Program.cs` detecta si está en contenedor mediante `DOTNET_RUNNING_IN_CONTAINER` y no carga `.env` en ese caso.
- Para despliegues (Fly.io, Docker), usa variables de entorno seguras (secret management) para las claves y credenciales.
- Asegúrate de exponer el puerto correcto y configurar `ConnectionStrings:DefaultConnection` mediante variables de entorno.

---

## Seguridad y Autenticación (JWT)

- El proyecto usa JWT para autenticación. `JwtSettings` (SecretKey, Issuer, Audience, ExpirationHours) se configuran desde variables de entorno.
- Para rutas protegidas, incluye el header:

```
Authorization: Bearer {tu_jwt_token}
```

- Hay endpoints que requieren roles (por ejemplo `Authorize(Roles = "Admin")` en algunos endpoints de `WeeklyAvailabilityController`).

---

## ZEGOCLOUD (Videoconsultas)

- `Program.cs` requiere `ZEGOCLOUD_APP_ID` y `ZEGOCLOUD_SERVER_SECRET`.
- Estos valores se mapean a la configuración interna `ZegoCloud:AppId` y `ZegoCloud:ServerSecret` para el servicio que genera tokens de sesión de video.
- Nunca subas `ZEGOCLOUD_SERVER_SECRET` a repositorio público.

---

## CORS y Swagger

- Política CORS llamada `AllowFrontend` permite orígenes:
  - http://localhost:3000
  - http://localhost:5200
  - https://proconnectnext.fly.dev
- Swagger está configurado con soporte para autorización Bearer (puedes pegar el JWT en la UI de Swagger).

---

## Endpoints del API (resumen)

A continuación se listan los controladores analizados y sus endpoints principales. Los prefijos usan `api/[controller]` salvo excepciones documentadas.

1) `AuthController` (ruta: `api/auth`)
- POST /api/auth/login — Iniciar sesión (body: `LoginRequestDto`). Respuesta: token + user data.
- POST /api/auth/register — Registrar usuario (body: `RegisterRequestDto`).
- POST /api/auth/logout — Requiere `Authorization`. Revoca token.
- POST /api/auth/change-password — Requiere `Authorization`. Cambiar contraseña (body: `ChangePasswordRequestDto`).

2) `UserController` (ruta: `api/user`)
- GET /api/user/me — Requiere `Authorization`. Obtiene usuario autenticado.
- GET /api/user/{id} — Obtener usuario por id.
- PUT /api/user/{id} — Requiere `Authorization`. Actualizar usuario (solo el propio usuario o `Admin`). (body: `UpdateUserRequestDto`).

3) `ProfessionalProfileController` (ruta: `api/professionalprofile`)
- POST /api/professionalprofile — Requiere `Authorization`. Crear perfil profesional (`CreateProfessionalProfileDto`).
- GET /api/professionalprofile/{id} — Obtener perfil por id.
- GET /api/professionalprofile — Obtener todos los perfiles.
- PUT /api/professionalprofile/{id} — Requiere `Authorization`. Actualizar perfil (`UpdateProfessionalProfileDto`).
- DELETE /api/professionalprofile/{id} — Requiere `Authorization`. Eliminar perfil.
- GET /api/professionalprofile/search — Requiere `Authorization`. Buscar profesionales con filtros (Query params: `FilterProfessionalsRequestDto`, paginado).

4) `ProfessionCategoryController` (ruta: `api/professioncategory`)
- POST /api/professioncategory — Crear categoría (`CreateProfessionCategoryRequestDto`).
- GET /api/professioncategory — Obtener todas.
- GET /api/professioncategory/{id} — Obtener por id.
- PUT /api/professioncategory/{id} — Actualizar (`UpdateProfessionCategoryRequestDto`).
- DELETE /api/professioncategory/{id} — Eliminar.

5) `ProfessionController` (ruta: `api/profession`)
- POST /api/profession — Crear profesión (`CreateProfessionRequestDto`).
- GET /api/profession — Obtener todas.
- GET /api/profession/{id} — Obtener por id.
- GET /api/profession/category/{categoryId} — Obtener profesiones por categoría.
- PUT /api/profession/{id} — Actualizar (`UpdateProfessionRequestDto`).
- DELETE /api/profession/{id} — Eliminar.

6) `SpecializationController` (ruta: `api/specialization`)
- POST /api/specialization — Crear especialización (`CreateSpecializationRequestDto`).
- GET /api/specialization — Obtener todas.
- GET /api/specialization/{id} — Obtener por id.
- GET /api/specialization/profession/{professionId} — Obtener por profesión.
- PUT /api/specialization/{id} — Actualizar (`UpdateSpecializationRequestDto`).
- DELETE /api/specialization/{id} — Eliminar.

7) `ProfileSpecializationController` (ruta: `api/profilespecialization`)
- POST /api/profilespecialization/assign — Asignar especialización a perfil (`AssignSpecializationRequestDto`).
- DELETE /api/profilespecialization/remove?profileId={profileId}&specializationId={specializationId} — Remover especialización.
- GET /api/profilespecialization/profile/{profileId} — Obtener especializaciones de un perfil.
- GET /api/profilespecialization/specialization/{specializationId} — Obtener perfiles por especialización.

8) `WeeklyAvailabilityController` (ruta: `api/weekly-availability`)
- POST /api/weekly-availability — Requiere `Authorization`. Crear disponibilidad semanal (`CreateWeeklyAvailabilityDto`).
- PUT /api/weekly-availability — Requiere `Authorization`. Actualizar availability (`UpdateWeeklyAvailabilityDto`).
- DELETE /api/weekly-availability/{id} — Requiere `Authorization`. Eliminar.
- GET /api/weekly-availability/professional/{professionalId} — Público. Obtener disponibilidades de un profesional.
- PUT /api/weekly-availability/admin/{professionalId} — `Authorize(Roles = "Admin")` Actualizar por admin.
- DELETE /api/weekly-availability/admin/{professionalId}/{weeklyAvailabilityId} — `Authorize(Roles = "Admin")` Eliminar por admin.

9) `DisponibilityController` (ruta: `api/disponibility`) — Endpoint documental simple:
- GET /api/disponibility/ — Comprueba disponibilidad: responde `API Disponible`.

10) `SessionController` (ruta: `api/session`) — Gestión de sesiones / videoconsultas
- [Authorize] GET /api/session — Obtener todas las sesiones.
- [Authorize] GET /api/session/{id} — Obtener sesión por id.
- [Authorize] GET /api/session/my-sessions/future — Obtener mis sesiones futuras.
- [Authorize] GET /api/session/my-sessions/previous — Obtener mis sesiones pasadas.
- [Authorize] POST /api/session — Crear sesión (`CreateSessionDto`).
- [Authorize] PUT /api/session/{id} — Actualizar sesión (`UpdateSessionDto`).
- [Authorize] DELETE /api/session/{id} — Eliminar sesión.
- [Authorize] GET /api/session/{sessionId}/connection-data — Obtener datos seguros de conexión para videoconsulta (genera token seguro para ZEGOCLOUD). Requiere que el usuario sea participante y que sea la hora correcta según la lógica de negocio.

---

## Ejemplos de peticiones (corto)

1) Login (POST /api/auth/login)

Request body (JSON):
```json
{
  "email": "usuario@ejemplo.com",
  "password": "MiPassword123"
}
```

Respuesta (200 OK si correcto):
```json
{
  "success": true,
  "message": "...",
  "data": {
    "token": "eyJ...",
    "email": "usuario@ejemplo.com",
    "userId": 123
  }
}
```

2) Obtener sesiones de un usuario (GET /api/session/my-sessions/future)

- Debes incluir header `Authorization: Bearer {token}`; la API extrae el claim `NameIdentifier`/`sub` para identificar al usuario.

---

## Manejo de errores y códigos HTTP

- 200 OK — Operación exitosa (GET, PUT, POST cuando retorna objeto existentes o confirmación)
- 201 Created — Recurso creado (POST) con ubicación (CreatedAtAction) en algunos controladores
- 204 No Content — Usado para DELETE (en algunos lugares) — verificar si el controlador devuelve `NoContent()` o `Ok(...)`
- 400 Bad Request — Validaciones o `InvalidOperationException`/`ArgumentException`
- 401 Unauthorized — Token inválido o no provisto
- 403 Forbidden — Acceso denegado (roles o permisos)
- 404 Not Found — Recurso no encontrado
- 500 Internal Server Error — Excepción no manejada

Los controladores devuelven JSON con una estructura consistente: al menos `success` booleano y `message` y/o `data`.

---

## Buenas prácticas y próximos pasos (recomendaciones)

- Añadir un archivo `README.md` detallado en la carpeta del proyecto web con pasos rápidos de inicio y variables mínimas.
- Añadir integración continua que valide build y unit tests.
- Registrar y rotar secretos de ZEGOCLOUD y JWT en un secret manager (ei. Azure Key Vault, Fly.io secrets, etc.).
- Añadir ejemplos de payloads DTO (por ejemplo, dentro de `docs/samples`), y un collection de Postman / Insomnia.
- Añadir tests unitarios e integración para los UseCases principales (Auth, Session, ProfessionalProfile).

---

## Resumen del trabajo realizado

- Inspeccioné `Program.cs` y `appsettings.json` para extraer la configuración y las variables de entorno necesarias.
- Leí los controladores en `ProConnect_Backend/Controllers` para construir el listado completo de endpoints, requisitos de autenticación y comportamiento esperado.
- Generé este archivo `DOCUMENTATION.md` en la raíz del proyecto con instrucciones, ejemplos y recomendaciones.

---

Si quieres, puedo:
- Generar un `README.md` más corto en la carpeta `ProConnect_Backend/` con comandos rápidos de arranque.
- Crear un archivo `.env.example` real en la raíz con los valores comentados.
- Generar una colección de Postman/Insomnia con ejemplos listos (export JSON).

Dime cuál de estas acciones quieres que haga a continuación y lo implemento.


