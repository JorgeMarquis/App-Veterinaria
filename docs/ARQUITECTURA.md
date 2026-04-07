# 🏗️ Arquitectura del Proyecto - Veterinaria

## 📋 Índice
1. [Visión General](#visión-general)
2. [Estructura de Carpetas](#estructura-de-carpetas)
3. [Capas de la Arquitectura](#capas-de-la-arquitectura)
4. [Dependencias entre Proyectos](#dependencias-entre-proyectos)
5. [Patrones Implementados](#patrones-implementados)
6. [Mejores Prácticas](#mejores-prácticas)

---

## 🎯 Visión General

El proyecto sigue una **Clean Architecture** con **5 capas** claramente separadas:

```
┌─────────────────────────────────────┐
│     Veterinaria.API (REST)          │  ← Presentación (Controllers, DTOs)
├─────────────────────────────────────┤
│  Veterinaria.Application (Lógica)   │  ← Casos de uso (Services, DTOs)
├─────────────────────────────────────┤
│  Veterinaria.Infrastructure (Datos) │  ← Detalles técnicos (BD, EF Core)
├─────────────────────────────────────┤
│   Veterinaria.Domain (Entidades)    │  ← Modelos de negocio puros
├─────────────────────────────────────┤
│    Veterinaria.Shared (Común)       │  ← Excepciones, Constantes
└─────────────────────────────────────┘
```

---

## 📁 Estructura de Carpetas

```
App-Veterinaria/
│
├── src/
│   ├── Veterinaria.Domain/
│   │   └── Entities/                 [Modelos puros sin lógica de negocio]
│   │       ├── Cliente.cs
│   │       ├── Mascota.cs
│   │       ├── Vacuna.cs
│   │       └── ... (otras entidades)
│   │
│   ├── Veterinaria.Application/
│   │   ├── DTOs/                     [Objetos de transferencia de datos]
│   │   │   ├── ClienteDTOs/
│   │   │   ├── MascotaDTOs/
│   │   │   └── VacunaDTOs/
│   │   ├── Services/                 [Lógica de negocio]
│   │   ├── Interfaces/               [Contratos]
│   │   ├── Exceptions/               [Excepciones de negocio]
│   │   └── DependencyInjection.cs
│   │
│   ├── Veterinaria.Infrastructure/
│   │   ├── Persistence/              [Acceso a datos]
│   │   │   └── BDVeterinariaContext.cs
│   │   ├── Repositories/             [Patrón Repository (futuro)]
│   │   └── DependencyInjection.cs
│   │
│   ├── Veterinaria.API/
│   │   ├── Controllers/              [Endpoints REST]
│   │   ├── Middleware/               [Manejo de solicitudes]
│   │   ├── Extensions/               [Configuración]
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   └── appsettings.Development.json
│   │
│   ├── Veterinaria.Web/              [Presentación web (futuro)]
│   │
│   └── Veterinaria.Shared/
│       ├── Constants/                [Constantes globales]
│       ├── Exceptions/               [Excepciones compartidas]
│       ├── Extensions/               [Métodos de extensión]
│       └── Utilities/                [Funciones auxiliares]
│
├── tests/
│   ├── Veterinaria.Tests.Unit/
│   │   ├── Application/
│   │   ├── Domain/
│   │   └── ... (tests unitarios)
│   │
│   └── Veterinaria.Tests.Integration/
│       ├── API/
│       └── ... (tests de integración)
│
├── docs/
│   ├── ARQUITECTURA.md               [Este archivo]
│   ├── SETUP.md                      [Configuración inicial]
│   └── API.md                        [Documentación de API]
│
└── App-Veterinaria.sln
```

---

## 🏛️ Capas de la Arquitectura

### 1. **Veterinaria.Domain** (Capa de Dominio)
**Responsabilidad:** Modelos puros de negocio

- ✅ Contiene entidades del dominio (Cliente, Mascota, Vacuna, etc.)
- ✅ NO tiene dependencias de otras capas
- ✅ Representa la lógica de negocio pura
- ❌ NO accede a BD
- ❌ NO tiene referencias a HTTP

**Ejemplo:**
```csharp
public class Cliente
{
    public int IdCliente { get; set; }
    public string NombreCompleto { get; set; }
    // ... propiedades
}
```

---

### 2. **Veterinaria.Application** (Capa de Aplicación)
**Responsabilidad:** Lógica de casos de uso y coordinación

- ✅ Contiene DTOs para transferencia de datos
- ✅ Contiene interfaces de servicios
- ✅ Contiene excepciones de negocio
- ✅ Depende de Domain
- ❌ NO accede directamente a BD (usa abstracciones)
- ❌ NO tiene referencias a HTTP

**Estructura:**
```
DTOs/
  ├── ClienteDTOs/
  │   ├── ClienteDTO.cs              (lectura)
  │   ├── CreateClienteDTO.cs        (creación)
  │   └── UpdateClienteDTO.cs        (actualización)
  ├── MascotaDTOs/
  └── VacunaDTOs/

Services/
  ├── IClienteService.cs
  └── ClienteService.cs

Abstractions/
  └── IApplicationDbContext.cs
```

---

### 3. **Veterinaria.Infrastructure** (Capa de Infraestructura)
**Responsabilidad:** Detalles técnicos de implementación

- ✅ Acceso a base de datos con Entity Framework Core
- ✅ Implementa las interfaces de Application
- ✅ Configuración de contexto de BD
- ✅ Depende de Application y Domain
- ❌ NO tiene referencias a HTTP/API

**Estructura:**
```
Persistence/
  └── BDVeterinariaContext.cs       (DbContext)

Repositories/
  ├── IRepository.cs                (Futuro)
  ├── BaseRepository.cs             (Futuro)
  └── ...

DependencyInjection.cs              (Inyección de dependencias)
```

---

### 4. **Veterinaria.API** (Capa de Presentación)
**Responsabilidad:** Exponer funcionalidades como REST API

- ✅ Controllers con endpoints REST
- ✅ Validación de entrada
- ✅ Manejo de excepciones global
- ✅ Middleware personalizado
- ✅ Swagger/OpenAPI
- ✅ Depende de Application, Infrastructure y Shared

**Estructura:**
```
Controllers/
  ├── ClientesController.cs
  ├── MascotasController.cs
  └── VacunasController.cs

Middleware/
  └── ExceptionHandlingMiddleware.cs

Extensions/
  ├── ServiceCollectionExtensions.cs
  └── ApplicationBuilderExtensions.cs

Program.cs
```

---

### 5. **Veterinaria.Shared** (Capa Compartida)
**Responsabilidad:** Código reutilizable en todo el proyecto

- ✅ Excepciones compartidas
- ✅ Constantes globales
- ✅ Métodos de extensión
- ✅ Utilidades comunes
- ✅ NO tiene dependencias de otras capas

**Contenido:**
```
Constants/
  └── ApiConstants.cs

Exceptions/
  ├── NotFoundException.cs
  └── ValidationException.cs

Extensions/
  └── ... (métodos de extensión futuros)

Utilities/
  └── ... (funciones auxiliares futuras)
```

---

## 🔗 Dependencias entre Proyectos

```
Veterinaria.API
  ├─→ Veterinaria.Application (Controllers usan Services y DTOs)
  ├─→ Veterinaria.Infrastructure (Inyección de dependencias)
  └─→ Veterinaria.Shared (Excepciones, Constantes)

Veterinaria.Application
  ├─→ Veterinaria.Domain (Usa entidades)
  └─→ Veterinaria.Shared (Excepciones, Constantes)

Veterinaria.Infrastructure
  ├─→ Veterinaria.Application (Implementa interfaces)
  ├─→ Veterinaria.Domain (Acceso a entidades)
  └─→ Veterinaria.Shared (Excepciones)

Veterinaria.Domain
  └─→ (Independiente - no depende de nada)

Veterinaria.Shared
  └─→ (Independiente - no depende de nada)
```

---

## 🎨 Patrones Implementados

### 1. **Clean Architecture**
- Separación clara de responsabilidades
- Independencia de frameworks
- Fácil de testear

### 2. **Dependency Injection**
```csharp
// En DependencyInjection.cs
services.AddScoped<IApplicationDbContext, BDVeterinariaContext>();
services.AddScoped<IClienteService, ClienteService>();
```

### 3. **DTO Pattern**
- Separación entre modelos de negocio y transferencia
- Validación automática
- Mapeo entre capas

### 4. **Repository Pattern** (Futuro)
- Abstracción del acceso a datos
- Facilita testing
- Cambio de BD sin afectar lógica

### 5. **Middleware Pattern**
```csharp
// Manejo global de excepciones
app.UseMiddleware<ExceptionHandlingMiddleware>();
```

---

## 📏 Mejores Prácticas

### ✅ **HACER**

1. **Mantener la separación de capas**
   ```csharp
   // ✅ BIEN: API llama a Services en Application
   [HttpGet("{id}")]
   public async Task<IActionResult> GetCliente(int id)
   {
       var cliente = await _clienteService.GetClienteAsync(id);
       return Ok(cliente);
   }
   ```

2. **Usar inyección de dependencias**
   ```csharp
   // ✅ BIEN
   public class ClientesController
   {
       public ClientesController(IApplicationDbContext context) { }
   }
   
   // ❌ MALO
   public class ClientesController
   {
       private BDVeterinariaContext _context = new(); // Direct instantiation
   }
   ```

3. **Validar en DTOs**
   ```csharp
   // ✅ BIEN: Validación declarativa
   public class CreateClienteDTO
   {
       [Required(ErrorMessage = "...")]
       [StringLength(100)]
       public string NombreCompleto { get; set; }
   }
   ```

4. **Mapear entre capas**
   ```csharp
   // ✅ BIEN: Usar mappers (AutoMapper en el futuro)
   var dto = new ClienteDTO
   {
       IdCliente = cliente.IdCliente,
       NombreCompleto = cliente.NombreCompleto
   };
   ```

5. **Manejar excepciones globalmente**
   ```csharp
   // ✅ BIEN: Middleware global captura excepciones
   // ❌ MALO: Try-catch en cada endpoint
   ```

---

### ❌ **NO HACER**

1. ❌ Dejar lógica de negocio en Controllers
2. ❌ Pasar entidades directamente a cliente (usar DTOs)
3. ❌ Acceder a BD desde Controllers
4. ❌ Mezclar responsabilidades en una clase
5. ❌ Ignorar excepciones silenciosamente

---

## 🧪 Testing

### Unit Tests (`tests/Veterinaria.Tests.Unit/`)
- Pruebas de servicios sin BD
- Pruebas de utilidades
- Mock de dependencias

### Integration Tests (`tests/Veterinaria.Tests.Integration/`)
- Pruebas de endpoints API
- Pruebas con BD real
- Validación de flujos completos

---

## 🔄 Flujo de una Solicitud

```
HTTP Request
    ↓
API Controller (Veterinaria.API)
    ↓
Service (Veterinaria.Application)
    ↓
DbContext (Veterinaria.Infrastructure)
    ↓
Database
    ↓
Entity (Veterinaria.Domain)
    ↓
DTO (Veterinaria.Application)
    ↓
HTTP Response
```

---

## 📦 Próximas Mejoras

- [ ] Implementar Repository Pattern completamente
- [ ] Agregar AutoMapper para mapeo automático de DTOs
- [ ] Implementar especificaciones (Specification Pattern)
- [ ] Agregar Unit of Work Pattern
- [ ] Implementar CQRS (Command Query Responsibility Segregation)
- [ ] Autenticación JWT
- [ ] Rate limiting
- [ ] Caching distribuido

---

**Última actualización:** 2024
**Autor:** Senior Developer
**Versión:** 1.0
