# 🔍 GUÍA RÁPIDA PARA DESARROLLADORES

## ⚡ Preguntas Frecuentes

### 🤔 ¿Dónde coloco X?

| Elemento | Ubicación | Ejemplo |
|----------|-----------|---------|
| **Entidades** | `Veterinaria.Domain/Entities/` | `Cliente.cs` |
| **DTOs** | `Veterinaria.Application/DTOs/` | `ClienteDTO.cs` |
| **Services** | `Veterinaria.Application/Services/` | `ClienteService.cs` |
| **Interfaces de Services** | `Veterinaria.Application/Interfaces/` | `IClienteService.cs` |
| **DbContext** | `Veterinaria.Infrastructure/Persistence/` | `BDVeterinariaContext.cs` |
| **Repositories** | `Veterinaria.Infrastructure/Repositories/` | `ClienteRepository.cs` |
| **Controllers** | `Veterinaria.API/Controllers/` | `ClientesController.cs` |
| **Exceptions** | `Veterinaria.Shared/Exceptions/` | `NotFoundException.cs` |
| **Constantes** | `Veterinaria.Shared/Constants/` | `ApiConstants.cs` |
| **Tests Unitarios** | `tests/Veterinaria.Tests.Unit/` | `ClienteServiceTests.cs` |
| **Tests Integración** | `tests/Veterinaria.Tests.Integration/` | `ClientesControllerTests.cs` |

---

## 🏗️ Patrón de Creación de Nuevo Feature

### Paso 1: Entidad (Domain)
```csharp
// src/Veterinaria.Domain/Entities/Producto.cs
public class Producto
{
    public int IdProducto { get; set; }
    public string Nombre { get; set; }
    // ... propiedades
}
```

### Paso 2: DTOs (Application)
```csharp
// src/Veterinaria.Application/DTOs/ProductoDTOs/ProductoDTO.cs
public class ProductoDTO
{
    public int IdProducto { get; set; }
    public string Nombre { get; set; }
}

// src/Veterinaria.Application/DTOs/ProductoDTOs/CreateProductoDTO.cs
public class CreateProductoDTO
{
    [Required]
    public string Nombre { get; set; }
}
```

### Paso 3: Service Interface (Application)
```csharp
// src/Veterinaria.Application/Interfaces/IProductoService.cs
public interface IProductoService
{
    Task<ProductoDTO> GetProductoAsync(int id);
    Task<IEnumerable<ProductoDTO>> GetProductosAsync();
    Task<ProductoDTO> CreateProductoAsync(CreateProductoDTO createDTO);
}
```

### Paso 4: Service Implementation (Application)
```csharp
// src/Veterinaria.Application/Services/ProductoService.cs
public class ProductoService : IProductoService
{
    private readonly IApplicationDbContext _context;

    public ProductoService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductoDTO> GetProductoAsync(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto == null)
            throw new NotFoundException(nameof(Producto), id);
        
        return MapToDTO(producto);
    }

    // ... otros métodos
}
```

### Paso 5: Controller (API)
```csharp
// src/Veterinaria.API/Controllers/ProductosController.cs
[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _service;
    private readonly ILogger<ProductosController> _logger;

    public ProductosController(IProductoService service, ILogger<ProductosController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductoDTO>> GetProducto(int id)
    {
        try
        {
            var producto = await _service.GetProductoAsync(id);
            return Ok(producto);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Producto no encontrado");
            return NotFound(new { message = ex.Message });
        }
    }

    // ... otros endpoints
}
```

### Paso 6: Dependency Injection (Application)
```csharp
// src/Veterinaria.Application/DependencyInjection.cs
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IProductoService, ProductoService>();
        // ... otros servicios
        return services;
    }
}
```

### Paso 7: Tests (Tests.Unit)
```csharp
// tests/Veterinaria.Tests.Unit/Services/ProductoServiceTests.cs
public class ProductoServiceTests
{
    [Fact]
    public async Task GetProductoAsync_WithValidId_ReturnsProductoDTO()
    {
        // Arrange
        var mockContext = new Mock<IApplicationDbContext>();
        var service = new ProductoService(mockContext.Object);

        // Act
        var result = await service.GetProductoAsync(1);

        // Assert
        Assert.NotNull(result);
    }
}
```

---

## 📦 Estructura de Carpetas por Feature

Para un nuevo módulo (ej: Citas), la estructura sería:

```
Domain:
  Entities/
    └── Cita.cs

Application:
  DTOs/CitaDTOs/
    ├── CitaDTO.cs
    ├── CreateCitaDTO.cs
    └── UpdateCitaDTO.cs
  Services/
    └── CitaService.cs
  Interfaces/
    └── ICitaService.cs

Infrastructure:
  Persistence/
    └── Migrations/
       └── [migración para tabla Citas]
  Repositories/
    └── CitaRepository.cs (futuro)

API:
  Controllers/
    └── CitasController.cs

Tests.Unit:
  Services/
    └── CitaServiceTests.cs

Tests.Integration:
  API/
    └── CitasControllerTests.cs
```

---

## 🚀 Comandos Comunes

### Compilación
```bash
# Compilar solución completa
dotnet build App-Veterinaria.sln

# Compilar solo un proyecto
dotnet build src/Veterinaria.API/Veterinaria.API.csproj

# Compilar sin restore
dotnet build --no-restore
```

### Ejecutar
```bash
# Ejecutar API
dotnet run --project src/Veterinaria.API/

# Ejecutar con hot reload
dotnet watch run --project src/Veterinaria.API/
```

### Testing
```bash
# Correr todos los tests
dotnet test tests/

# Correr tests específicos
dotnet test tests/Veterinaria.Tests.Unit/

# Con coverage
dotnet test /p:CollectCoverage=true
```

### Entity Framework
```bash
# Crear migration
dotnet ef migrations add InitialCreate -p src/Veterinaria.Infrastructure/

# Aplicar migration
dotnet ef database update -p src/Veterinaria.Infrastructure/
```

---

## ✅ Checklist para Código Nuevo

Antes de hacer commit, verifica:

- [ ] ¿Compilación sin errores?
  ```bash
  dotnet build App-Veterinaria.sln
  ```

- [ ] ¿Tests pasando?
  ```bash
  dotnet test tests/
  ```

- [ ] ¿Namespaces correctos?
  - Domain: `Veterinaria.Domain.Entities`
  - Application: `Veterinaria.Application.{DTOs|Services|Interfaces}`
  - API: `Veterinaria.API.Controllers`

- [ ] ¿Excepciones de Shared?
  - ✅ `throw new NotFoundException(...)`
  - ✅ `throw new ValidationException(...)`
  - ❌ `throw new Exception(...)`

- [ ] ¿DTOs validados?
  ```csharp
  [Required]
  [StringLength(100)]
  public string Nombre { get; set; }
  ```

- [ ] ¿Logging en Controllers?
  ```csharp
  _logger.LogError(ex, "Error al obtener recurso");
  ```

- [ ] ¿Respuestas HTTP correctas?
  - 200: OK
  - 201: Created
  - 400: Bad Request
  - 404: Not Found
  - 500: Internal Server Error

- [ ] ¿Inyección de dependencias?
  ```csharp
  // ✅ BIEN
  public class Service(IApplicationDbContext context) { }
  
  // ❌ MALO
  new IApplicationDbContext();
  ```

---

## 🐛 Troubleshooting Común

### Error: "No se encontró el archivo de metadatos"
```bash
# Solución:
dotnet clean App-Veterinaria.sln
dotnet build App-Veterinaria.sln
```

### Error: "El tipo no existe en el espacio de nombres"
- Verifica que agregaste la `using` correcta
- Verifica que el proyecto está referenciado en `.csproj`
- Verifica que compilaste el proyecto que contiene la clase

### Error: "Cannot convert from X to Y"
- Los DTOs deben mapearse a/desde Entidades
- No puedes pasar Entidades directamente al cliente
- Usa los DTOs apropiados para cada operación

### Build lento
```bash
# Compilar en paralelo
dotnet build -m:8

# Compilar solo cambios
dotnet build --incremental
```

---

## 📚 Referencias Útiles

- **Arquitectura:** Ver `docs/ARQUITECTURA.md`
- **Plan de Acción:** Ver `PLAN_ACCION.md`
- **Cambios:** Ver `RESUMEN_CAMBIOS.md`
- **Estructura:** Ver `ESTRUCTURA_FINAL.md`
- **API REST:** Ver `src/Veterinaria.API/README.md`

---

## 🎓 Patrones a Seguir

### ✅ Repository Pattern (Futuro)
```csharp
public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
}
```

### ✅ Unit of Work Pattern (Futuro)
```csharp
public interface IUnitOfWork
{
    IRepository<Cliente> Clientes { get; }
    Task<int> SaveChangesAsync();
}
```

### ✅ Specification Pattern (Futuro)
```csharp
public class ClientesConMascotasSpec : Specification<Cliente>
{
    public ClientesConMascotasSpec()
    {
        AddInclude(c => c.Mascotas);
        AddOrderBy(c => c.Nombre);
    }
}
```

---

## 🚨 Anti-Patrones a Evitar

```csharp
// ❌ NO HAGAS ESTO:

// 1. Acceder a BD directamente desde Controller
_dbContext.Clientes.ToList();

// 2. Pasar entidades al cliente
return Ok(cliente);  // Debes usar DTO

// 3. Lógica de negocio en Controller
if (cliente.Deuda > 1000) { ... }  // Va en Service

// 4. Ignorar excepciones
try { ... } catch { }  // Nunca silencies errores

// 5. Crear instancias sin inyección
var service = new ClienteService();

// 6. Código duplicado
if (x == null) throw new NotFoundException();
if (y == null) throw new NotFoundException();
// Haz un método helper

// 7. Excepciones genéricas
throw new Exception("Error");  // Usa NotFoundException, etc.
```

---

**Última actualización:** Diciembre 2024
**Versión:** 1.0 - Guía Rápida
**Mantenido por:** Team de Desarrollo
