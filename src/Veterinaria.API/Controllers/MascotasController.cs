using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Veterinaria.Application.Abstractions;
using Veterinaria.Application.DTOs.MascotaDTOs;
using Veterinaria.Domain.Entities;

namespace Veterinaria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MascotasController : ControllerBase
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<MascotasController> _logger;

    public MascotasController(IApplicationDbContext context, ILogger<MascotasController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene la lista de todas las mascotas
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<MascotaDTO>>> GetMascotas()
    {
        try
        {
            var mascotas = await _context.Mascota.ToListAsync();
            var mascotaDTOs = mascotas.Select(m => MapToDTO(m)).ToList();
            return Ok(mascotaDTOs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener mascotas");
            return StatusCode(500, new { message = "Error al obtener las mascotas", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene una mascota específica por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<MascotaDTO>> GetMascota(int id)
    {
        try
        {
            var mascota = await _context.Mascota.FindAsync(id);
            if (mascota == null)
                return NotFound(new { message = $"Mascota con ID {id} no encontrada" });

            return Ok(MapToDTO(mascota));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener mascota {MascotaId}", id);
            return StatusCode(500, new { message = "Error al obtener la mascota", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene mascotas de un cliente específico
    /// </summary>
    [HttpGet("cliente/{idCliente}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<MascotaDTO>>> GetMascotasPorCliente(int idCliente)
    {
        try
        {
            var mascotas = await _context.Mascota
                .Where(m => m.IdCliente == idCliente)
                .ToListAsync();

            var mascotaDTOs = mascotas.Select(m => MapToDTO(m)).ToList();
            return Ok(mascotaDTOs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener mascotas del cliente {ClienteId}", idCliente);
            return StatusCode(500, new { message = "Error al obtener las mascotas", error = ex.Message });
        }
    }

    /// <summary>
    /// Crea una nueva mascota
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<MascotaDTO>> CreateMascota(CreateMascotaDTO createDTO)
    {
        try
        {
            // Validar que el cliente existe
            var clienteExiste = await _context.Clientes.AnyAsync(c => c.IdCliente == createDTO.IdCliente);
            if (!clienteExiste)
                return BadRequest(new { message = "El cliente especificado no existe" });

            // Validar que la especie existe
            var especieExiste = await _context.Especies.AnyAsync(e => e.IdEspecie == createDTO.IdEspecie);
            if (!especieExiste)
                return BadRequest(new { message = "La especie especificada no existe" });

            var mascota = new Mascota
            {
                IdCliente = createDTO.IdCliente,
                Nombre = createDTO.Nombre,
                IdEspecie = createDTO.IdEspecie,
                IdRaza = createDTO.IdRaza,
                FechaNacimiento = createDTO.FechaNacimiento,
                Sexo = createDTO.Sexo,
                ColorPelaje = createDTO.ColorPelaje,
                MicrochipId = createDTO.MicrochipId,
                FotoUrl = createDTO.FotoUrl,
                Activo = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Add(mascota);
            await _context.SaveChangesAsync();

            var mascotaDTO = MapToDTO(mascota);
            return CreatedAtAction(nameof(GetMascota), new { id = mascota.IdMascota }, mascotaDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear mascota");
            return StatusCode(500, new { message = "Error al crear la mascota", error = ex.Message });
        }
    }

    /// <summary>
    /// Actualiza una mascota existente
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateMascota(int id, UpdateMascotaDTO updateDTO)
    {
        try
        {
            var mascota = await _context.Mascota.FindAsync(id);
            if (mascota == null)
                return NotFound(new { message = $"Mascota con ID {id} no encontrada" });

            mascota.Nombre = updateDTO.Nombre;
            mascota.IdEspecie = updateDTO.IdEspecie;
            mascota.IdRaza = updateDTO.IdRaza;
            mascota.FechaNacimiento = updateDTO.FechaNacimiento;
            mascota.Sexo = updateDTO.Sexo;
            mascota.ColorPelaje = updateDTO.ColorPelaje;
            mascota.FotoUrl = updateDTO.FotoUrl;
            mascota.Activo = updateDTO.Activo;
            mascota.UpdatedAt = DateTime.UtcNow;

            _context.Update(mascota);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar mascota {MascotaId}", id);
            return StatusCode(500, new { message = "Error al actualizar la mascota", error = ex.Message });
        }
    }

    /// <summary>
    /// Elimina una mascota
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteMascota(int id)
    {
        try
        {
            var mascota = await _context.Mascota.FindAsync(id);
            if (mascota == null)
                return NotFound(new { message = $"Mascota con ID {id} no encontrada" });

            _context.Mascota.Remove(mascota);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar mascota {MascotaId}", id);
            return StatusCode(500, new { message = "Error al eliminar la mascota", error = ex.Message });
        }
    }

    private static MascotaDTO MapToDTO(Mascota mascota)
    {
        return new MascotaDTO
        {
            IdMascota = mascota.IdMascota,
            IdCliente = mascota.IdCliente,
            Nombre = mascota.Nombre,
            IdEspecie = mascota.IdEspecie,
            IdRaza = mascota.IdRaza,
            FechaNacimiento = mascota.FechaNacimiento,
            Sexo = mascota.Sexo,
            ColorPelaje = mascota.ColorPelaje,
            MicrochipId = mascota.MicrochipId,
            FotoUrl = mascota.FotoUrl,
            FechaFallecimiento = mascota.FechaFallecimiento,
            Activo = mascota.Activo,
            CreatedAt = mascota.CreatedAt,
            CreatedBy = mascota.CreatedBy,
            UpdatedAt = mascota.UpdatedAt,
            UpdatedBy = mascota.UpdatedBy
        };
    }
}
