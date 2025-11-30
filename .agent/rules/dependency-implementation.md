---
trigger: model_decision
description: Cuando se requiera añadir una dependencia a cualquier capa del Clean Architecture
---

Prompt para agregar la dependencia **[NOMBRE_PAQUETE]** para **[PROPÓSITO]**.

**Contexto del uso:**

- [Descripción de para qué se usará]
- [Ejemplos: envío de emails, almacenamiento en caché, procesamiento de imágenes, etc.]

Por favor, analiza en qué capa(s) del **Clean Architecture** debe instalarse según estas reglas:

---

### **Análisis de Capa**

#### 1. **Domain Layer** (`ProConnect_Backend.Domain`)

- **Instalar aquí SI:** Es una abstracción pura, interfaces o DTOs sin dependencias externas.
- **NO instalar SI:** Tiene dependencias de infraestructura (bases de datos, frameworks, librerías externas).
- **Ejemplo:** `FluentValidation` (validaciones), `MediatR.Contracts` (solo interfaces).

#### 2. **Application Layer** (`ProConnect_Backend.Application`)

- **Instalar aquí SI:** Orquesta lógica de negocio sin tocar infraestructura directamente.
- **NO instalar SI:** Requiere acceso directo a BD, HTTP, archivos o servicios externos.
- **Ejemplo:** `AutoMapper`, `MediatR`, `FluentValidation`.

#### 3. **Infrastructure Layer** (`ProConnect_Backend.Infrastructure`)

- **Instalar aquí SI:** Interactúa con servicios externos, BD, archivos, APIs o librerías técnicas.
- **Siempre instalar implementaciones concretas aquí.**
- **Ejemplo:** `EF Core`, `Dapper`, `SendGrid`, `AWS SDK`, `StackExchange.Redis`, `Serilog`.

#### 4. **API/Web Layer** (`ProConnect_Backend`)

- **Instalar aquí SI:** Es específico de ASP.NET Core, middleware, Swagger, etc.
- **Ejemplo:** `Swashbuckle`, `JWT Bearer`, `CORS`.

---

### **Después del análisis, realiza:**

#### 1. Instalar el paquete en la(s) capa(s) correcta(s):

```bash
dotnet add [Proyecto]/[Proyecto].csproj package [Paquete] --version [Versión]
```

---

#### 2. Si crea nuevos servicios:

- Crea la **interface** en `Domain/Ports/IServices/`
- Crea la **implementación** en `Infrastructure/Services/`
- Registra en `ServiceRegistrationExtensions.cs`

---

#### 3. Si modifica configuración:

- Actualiza `appsettings.json` si requiere configuración.
- Actualiza `.env` si son secretos.
- Documenta variables de entorno nuevas.

---

#### 4. Valida que no rompa Clean Architecture:

- `Domain` **NO** debe tener dependencias de `Infrastructure`.
- `Application` **NO** debe conocer implementaciones concretas.
- `Infrastructure` **implementa interfaces del Domain.**

---

#### 5. Compila y verifica:

```bash
dotnet build ProConnect_Backend.sln
```
