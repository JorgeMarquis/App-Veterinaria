# 🎯 EJEMPLOS PRÁCTICOS DE USO DE LA API

## 📝 Ejemplos con JavaScript/Fetch API

### **1. Obtener todos los clientes**

```javascript
// Método: GET
async function obtenerClientes() {
  try {
    const response = await fetch('https://localhost:5001/api/clientes');
    const clientes = await response.json();
    console.log('Clientes:', clientes);
    return clientes;
  } catch (error) {
    console.error('Error al obtener clientes:', error);
  }
}

// Usar
obtenerClientes();
```

---

### **2. Crear un nuevo cliente**

```javascript
async function crearCliente(datosCliente) {
  try {
    const response = await fetch('https://localhost:5001/api/clientes', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        nombreCompleto: datosCliente.nombre,
        tipoIdentificacion: 'CC',
        numIdentificacion: datosCliente.cedula,
        email: datosCliente.email,
        telefonoPrincipal: datosCliente.telefono,
        direccion: datosCliente.direccion,
        ciudad: datosCliente.ciudad
      })
    });

    if (!response.ok) {
      throw new Error(`Error: ${response.status}`);
    }

    const clienteCreado = await response.json();
    console.log('Cliente creado:', clienteCreado);
    return clienteCreado;
  } catch (error) {
    console.error('Error al crear cliente:', error);
  }
}

// Usar
crearCliente({
  nombre: 'Juan Pérez',
  cedula: '1234567890',
  email: 'juan@example.com',
  telefono: '+57 300 123 4567',
  direccion: 'Calle 123',
  ciudad: 'Bogotá'
});
```

---

### **3. Obtener un cliente específico**

```javascript
async function obtenerCliente(idCliente) {
  try {
    const response = await fetch(`https://localhost:5001/api/clientes/${idCliente}`);
    
    if (!response.ok) {
      if (response.status === 404) {
        console.log('Cliente no encontrado');
        return null;
      }
      throw new Error(`Error: ${response.status}`);
    }

    const cliente = await response.json();
    console.log('Cliente encontrado:', cliente);
    return cliente;
  } catch (error) {
    console.error('Error:', error);
  }
}

// Usar
obtenerCliente(1);
```

---

### **4. Actualizar un cliente**

```javascript
async function actualizarCliente(idCliente, datosActualizados) {
  try {
    const response = await fetch(
      `https://localhost:5001/api/clientes/${idCliente}`,
      {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          nombreCompleto: datosActualizados.nombreCompleto,
          email: datosActualizados.email,
          telefonoPrincipal: datosActualizados.telefonoPrincipal,
          direccion: datosActualizados.direccion,
          ciudad: datosActualizados.ciudad
        })
      }
    );

    if (response.ok) {
      console.log('Cliente actualizado correctamente');
      return true;
    }
  } catch (error) {
    console.error('Error al actualizar cliente:', error);
  }
}

// Usar
actualizarCliente(1, {
  nombreCompleto: 'Juan Carlos Pérez',
  email: 'juancarlos@example.com',
  telefonoPrincipal: '+57 300 987 6543',
  direccion: 'Calle 456',
  ciudad: 'Medellín'
});
```

---

### **5. Eliminar un cliente**

```javascript
async function eliminarCliente(idCliente) {
  try {
    const response = await fetch(
      `https://localhost:5001/api/clientes/${idCliente}`,
      { method: 'DELETE' }
    );

    if (response.ok) {
      console.log('Cliente eliminado correctamente');
      return true;
    }
  } catch (error) {
    console.error('Error al eliminar cliente:', error);
  }
}

// Usar
eliminarCliente(1);
```

---

## 🐕 Ejemplos con Mascotas

### **1. Obtener mascotas de un cliente**

```javascript
async function obtenerMascotasCliente(idCliente) {
  try {
    const response = await fetch(
      `https://localhost:5001/api/mascotas/cliente/${idCliente}`
    );
    const mascotas = await response.json();
    console.log('Mascotas del cliente:', mascotas);
    return mascotas;
  } catch (error) {
    console.error('Error:', error);
  }
}

// Usar
obtenerMascotasCliente(1);
```

---

### **2. Crear una mascota**

```javascript
async function crearMascota(datosM ascota) {
  try {
    const response = await fetch('https://localhost:5001/api/mascotas', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        idCliente: datosMascota.idCliente,
        nombre: datosMascota.nombre,
        idEspecie: datosMascota.idEspecie,  // 1=Perro, 2=Gato, etc.
        idRaza: datosMascota.idRaza,
        sexo: datosMascota.sexo,              // 'M' o 'F'
        colorPelaje: datosMascota.colorPelaje,
        microchipId: datosMascota.microchipId
      })
    });

    const mascotaCreada = await response.json();
    console.log('Mascota creada:', mascotaCreada);
    return mascotaCreada;
  } catch (error) {
    console.error('Error:', error);
  }
}

// Usar
crearMascota({
  idCliente: 1,
  nombre: 'Firulais',
  idEspecie: 1,
  idRaza: 5,
  sexo: 'M',
  colorPelaje: 'Café',
  microchipId: 'ABC123456'
});
```

---

## 💉 Ejemplos con Vacunas

### **1. Obtener todas las vacunas**

```javascript
async function obtenerVacunas() {
  try {
    const response = await fetch('https://localhost:5001/api/vacunas');
    const vacunas = await response.json();
    console.log('Vacunas disponibles:', vacunas);
    return vacunas;
  } catch (error) {
    console.error('Error:', error);
  }
}

// Usar
obtenerVacunas();
```

---

### **2. Crear una vacuna**

```javascript
async function crearVacuna(datosVacuna) {
  try {
    const response = await fetch('https://localhost:5001/api/vacunas', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        nombre: datosVacuna.nombre,
        tipo: datosVacuna.tipo,
        frecuenciaRefuerzoMeses: datosVacuna.frecuencia,
        edadPrimeraDosisSemanas: datosVacuna.edadInicial,
        laboratorio: datosVacuna.laboratorio,
        enfermedadesPrevine: datosVacuna.enfermedades,
        activo: true
      })
    });

    const vacunaCreada = await response.json();
    return vacunaCreada;
  } catch (error) {
    console.error('Error:', error);
  }
}

// Usar
crearVacuna({
  nombre: 'DHPP',
  tipo: 'Polivalente',
  frecuencia: 12,
  edadInicial: 6,
  laboratorio: 'Boehringer',
  enfermedades: 'Distemper, Hepatitis, Parvovirus, Parainfluenza'
});
```

---

## 🎛️ Clase Helper para API

```javascript
class VeterinariaAPI {
  constructor(baseUrl = 'https://localhost:5001/api') {
    this.baseUrl = baseUrl;
  }

  async request(endpoint, options = {}) {
    const url = `${this.baseUrl}${endpoint}`;
    
    try {
      const response = await fetch(url, {
        headers: {
          'Content-Type': 'application/json',
          ...options.headers
        },
        ...options
      });

      if (!response.ok) {
        const error = await response.json().catch(() => ({
          message: `HTTP ${response.status}`
        }));
        throw new Error(error.message || `Error ${response.status}`);
      }

      if (response.status === 204) {
        return null; // No Content
      }

      return await response.json();
    } catch (error) {
      console.error(`Error en ${endpoint}:`, error);
      throw error;
    }
  }

  // CLIENTES
  clientes = {
    obtenerTodos: () => this.request('/clientes'),
    obtener: (id) => this.request(`/clientes/${id}`),
    crear: (datos) => this.request('/clientes', {
      method: 'POST',
      body: JSON.stringify(datos)
    }),
    actualizar: (id, datos) => this.request(`/clientes/${id}`, {
      method: 'PUT',
      body: JSON.stringify(datos)
    }),
    eliminar: (id) => this.request(`/clientes/${id}`, {
      method: 'DELETE'
    })
  };

  // MASCOTAS
  mascotas = {
    obtenerTodas: () => this.request('/mascotas'),
    obtener: (id) => this.request(`/mascotas/${id}`),
    porCliente: (idCliente) => this.request(`/mascotas/cliente/${idCliente}`),
    crear: (datos) => this.request('/mascotas', {
      method: 'POST',
      body: JSON.stringify(datos)
    }),
    actualizar: (id, datos) => this.request(`/mascotas/${id}`, {
      method: 'PUT',
      body: JSON.stringify(datos)
    }),
    eliminar: (id) => this.request(`/mascotas/${id}`, {
      method: 'DELETE'
    })
  };

  // VACUNAS
  vacunas = {
    obtenerTodas: () => this.request('/vacunas'),
    obtener: (id) => this.request(`/vacunas/${id}`),
    crear: (datos) => this.request('/vacunas', {
      method: 'POST',
      body: JSON.stringify(datos)
    }),
    actualizar: (id, datos) => this.request(`/vacunas/${id}`, {
      method: 'PUT',
      body: JSON.stringify(datos)
    }),
    eliminar: (id) => this.request(`/vacunas/${id}`, {
      method: 'DELETE'
    })
  };
}

// Usar
const api = new VeterinariaAPI();

// Ejemplo
(async () => {
  try {
    const clientes = await api.clientes.obtenerTodos();
    console.log('Clientes:', clientes);

    const nuevoCliente = await api.clientes.crear({
      nombreCompleto: 'María García',
      tipoIdentificacion: 'CC',
      numIdentificacion: '9876543210',
      email: 'maria@example.com',
      telefonoPrincipal: '+57 310 654 3210',
      direccion: 'Calle 789',
      ciudad: 'Cali'
    });
    console.log('Cliente creado:', nuevoCliente);
  } catch (error) {
    console.error('Error:', error);
  }
})();
```

---

## 📱 Ejemplo con React

```javascript
import { useState, useEffect } from 'react';

function ListaClientes() {
  const [clientes, setClientes] = useState([]);
  const [cargando, setCargando] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const cargarClientes = async () => {
      try {
        const response = await fetch('https://localhost:5001/api/clientes');
        const datos = await response.json();
        setClientes(datos);
      } catch (err) {
        setError(err.message);
      } finally {
        setCargando(false);
      }
    };

    cargarClientes();
  }, []);

  if (cargando) return <div>Cargando...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <div>
      <h1>Clientes</h1>
      <ul>
        {clientes.map(cliente => (
          <li key={cliente.idCliente}>
            {cliente.nombreCompleto} - {cliente.email}
          </li>
        ))}
      </ul>
    </div>
  );
}

export default ListaClientes;
```

---

## 🧪 Probando con Thunder Client / Postman

### **Crear Cliente - Request**
```
POST https://localhost:5001/api/clientes
Content-Type: application/json

{
  "nombreCompleto": "Carlos López",
  "tipoIdentificacion": "CC",
  "numIdentificacion": "5555555555",
  "email": "carlos@example.com",
  "telefonoPrincipal": "+57 300 555 5555",
  "telefonoAlternativo": "+57 310 555 5555",
  "direccion": "Avenida 10",
  "ciudad": "Barranquilla",
  "contactoEmergencia": "Ana López",
  "telefonoEmergencia": "+57 300 777 7777",
  "observaciones": "Cliente VIP"
}
```

### **Response (201 Created)**
```json
{
  "idCliente": 5,
  "nombreCompleto": "Carlos López",
  "tipoIdentificacion": "CC",
  "numIdentificacion": "5555555555",
  "email": "carlos@example.com",
  "telefonoPrincipal": "+57 300 555 5555",
  ...
}
```

---

## ⚠️ Manejo de Errores

```javascript
async function obtenerClienteSeguro(id) {
  try {
    const response = await fetch(`https://localhost:5001/api/clientes/${id}`);
    
    switch(response.status) {
      case 200:
        return await response.json();
      case 404:
        throw new Error('Cliente no encontrado');
      case 400:
        throw new Error('Solicitud inválida');
      case 500:
        throw new Error('Error en el servidor');
      default:
        throw new Error(`Error desconocido: ${response.status}`);
    }
  } catch (error) {
    console.error('Error:', error.message);
    return null;
  }
}
```

---

**¡Todos los ejemplos están listos para usar! Cópia y modifica según tus necesidades 🚀**
