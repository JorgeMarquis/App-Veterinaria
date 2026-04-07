using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Veterinaria.Application.Abstractions;
using Veterinaria.Application.DTOs.VacunaDTOs;
using Veterinaria.Domain.Entities;

namespace Veterinaria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VacunasController : ControllerBase
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<VacunasController> _logger;

    public VacunasController(IApplicationDbContext context, ILogger<VacunasController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene la lista de todas las vacunas
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<VacunaDTO>>> GetVacunas()
    {
        try
        {
            var vacunas = await _context.Vacunas.ToListAsync();
            var vacunaDTOs = vacunas.Select(v => MapToDTO(v)).ToList();
            return Ok(vacunaDTOs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener vacunas");
            return StatusCode(500, new { message = "Error al obtener las vacunas", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene una vacuna específica por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<VacunaDTO>> GetVacuna(int id)
    {
        try
        {
            var vacuna = await _context.Vacunas.FindAsync(id);
            if (vacuna == null)
                return NotFound(new { message = $"Vacuna con ID {id} no encontrada" });

            return Ok(MapToDTO(vacuna));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener vacuna {VacunaId}", id);
            return StatusCode(500, new { message = "Error al obtener la vacuna", error = ex.Message });
        }
    }

    /// <summary>
    /// Crea una nueva vacuna
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<VacunaDTO>> CreateVacuna(CreateVacunaDTO createDTO)
    {
        try
        {
            var vacuna = new Vacuna
            {
                Nombre = createDTO.Nombre,
                Tipo = createDTO.Tipo,
                FrecuenciaRefuerzoMeses = createDTO.FrecuenciaRefuerzoMeses,
                EdadPrimeraDosisSemanas = createDTO.EdadPrimeraDosisSemanas,
                Laboratorio = createDTO.Laboratorio,
                EnfermedadesPrevine = createDTO.EnfermedadesPrevine,
                Activo = createDTO.Activo,
                FechaCreacion = DateTime.UtcNow
            };

            _context.Add(vacuna);
            await _context.SaveChangesAsync();

            var vacunaDTO = MapToDTO(vacuna);
            return CreatedAtAction(nameof(GetVacuna), new { id = vacuna.IdVacuna }, vacunaDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear vacuna");
            return StatusCode(500, new { message = "Error al crear la vacuna", error = ex.Message });
        }
    }

    /// <summary>
    /// Actualiza una vacuna existente
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateVacuna(int id, UpdateVacunaDTO updateDTO)
    {
        try
        {
            var vacuna = await _context.Vacunas.FindAsync(id);
            if (vacuna == null)
                return NotFound(new { message = $"Vacuna con ID {id} no encontrada" });

            vacuna.Nombre = updateDTO.Nombre;
            vacuna.Tipo = updateDTO.Tipo;
            vacuna.FrecuenciaRefuerzoMeses = updateDTO.FrecuenciaRefuerzoMeses;
            vacuna.EdadPrimeraDosisSemanas = updateDTO.EdadPrimeraDosisSemanas;
            vacuna.Laboratorio = updateDTO.Laboratorio;
            vacuna.EnfermedadesPrevine = updateDTO.EnfermedadesPrevine;
            vacuna.Activo = updateDTO.Activo;
            vacuna.FechaModificacion = DateTime.UtcNow;

            _context.Update(vacuna);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar vacuna {VacunaId}", id);
            return StatusCode(500, new { message = "Error al actualizar la vacuna", error = ex.Message });
        }
    }

    /// <summary>
    /// Elimina una vacuna
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteVacuna(int id)
    {
        try
        {
            var vacuna = await _context.Vacunas.FindAsync(id);
            if (vacuna == null)
                return NotFound(new { message = $"Vacuna con ID {id} no encontrada" });

            _context.Vacunas.Remove(vacuna);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar vacuna {VacunaId}", id);
            return StatusCode(500, new { message = "Error al eliminar la vacuna", error = ex.Message });
        }
    }

    private static VacunaDTO MapToDTO(Vacuna vacuna)
    {
        return new VacunaDTO
        {
            IdVacuna = vacuna.IdVacuna,
            Nombre = vacuna.Nombre,
            Tipo = vacuna.Tipo,
            FrecuenciaRefuerzoMeses = vacuna.FrecuenciaRefuerzoMeses,
            EdadPrimeraDosisSemanas = vacuna.EdadPrimeraDosisSemanas,
            Laboratorio = vacuna.Laboratorio,
            EnfermedadesPrevine = vacuna.EnfermedadesPrevine,
            Activo = vacuna.Activo,
            FechaCreacion = vacuna.FechaCreacion,
            FechaModificacion = vacuna.FechaModificacion
        };
    }
}
