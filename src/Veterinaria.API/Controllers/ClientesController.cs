using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Veterinaria.Application.Abstractions;
using Veterinaria.Application.DTOs.ClienteDTOs;
using Veterinaria.Domain.Entities;

namespace Veterinaria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<ClientesController> _logger;

    public ClientesController(IApplicationDbContext context, ILogger<ClientesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene la lista de todos los clientes
    /// </summary>
    /// <returns>Lista de clientes</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<ClienteDTO>>> GetClientes()
    {
        try
        {
            var clientes = await _context.Clientes.ToListAsync();
            var clienteDTOs = clientes.Select(c => MapToDTO(c)).ToList();
            return Ok(clienteDTOs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener clientes");
            return StatusCode(500, new { message = "Error al obtener los clientes", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene un cliente específico por ID
    /// </summary>
    /// <param name="id">ID del cliente</param>
    /// <returns>Datos del cliente</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ClienteDTO>> GetCliente(int id)
    {
        try
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound(new { message = $"Cliente con ID {id} no encontrado" });

            return Ok(MapToDTO(cliente));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener cliente {ClienteId}", id);
            return StatusCode(500, new { message = "Error al obtener el cliente", error = ex.Message });
        }
    }

    /// <summary>
    /// Crea un nuevo cliente
    /// </summary>
    /// <param name="createDTO">Datos del cliente a crear</param>
    /// <returns>Cliente creado</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ClienteDTO>> CreateCliente(CreateClienteDTO createDTO)
    {
        try
        {
            // Validar si el cliente ya existe
            var existe = await _context.Clientes
                .AnyAsync(c => c.NumIdentificacion == createDTO.NumIdentificacion);
            
            if (existe)
                return BadRequest(new { message = "Ya existe un cliente con este número de identificación" });

            var cliente = new Cliente
            {
                NombreCompleto = createDTO.NombreCompleto,
                TipoIdentificacion = createDTO.TipoIdentificacion,
                NumIdentificacion = createDTO.NumIdentificacion,
                Email = createDTO.Email,
                TelefonoPrincipal = createDTO.TelefonoPrincipal,
                TelefonoAlternativo = createDTO.TelefonoAlternativo,
                Direccion = createDTO.Direccion,
                Ciudad = createDTO.Ciudad,
                ContactoEmergencia = createDTO.ContactoEmergencia,
                TelefonoEmergencia = createDTO.TelefonoEmergencia,
                Observaciones = createDTO.Observaciones
            };

            _context.Add(cliente);
            await _context.SaveChangesAsync();

            var clienteDTO = MapToDTO(cliente);
            return CreatedAtAction(nameof(GetCliente), new { id = cliente.IdCliente }, clienteDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear cliente");
            return StatusCode(500, new { message = "Error al crear el cliente", error = ex.Message });
        }
    }

    /// <summary>
    /// Actualiza un cliente existente
    /// </summary>
    /// <param name="id">ID del cliente</param>
    /// <param name="updateDTO">Datos a actualizar</param>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateCliente(int id, UpdateClienteDTO updateDTO)
    {
        try
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound(new { message = $"Cliente con ID {id} no encontrado" });

            cliente.NombreCompleto = updateDTO.NombreCompleto;
            cliente.Email = updateDTO.Email;
            cliente.TelefonoPrincipal = updateDTO.TelefonoPrincipal;
            cliente.TelefonoAlternativo = updateDTO.TelefonoAlternativo;
            cliente.Direccion = updateDTO.Direccion;
            cliente.Ciudad = updateDTO.Ciudad;
            cliente.ContactoEmergencia = updateDTO.ContactoEmergencia;
            cliente.TelefonoEmergencia = updateDTO.TelefonoEmergencia;
            cliente.Observaciones = updateDTO.Observaciones;

            _context.Update(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar cliente {ClienteId}", id);
            return StatusCode(500, new { message = "Error al actualizar el cliente", error = ex.Message });
        }
    }

    /// <summary>
    /// Elimina un cliente
    /// </summary>
    /// <param name="id">ID del cliente</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteCliente(int id)
    {
        try
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound(new { message = $"Cliente con ID {id} no encontrado" });

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar cliente {ClienteId}", id);
            return StatusCode(500, new { message = "Error al eliminar el cliente", error = ex.Message });
        }
    }

    private static ClienteDTO MapToDTO(Cliente cliente)
    {
        return new ClienteDTO
        {
            IdCliente = cliente.IdCliente,
            NombreCompleto = cliente.NombreCompleto,
            TipoIdentificacion = cliente.TipoIdentificacion,
            NumIdentificacion = cliente.NumIdentificacion,
            Email = cliente.Email,
            TelefonoPrincipal = cliente.TelefonoPrincipal,
            TelefonoAlternativo = cliente.TelefonoAlternativo,
            Direccion = cliente.Direccion,
            Ciudad = cliente.Ciudad,
            ContactoEmergencia = cliente.ContactoEmergencia,
            TelefonoEmergencia = cliente.TelefonoEmergencia,
            Observaciones = cliente.Observaciones
        };
    }
}
