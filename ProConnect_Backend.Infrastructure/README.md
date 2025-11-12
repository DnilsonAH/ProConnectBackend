# ⚙️ Infrastructure Layer (Capa de Infraestructura)

## 📋 Descripción General

La **capa de infraestructura** contiene las implementaciones técnicas de las interfaces definidas en el Domain. Esta capa:

- ✅ **Implementa interfaces del Domain** (IUserRepository, IPasswordHasher, etc.)
- ✅ Contiene detalles técnicos (EF Core, BCrypt, JWT, etc.)
- ✅ Maneja **acceso a datos** (base de datos)
- ✅ Implementa **servicios externos** (autenticación, hashing, etc.)
- ✅ Configura **Entity Framework Core** con Fluent API

**Dependencias**: Domain Layer + Application Layer + paquetes NuGet técnicos

---

## 📁 Estructura de Carpetas

### 🗂️ **`Data/`**
**Propósito**: Configuración de Entity Framework Core

**Contenido**:
- `ProConnectDbContext.cs` - Contexto principal de EF Core
- `Configurations/` - Configuraciones Fluent API de entidades

---

#### **`ProConnectDbContext.cs`**
**Propósito**: Contexto de base de datos usando EF Core

**Características**:
- ✅ Hereda de `DbContext`
- ✅ Define `DbSet<>` para cada entidad
- ✅ Aplica configuraciones Fluent API automáticamente
- ✅ Configurado para MySQL con Pomelo

**Ejemplo**:
```csharp
public class ProConnectDbContext : DbContext
{
    public ProConnectDbContext(DbContextOptions<ProConnectDbContext> options) 
        : base(options) { }

    // DbSets (tablas)
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Session> Sessions { get; set; }
    public virtual DbSet<JwtBlacklist> JwtBlacklists { get; set; }
    // ... 10 DbSets más

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplica todas las configuraciones automáticamente
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
```

---

#### **`Configurations/` (Fluent API)**
**Propósito**: Configuración de entidades sin contaminar el Domain

**Contenido actual**: 13 archivos de configuración
- `UserConfiguration.cs`
- `SessionConfiguration.cs`
- `PaymentConfiguration.cs`
- `ReviewConfiguration.cs`
- `JwtBlacklistConfiguration.cs`
- `ProfessionalProfileConfiguration.cs`
- `ProfessionConfiguration.cs`
- `ProfessionCategoryConfiguration.cs`
- `SpecializationConfiguration.cs`
- `VerificationConfiguration.cs`
- `VerificationDocumentConfiguration.cs`
- `WeeklyAvailabilityConfiguration.cs`
- `ScheduledConfiguration.cs`

**Características**:
- ✅ Implementan `IEntityTypeConfiguration<TEntity>`
- ✅ Configuran claves primarias, índices, relaciones
- ✅ Mantienen el Domain libre de anotaciones de EF Core
- ✅ Centralizan la configuración de persistencia

**Ejemplo**: `UserConfiguration.cs`
```csharp
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Tabla
        builder.ToTable("users");
        
        // Clave primaria
        builder.HasKey(e => e.UserId).HasName("PRIMARY");
        builder.Property(e => e.UserId).HasColumnName("user_id");
        
        // Propiedades
        builder.Property(e => e.FirstName)
            .HasMaxLength(50)
            .HasColumnName("first_name");
            
        builder.Property(e => e.Email)
            .HasMaxLength(255)
            .HasColumnName("email");
        
        // Índices
        builder.HasIndex(e => e.Email, "users_email_unique").IsUnique();
        
        // Relaciones
        builder.HasMany(u => u.JwtBlacklists)
            .WithOne(j => j.User)
            .HasForeignKey(j => j.UserId);
    }
}
```

**Ventajas**:
- ✅ Domain permanece puro (sin `[Key]`, `[Column]`, etc.)
- ✅ Toda la configuración de BD está en Infrastructure
- ✅ Fácil de mantener y encontrar

---

### 🗂️ **`Adapters/Repositories/`**
**Propósito**: Implementaciones concretas de los repositorios

**Contenido actual**: 14 clases
- `GenericRepository<TEntity>` - Repositorio base
- `UserRepository` - Implementa IUserRepository
- `JwtBlacklistRepository` - Implementa IJwtBlacklistRepository
- `SessionRepository` - Implementa ISessionRepository
- ... (11 repositorios más)

---

#### **`GenericRepository<TEntity>`**
**Propósito**: Implementación base con operaciones CRUD comunes

**Características**:
- ✅ Operaciones básicas: GetByIdAsync, GetAllAsync, AddAsync, Update, Delete
- ✅ Usado como base por repositorios específicos
- ✅ Usa EF Core internamente

**Ejemplo**:
```csharp
public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    protected readonly ProConnectDbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;

    public GenericRepository(ProConnectDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(uint id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }
}
```

---

#### **Repositorios Específicos**
**Ejemplo**: `UserRepository.cs`

```csharp
public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(ProConnectDbContext dbContext) : base(dbContext) { }

    // Métodos específicos de negocio
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _dbContext.Users
            .AnyAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User>> GetUsersByRoleAsync(string role)
    {
        return await _dbContext.Users
            .Where(u => u.Role == role)
            .ToListAsync();
    }

    public async Task<User?> GetUserWithProfilesAsync(uint userId)
    {
        return await _dbContext.Users
            .Include(u => u.ProfessionalProfiles)
            .FirstOrDefaultAsync(u => u.UserId == userId);
    }
}
```

**Características**:
- ✅ Heredan de `GenericRepository<T>` (operaciones base)
- ✅ Implementan métodos específicos definidos en la interface del Domain
- ✅ Usan LINQ y EF Core para queries
- ✅ Pueden incluir relaciones con `.Include()`

---

### 🗂️ **`Adapters/UnitOfWork.cs`**
**Propósito**: Implementa el patrón Unit of Work

**Características**:
- ✅ Agrupa todos los repositorios
- ✅ Gestiona transacciones
- ✅ Garantiza que SaveChanges afecte a todas las operaciones

**Ejemplo**:
```csharp
public class UnitOfWork : IUnitOfWork
{
    private readonly ProConnectDbContext _dbContext;
    
    public IUserRepository UserRepository { get; }
    public IJwtBlacklistRepository JwtBlacklistRepository { get; }
    // ... 11 repositorios más

    public UnitOfWork(
        ProConnectDbContext dbContext,
        IUserRepository userRepository,
        IJwtBlacklistRepository jwtBlacklistRepository,
        // ... todos los repositorios
    )
    {
        _dbContext = dbContext;
        UserRepository = userRepository;
        JwtBlacklistRepository = jwtBlacklistRepository;
        // ... asignación de todos
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}
```

**Ventajas**:
- ✅ Una sola llamada a `SaveChanges()` para múltiples operaciones
- ✅ Transacciones automáticas
- ✅ Punto único de acceso a repositorios

---

### 🗂️ **`Services/`**
**Propósito**: Implementaciones de servicios técnicos

**Contenido actual**:
- `PasswordHasher.cs` - Implementa IPasswordHasher usando BCrypt
- `JwtTokenService.cs` - Implementa IJwtTokenService usando System.IdentityModel.Tokens.Jwt

---

#### **`PasswordHasher.cs`**
**Propósito**: Implementa hashing de contraseñas con BCrypt

**Ejemplo**:
```csharp
public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
```

**Características**:
- ✅ Usa BCrypt.Net-Next
- ✅ Genera salt automáticamente
- ✅ Implementa IPasswordHasher del Domain

---

#### **`JwtTokenService.cs`**
**Propósito**: Implementa generación y validación de JWT

**Ejemplo**:
```csharp
public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;

    public string GenerateToken(uint userId, string email, string role)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(72),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool ValidateToken(string token) { /* ... */ }
    
    public (string jti, uint userId, DateTime expiresAt)? ParseToken(string token) { /* ... */ }
}
```

**Características**:
- ✅ Usa System.IdentityModel.Tokens.Jwt
- ✅ Genera tokens con claims personalizados
- ✅ Valida y parsea tokens
- ✅ Implementa IJwtTokenService del Domain

---

## 📦 Dependencias (Paquetes NuGet)

**Persistencia**:
- `Microsoft.EntityFrameworkCore` (9.0.10)
- `Microsoft.EntityFrameworkCore.Relational` (9.0.10)
- `Microsoft.EntityFrameworkCore.Tools` (9.0.10)
- `Microsoft.EntityFrameworkCore.Design` (9.0.10)
- `Pomelo.EntityFrameworkCore.MySql` (9.0.0) - Proveedor MySQL

**Seguridad**:
- `BCrypt.Net-Next` (4.0.3) - Hashing de contraseñas
- `System.IdentityModel.Tokens.Jwt` (8.2.1) - Tokens JWT
- `Microsoft.AspNetCore.Authentication.JwtBearer` (9.0.0) - Middleware JWT

**Referencias de Proyecto**:
- `ProConnect_Backend.Domain`
- `ProConnect_Backend.Application`

---

## 🔄 Relación con Otras Capas

```
┌─────────────────────────────────────┐
│         DOMAIN LAYER                │
│   (Interfaces)                      │
└─────────────────────────────────────┘
          ↑
          │ implementa
          │
┌─────────────────────────────────────┐
│    INFRASTRUCTURE LAYER             │
│                                     │
│   - DbContext (EF Core)             │
│   - Repositories (implementaciones) │
│   - UnitOfWork                      │
│   - Services (BCrypt, JWT)          │
│   - Fluent API Configurations       │
└─────────────────────────────────────┘
          ↓
          │ conecta a
          │
┌─────────────────────────────────────┐
│    MYSQL DATABASE                   │
│   (Google Cloud SQL)                │
└─────────────────────────────────────┘
```

---

## 🎯 Principios de la Capa de Infraestructura

### ✅ **Implementa, no Define**
```csharp
// Domain define:
public interface IUserRepository { ... }

// Infrastructure implementa:
public class UserRepository : IUserRepository { ... }
```

### ✅ **Detalles Técnicos Encapsulados**
- EF Core, BCrypt, JWT están SOLO aquí
- Application/Domain no conocen estas tecnologías
- Permite cambiar sin afectar otras capas

### ✅ **Configuración Centralizada**
- Fluent API en vez de Data Annotations
- Domain permanece limpio
- Fácil de mantener

### ✅ **Connection String desde Environment**
```csharp
// Lee desde .env o variables de entorno
var connectionString = $"Server={dbServer};Port={dbPort};Database={dbDatabase};...";
```

---

## 🚀 Buenas Prácticas

1. ✅ **Fluent API en vez de Data Annotations**
2. ✅ **GenericRepository para operaciones comunes**
3. ✅ **Repositorios específicos para lógica compleja**
4. ✅ **Unit of Work para transacciones**
5. ✅ **Servicios para lógica técnica** (hashing, tokens)
6. ✅ **No exponer DbContext fuera de Infrastructure**
7. ✅ **Usar async/await para operaciones de BD**
8. ✅ **Configuración desde variables de entorno**

---

**Última actualización**: Noviembre 2025  
**Conexión**: Google Cloud SQL MySQL 8.0.41 con SSL
