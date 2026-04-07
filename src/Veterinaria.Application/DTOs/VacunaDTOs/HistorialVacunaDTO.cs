namespace Veterinaria.Application.DTOs.VacunaDTOs;

/// <summary>
/// DTO para lectura del Historial de Vacuna
/// </summary>
public class HistorialVacunaDTO
{
    public int IdHistorial { get; set; }
    public int IdMascota { get; set; }
    public int IdVacuna { get; set; }
    public DateTime FechaAplicacion { get; set; }
    public DateOnly? FechaProximoRefuerzo { get; set; }
    public int IdVeterinario { get; set; }
    public int IdLote { get; set; }
    public int DosisNumero { get; set; }
    public string? ReaccionesAdversas { get; set; }
    public string? Observaciones { get; set; }
    public DateTime CreatedAt { get; set; }
}
