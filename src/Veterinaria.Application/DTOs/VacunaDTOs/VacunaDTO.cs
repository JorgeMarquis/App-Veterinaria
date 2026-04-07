namespace Veterinaria.Application.DTOs.VacunaDTOs;

/// <summary>
/// DTO para lectura de Vacuna
/// </summary>
public class VacunaDTO
{
    public int IdVacuna { get; set; }
    public string Nombre { get; set; } = null!;
    public string Tipo { get; set; } = null!;
    public int? FrecuenciaRefuerzoMeses { get; set; }
    public int? EdadPrimeraDosisSemanas { get; set; }
    public string? Laboratorio { get; set; }
    public string? EnfermedadesPrevine { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
}
