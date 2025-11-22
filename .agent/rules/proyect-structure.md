---
trigger: always_on
---

# Clean Architecture - Documentación de Estructura

## Arquitectura General

Este proyecto implementa **Clean Architecture** (Arquitectura Limpia) propuesta por Robert C. Martin (Uncle Bob), organizando el código en 4 capas concéntricas donde las dependencias fluyen **únicamente hacia adentro**, garantizando que el núcleo del negocio permanezca independiente de frameworks, UI o bases de datos.

```
┌──────────────────────────────────────────────────────────────────┐
│                    ProConnect_Backend (API)                    │
│                          Presentation Layer                       │
│  Controllers • Middleware • Program.cs • appsettings.json         │
│  ┌────────────────────────────────────────────────────────────┐  │
│  │          ProConnect_Backend.Application                  │  │
│  │                   Application Layer                         │  │
│  │  UseCases (CQRS) • Handlers • DTOs Response • AutoMapper   │  │
│  │  ┌──────────────────────────────────────────────────────┐  │  │
│  │  │       ProConnect_Backend.Domain                   │  │  │
│  │  │             Domain Layer (CORE)                       │  │  │
│  │  │  Entities • Ports (Interfaces) • DTOs Request         │  │  │
│  │  └──────────────────────────────────────────────────────┘  │  │
│  │                                                              │  │
│  │  ProConnect_Backend.Infrastructure                       │  │
│  │           Infrastructure Layer                               │  │
│  │  Repositories • DbContext • Services • Configurations        │  │
│  └────────────────────────────────────────────────────────────┘  │
└──────────────────────────────────────────────────────────────────┘
```

### Flujo de Dependencias

```
API → Application → Domain ← Infrastructure
     ↓                ↑              ↓
     └───────────────────────────────┘
```

## **Regla de Oro**: Las capas externas pueden depender de las internas, pero **NUNCA al revés**.

Este proyecto sigue estrictamente los principios de Clean Architecture:

- **Independencia de frameworks**: Domain no conoce EF Core ni ASP.NET
- **Testeable**: Lógica de negocio completamente aislada
- **Independencia de UI**: API es intercambiable
- **Independencia de DB**: Repositories ocultan implementación MySQL
- **Separación de responsabilidades**: Cada capa tiene un propósito único

**Puerto de desarrollo**: http://localhost:5200  
**Swagger UI**: http://localhost:5200/swagger

---
