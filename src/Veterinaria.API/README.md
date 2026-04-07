# 📚 API REST - Veterinaria

## 🚀 Descripción

API REST profesional para gestión integral de clínica veterinaria, construida con **ASP.NET Core 10** y **Clean Architecture**.

---

## 🏗️ **Estructura**

```
Veterinaria.API/
├── Controllers/          # Controladores REST
│   ├── ClientesController.cs
│   ├── MascotasController.cs
│   └── VacunasController.cs
├── DTOs/                 # Data Transfer Objects
│   ├── ClienteDTOs/
│   ├── MascotaDTOs/
│   └── VacunaDTOs/
├── Program.cs            # Configuración de la aplicación
├── appsettings.json      # Configuración
└── Veterinaria.API.csproj
```

---

## 🔧 **Requisitos**

- .NET 10.0 SDK o superior
- SQL Server
- Visual Studio 2026 (recomendado)

---

## ⚙️ **Instalación y Ejecución**

### 1. Clonar el repositorio
```bash
git clone https://github.com/JorgeMarquis/App-Veterinaria.git
cd App-Veterinaria
```

### 2. Restaurar dependencias
```bash
dotnet restore
```

### 3. Configurar base de datos
- Editar `appsettings.json` con tu cadena de conexión
- Ejecutar migraciones (si es necesario)

### 4. Ejecutar la API
```bash
cd src/Veterinaria.API
dotnet run
```

La API estará disponible en: `https://localhost:5001`

Swagger UI estará en: `https://localhost:5001/`

---

## 📡 **Endpoints Principales**

### **Clientes**

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/clientes` | Obtener todos los clientes |
| GET | `/api/clientes/{id}` | Obtener cliente por ID |
| POST | `/api/clientes` | Crear nuevo cliente |
| PUT | `/api/clientes/{id}` | Actualizar cliente |
| DELETE | `/api/clientes/{id}` | Eliminar cliente |

### **Mascotas**

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/mascotas` | Obtener todas las mascotas |
| GET | `/api/mascotas/{id}` | Obtener mascota por ID |
| GET | `/api/mascotas/cliente/{idCliente}` | Obtener mascotas por cliente |
| POST | `/api/mascotas` | Crear nueva mascota |
| PUT | `/api/mascotas/{id}` | Actualizar mascota |
| DELETE | `/api/mascotas/{id}` | Eliminar mascota |

### **Vacunas**

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/vacunas` | Obtener todas las vacunas |
| GET | `/api/vacunas/{id}` | Obtener vacuna por ID |
| POST | `/api/vacunas` | Crear nueva vacuna |
| PUT | `/api/vacunas/{id}` | Actualizar vacuna |
| DELETE | `/api/vacunas/{id}` | Eliminar vacuna |

---

## 📝 **Ejemplos de Uso**

### **Crear un Cliente**

```bash
curl -X POST "https://localhost:5001/api/clientes" \
  -H "Content-Type: application/json" \
  -d '{
    "nombreCompleto": "Juan Pérez",
    "tipoIdentificacion": "CC",
    "numIdentificacion": "1234567890",
    "email": "juan@example.com",
    "telefonoPrincipal": "+57 300 123 4567",
    "direccion": "Calle 123 # 456",
    "ciudad": "Bogotá"
  }'
```

**Respuesta (201 Created):**
```json
{
  "idCliente": 1,
  "nombreCompleto": "Juan Pérez",
  "tipoIdentificacion": "CC",
  "numIdentificacion": "1234567890",
  "email": "juan@example.com",
  "telefonoPrincipal": "+57 300 123 4567",
  "direccion": "Calle 123 # 456",
  "ciudad": "Bogotá"
}
```

### **Obtener Cliente**

```bash
curl -X GET "https://localhost:5001/api/clientes/1"
```

**Respuesta (200 OK):**
```json
{
  "idCliente": 1,
  "nombreCompleto": "Juan Pérez",
  ...
}
```

### **Crear Mascota**

```bash
curl -X POST "https://localhost:5001/api/mascotas" \
  -H "Content-Type: application/json" \
  -d '{
    "idCliente": 1,
    "nombre": "Firulais",
    "idEspecie": 1,
    "idRaza": 5,
    "sexo": "M",
    "colorPelaje": "Café",
    "microchipId": "ABC123"
  }'
```

### **Obtener Mascotas de un Cliente**

```bash
curl -X GET "https://localhost:5001/api/mascotas/cliente/1"
```

---

## 📊 **Respuestas HTTP**

### Códigos de Éxito
```
200 OK              - Solicitud exitosa con datos
201 Created         - Recurso creado
204 No Content      - Operación exitosa sin datos
```

### Códigos de Error del Cliente
```
400 Bad Request     - Datos inválidos
404 Not Found       - Recurso no encontrado
```

### Códigos de Error del Servidor
```
500 Internal Server Error - Error en el servidor
```

---

## 📚 **Documentación Interactiva**

Accede a **Swagger UI** para probar todos los endpoints:

```
https://localhost:5001/
```

---

## 🏛️ **Arquitectura**

La API sigue **Clean Architecture** con separación de capas:

```
Veterinaria.Domain          ← Entidades puras (sin dependencias)
    ↑
Veterinaria.Application     ← Lógica de negocio (contratos)
    ↑
Veterinaria.Infrastructure  ← Detalles técnicos (BD, autenticación)
    ↑
Veterinaria.API             ← Presentación (Controladores REST)
```

---

## 🔐 **Validación de Datos**

Todos los DTOs incluyen validación de atributos:

- `[Required]` - Campo requerido
- `[StringLength]` - Longitud máxima
- `[EmailAddress]` - Formato de email
- `[Phone]` - Formato de teléfono
- `[Range]` - Rango numérico

---

## 📦 **Dependencias**

- **Microsoft.AspNetCore.OpenApi** - Documentación OpenAPI
- **Swashbuckle.AspNetCore** - Swagger UI
- **Microsoft.AspNetCore.Mvc.Versioning** - Versionado de API
- **Microsoft.AspNetCore.Cors** - CORS habilitado

---

## 🐛 **Manejo de Errores**

Todas las respuestas de error incluyen:

```json
{
  "message": "Descripción del error",
  "error": "Detalles técnicos"
}
```

---

## 🚀 **Próximas Mejoras**

- [ ] Autenticación JWT
- [ ] Más controladores (Atenciones, Productos, Compras)
- [ ] Paginación
- [ ] Filtrado avanzado
- [ ] Caching
- [ ] Rate limiting
- [ ] Auditoría detallada

---

## 📧 **Contacto**

Para reportar problemas o sugerencias, contacta al equipo de desarrollo.

---

**Última actualización:** Abril 2026
