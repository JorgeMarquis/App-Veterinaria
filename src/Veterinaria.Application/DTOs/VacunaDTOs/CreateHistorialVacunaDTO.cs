using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Application.DTOs.VacunaDTOs;

/// <summary>
/// DTO para crear un nuevo Historial de Vacuna
/// </summary>
public class CreateHistorialVacunaDTO
{
    [Required(ErrorMessage = "El ID de la mascota es requerido")]
    public int IdMascota { get; set; }

    [Required(ErrorMessage = "El ID de la vacuna es requerido")]
    public int IdVacuna { get; set; }

    [Required(ErrorMessage = "La fecha de aplicación es requerida")]
    public DateTime FechaAplicacion { get; set; }

    public DateOnly? FechaProximoRefuerzo { get; set; }

    [Required(ErrorMessage = "El ID del veterinario es requerido")]
    public int IdVeterinario { get; set; }

    [Required(ErrorMessage = "El ID del lote es requerido")]
    public int IdLote { get; set; }

    [Required(ErrorMessage = "El número de dosis es requerido")]
    [Range(1, 10)]
    public int DosisNumero { get; set; }

    public string? ReaccionesAdversas { get; set; }

    public string? Observaciones { get; set; }
}
