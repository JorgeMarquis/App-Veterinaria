using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Application.DTOs.VacunaDTOs;

/// <summary>
/// DTO para crear una nueva Vacuna
/// </summary>
public class CreateVacunaDTO
{
    [Required(ErrorMessage = "El nombre de la vacuna es requerido")]
    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El tipo de vacuna es requerido")]
    [StringLength(50)]
    public string Tipo { get; set; } = null!;

    [Range(0, 120, ErrorMessage = "La frecuencia debe estar entre 0 y 120 meses")]
    public int? FrecuenciaRefuerzoMeses { get; set; }

    [Range(0, 104, ErrorMessage = "La edad debe estar entre 0 y 104 semanas")]
    public int? EdadPrimeraDosisSemanas { get; set; }

    public string? Laboratorio { get; set; }

    public string? EnfermedadesPrevine { get; set; }

    public bool Activo { get; set; } = true;
}
