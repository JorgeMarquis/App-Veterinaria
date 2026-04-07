# 🎯 REFERENCIA RÁPIDA: API REST ENDPOINTS

## 📱 CLIENTES

```http
GET    /api/clientes
POST   /api/clientes
GET    /api/clientes/{id}
PUT    /api/clientes/{id}
DELETE /api/clientes/{id}
```

### Ejemplo: Crear Cliente
```http
POST /api/clientes HTTP/1.1
Content-Type: application/json

{
  "nombreCompleto": "Juan Pérez",
  "tipoIdentificacion": "CC",
  "numIdentificacion": "1234567890",
  "email": "juan@example.com",
  "telefonoPrincipal": "+57 300 123 4567",
  "direccion": "Calle 123",
  "ciudad": "Bogotá"
}
```

**Response (201 Created):**
```json
{
  "idCliente": 1,
  "nombreCompleto": "Juan Pérez",
  "tipoIdentificacion": "CC",
  ...
}
```

---

## 🐕 MASCOTAS

```http
GET    /api/mascotas
POST   /api/mascotas
GET    /api/mascotas/{id}
PUT    /api/mascotas/{id}
DELETE /api/mascotas/{id}
GET    /api/mascotas/cliente/{idCliente}
```

### Ejemplo: Obtener Mascotas de Cliente
```http
GET /api/mascotas/cliente/1 HTTP/1.1
```

**Response (200 OK):**
```json
[
  {
    "idMascota": 1,
    "idCliente": 1,
    "nombre": "Firulais",
    "idEspecie": 1,
    "sexo": "M",
    ...
  }
]
```

---

## 💉 VACUNAS

```http
GET    /api/vacunas
POST   /api/vacunas
GET    /api/vacunas/{id}
PUT    /api/vacunas/{id}
DELETE /api/vacunas/{id}
```

### Ejemplo: Crear Vacuna
```http
POST /api/vacunas HTTP/1.1
Content-Type: application/json

{
  "nombre": "DHPP",
  "tipo": "Polivalente",
  "frecuenciaRefuerzoMeses": 12,
  "edadPrimeraDosisSemanas": 6,
  "laboratorio": "Boehringer",
  "enfermedadesPrevine": "Distemper, Hepatitis, Parvovirus",
  "activo": true
}
```

---

## 📊 CÓDIGOS HTTP

| Código | Significado |
|--------|-------------|
| **200** | OK - Operación exitosa |
| **201** | Created - Recurso creado |
| **204** | No Content - Éxito sin datos |
| **400** | Bad Request - Datos inválidos |
| **404** | Not Found - Recurso no existe |
| **500** | Server Error - Error del servidor |

---

## 🧪 HERRAMIENTAS PARA PROBAR

### **Swagger UI**
```
https://localhost:5001/
```

### **Curl**
```bash
curl https://localhost:5001/api/clientes
```

### **JavaScript**
```javascript
const resp = await fetch('https://localhost:5001/api/clientes');
const data = await resp.json();
```

### **Python**
```python
import requests
r = requests.get('https://localhost:5001/api/clientes')
print(r.json())
```

---

## ⚠️ ERRORES COMUNES

```json
{
  "message": "Cliente no encontrado",
  "error": "Invalid ID"
}
```

---

## 🚀 INICIAR API

**Visual Studio:**
```
F5
```

**Terminal:**
```bash
cd src\Veterinaria.API
dotnet run
```

**Con Watch (reload automático):**
```bash
dotnet watch run
```

---

## 📚 DOCUMENTACIÓN

- `README.md` - Guía principal
- `QUICK_START.md` - Inicio rápido
- `EJEMPLOS_USO.md` - Código de ejemplo
- `EXPANSION_GUIA.md` - Expandir API

---

**API en: https://localhost:5001/ ✨**
