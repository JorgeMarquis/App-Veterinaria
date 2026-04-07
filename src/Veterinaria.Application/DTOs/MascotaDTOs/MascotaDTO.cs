namespace Veterinaria.Application.DTOs.MascotaDTOs;

/// <summary>
/// DTO para lectura de Mascota
/// </summary>
public class MascotaDTO
{
    public int IdMascota { get; set; }
    public int IdCliente { get; set; }
    public string Nombre { get; set; } = null!;
    public int IdEspecie { get; set; }
    public int? IdRaza { get; set; }
    public DateOnly? FechaNacimiento { get; set; }
    public string Sexo { get; set; } = null!;
    public string? ColorPelaje { get; set; }
    public string MicrochipId { get; set; } = null!;
    public string? FotoUrl { get; set; }
    public DateOnly? FechaFallecimiento { get; set; }
    public bool Activo { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
}
