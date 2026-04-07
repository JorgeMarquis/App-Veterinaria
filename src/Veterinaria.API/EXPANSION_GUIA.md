# 🔄 CÓMO EXPANDIR LA API REST

## 📋 Plantilla para crear nuevos Controladores

Si quieres agregar más controladores (Atenciones, Productos, Servicios, etc.), sigue este patrón:

### **Paso 1: Crear DTOs**

Ejemplo para `Servicio`:

```csharp
// src/Veterinaria.API/DTOs/ServicioDTOs/ServicioDTO.cs
namespace Veterinaria.API.DTOs.ServicioDTOs;

public class ServicioDTO
{
    public int IdServicio { get; set; }
    public string Nombre { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public decimal Precio { get; set; }
    public int IdTipoServicio { get; set; }
    public bool Activo { get; set; }
}

// src/Veterinaria.API/DTOs/ServicioDTOs/CreateServicioDTO.cs
using System.ComponentModel.DataAnnotations;

namespace Veterinaria.API.DTOs.ServicioDTOs;

public class CreateServicioDTO
{
    [Required(ErrorMessage = "El nombre del servicio es requerido")]
    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [StringLength(500)]
    public string Descripcion { get; set; } = null!;

    [Required(ErrorMessage = "El precio es requerido")]
    [Range(0.01, decimal.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
    public decimal Precio { get; set; }

    [Required(ErrorMessage = "El tipo de servicio es requerido")]
    public int IdTipoServicio { get; set; }

    public bool Activo { get; set; } = true;
}

// src/Veterinaria.API/DTOs/ServicioDTOs/UpdateServicioDTO.cs
using System.ComponentModel.DataAnnotations;

namespace Veterinaria.API.DTOs.ServicioDTOs;

public class UpdateServicioDTO
{
    [Required(ErrorMessage = "El nombre del servicio es requerido")]
    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [StringLength(500)]
    public string Descripcion { get; set; } = null!;

    [Required(ErrorMessage = "El precio es requerido")]
    [Range(0.01, decimal.MaxValue)]
    public decimal Precio { get; set; }

    [Required(ErrorMessage = "El tipo de servicio es requerido")]
    public int IdTipoServicio { get; set; }

    public bool Activo { get; set; }
}
```

---

### **Paso 2: Crear Controller**

```csharp
// src/Veterinaria.API/Controllers/ServiciosController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Veterinaria.Application.Abstractions;
using Veterinaria.Domain.Entities;
using Veterinaria.API.DTOs.ServicioDTOs;

namespace Veterinaria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiciosController : ControllerBase
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<ServiciosController> _logger;

    public ServiciosController(IApplicationDbContext context, ILogger<ServiciosController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene la lista de todos los servicios
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<ServicioDTO>>> GetServicios()
    {
        try
        {
            var servicios = await _context.Servicios.ToListAsync();
            var servicioDTOs = servicios.Select(s => MapToDTO(s)).ToList();
            return Ok(servicioDTOs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener servicios");
            return StatusCode(500, new { message = "Error al obtener los servicios", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene un servicio específico por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ServicioDTO>> GetServicio(int id)
    {
        try
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null)
                return NotFound(new { message = $"Servicio con ID {id} no encontrado" });

            return Ok(MapToDTO(servicio));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener servicio {ServicioId}", id);
            return StatusCode(500, new { message = "Error al obtener el servicio", error = ex.Message });
        }
    }

    /// <summary>
    /// Crea un nuevo servicio
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ServicioDTO>> CreateServicio(CreateServicioDTO createDTO)
    {
        try
        {
            // Validar que el tipo de servicio existe
            var tipoExists = await _context.TipoServicios.AnyAsync(t => t.IdTipoServicio == createDTO.IdTipoServicio);
            if (!tipoExists)
                return BadRequest(new { message = "El tipo de servicio especificado no existe" });

            var servicio = new Servicio
            {
                Nombre = createDTO.Nombre,
                Descripcion = createDTO.Descripcion,
                Precio = createDTO.Precio,
                IdTipoServicio = createDTO.IdTipoServicio,
                Activo = createDTO.Activo
            };

            _context.Add(servicio);
            await _context.SaveChangesAsync();

            var servicioDTO = MapToDTO(servicio);
            return CreatedAtAction(nameof(GetServicio), new { id = servicio.IdServicio }, servicioDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear servicio");
            return StatusCode(500, new { message = "Error al crear el servicio", error = ex.Message });
        }
    }

    /// <summary>
    /// Actualiza un servicio existente
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateServicio(int id, UpdateServicioDTO updateDTO)
    {
        try
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null)
                return NotFound(new { message = $"Servicio con ID {id} no encontrado" });

            servicio.Nombre = updateDTO.Nombre;
            servicio.Descripcion = updateDTO.Descripcion;
            servicio.Precio = updateDTO.Precio;
            servicio.IdTipoServicio = updateDTO.IdTipoServicio;
            servicio.Activo = updateDTO.Activo;

            _context.Update(servicio);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar servicio {ServicioId}", id);
            return StatusCode(500, new { message = "Error al actualizar el servicio", error = ex.Message });
        }
    }

    /// <summary>
    /// Elimina un servicio
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteServicio(int id)
    {
        try
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null)
                return NotFound(new { message = $"Servicio con ID {id} no encontrado" });

            _context.Servicios.Remove(servicio);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar servicio {ServicioId}", id);
            return StatusCode(500, new { message = "Error al eliminar el servicio", error = ex.Message });
        }
    }

    private static ServicioDTO MapToDTO(Servicio servicio)
    {
        return new ServicioDTO
        {
            IdServicio = servicio.IdServicio,
            Nombre = servicio.Nombre,
            Descripcion = servicio.Descripcion,
            Precio = servicio.Precio,
            IdTipoServicio = servicio.IdTipoServicio,
            Activo = servicio.Activo
        };
    }
}
```

---

## 🔍 Controladores para Crear Próximamente

### **1. AtencionsController**
```csharp
GET    /api/atencions
GET    /api/atencions/{id}
GET    /api/atencions/mascota/{idMascota}
POST   /api/atencions
PUT    /api/atencions/{id}
DELETE /api/atencions/{id}
```

### **2. ProductosController**
```csharp
GET    /api/productos
GET    /api/productos/{id}
GET    /api/productos/categoria/{idCategoria}
POST   /api/productos
PUT    /api/productos/{id}
DELETE /api/productos/{id}
```

### **3. ComprasController**
```csharp
GET    /api/compras
GET    /api/compras/{id}
POST   /api/compras
PUT    /api/compras/{id}
DELETE /api/compras/{id}
```

### **4. VentasController**
```csharp
GET    /api/ventas
GET    /api/ventas/{id}
GET    /api/ventas/cliente/{idCliente}
POST   /api/ventas
PUT    /api/ventas/{id}
DELETE /api/ventas/{id}
```

### **5. ProveedoresController**
```csharp
GET    /api/proveedores
GET    /api/proveedores/{id}
POST   /api/proveedores
PUT    /api/proveedores/{id}
DELETE /api/proveedores/{id}
```

---

## 🔐 Agregar Autenticación JWT

### **Paso 1: Instalar paquetes**
```bash
dotnet add package System.IdentityModel.Tokens.Jwt
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
```

### **Paso 2: Configurar en Program.cs**
```csharp
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!)),
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateLifetime = true
        };
    });

app.UseAuthentication();
```

### **Paso 3: Proteger endpoints**
```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]  // ← Requerir autenticación
public class ClientesController : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]  // ← Excepciones
    public async Task<ActionResult<List<ClienteDTO>>> GetClientes()
    {
        // ...
    }
}
```

---

## 📊 Agregar Paginación

```csharp
// DTOs
public class PaginationParams
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}

// En Controller
[HttpGet]
public async Task<ActionResult<PagedResult<ClienteDTO>>> GetClientes(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
{
    var query = _context.Clientes;
    var totalCount = await query.CountAsync();
    
    var clientes = await query
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    var result = new PagedResult<ClienteDTO>
    {
        Items = clientes.Select(c => MapToDTO(c)).ToList(),
        TotalCount = totalCount,
        PageNumber = pageNumber,
        PageSize = pageSize
    };

    return Ok(result);
}
```

---

## 🧪 Unit Testing

```csharp
// Tests/ClientesControllerTests.cs
using Xunit;
using Moq;
using Veterinaria.API.Controllers;
using Veterinaria.Application.Abstractions;
using Veterinaria.Domain.Entities;

public class ClientesControllerTests
{
    private readonly Mock<IApplicationDbContext> _mockContext;
    private readonly ClientesController _controller;

    public ClientesControllerTests()
    {
        _mockContext = new Mock<IApplicationDbContext>();
        var mockLogger = new Mock<ILogger<ClientesController>>();
        _controller = new ClientesController(_mockContext.Object, mockLogger.Object);
    }

    [Fact]
    public async Task GetCliente_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var clienteId = 1;
        var cliente = new Cliente 
        { 
            IdCliente = clienteId, 
            NombreCompleto = "Test Cliente" 
        };

        _mockContext.Setup(c => c.Clientes.FindAsync(clienteId))
            .ReturnsAsync(cliente);

        // Act
        var result = await _controller.GetCliente(clienteId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task GetCliente_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        _mockContext.Setup(c => c.Clientes.FindAsync(It.IsAny<int>()))
            .ReturnsAsync((Cliente?)null);

        // Act
        var result = await _controller.GetCliente(999);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
}
```

---

## 📝 Checklist de Expansión

- [ ] Agregar ServiciosController
- [ ] Agregar AtencionsController
- [ ] Agregar ProductosController
- [ ] Agregar ComprasController
- [ ] Agregar VentasController
- [ ] Implementar Autenticación JWT
- [ ] Agregar Paginación a todos los GET
- [ ] Implementar Filtrado avanzado
- [ ] Agregar Unit Tests
- [ ] Agregar Integration Tests
- [ ] Configurar Rate Limiting
- [ ] Implementar Caching con Redis
- [ ] Agregar Versionado de API (v2, v3)
- [ ] Documentar en OpenAPI/Swagger
- [ ] Crear Postman Collection

---

**¡Sigue este patrón y tu API seguirá creciendo profesionalmente! 🚀**
