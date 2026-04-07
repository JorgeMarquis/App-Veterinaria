using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Application.DTOs.MascotaDTOs;

/// <summary>
/// DTO para crear una nueva Mascota
/// </summary>
public class CreateMascotaDTO
{
    [Required(ErrorMessage = "El ID del cliente es requerido")]
    public int IdCliente { get; set; }

    [Required(ErrorMessage = "El nombre de la mascota es requerido")]
    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "La especie es requerida")]
    public int IdEspecie { get; set; }

    public int? IdRaza { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    [Required(ErrorMessage = "El sexo es requerido")]
    public string Sexo { get; set; } = null!;

    public string? ColorPelaje { get; set; }

    [Required(ErrorMessage = "El microchip ID es requerido")]
    public string MicrochipId { get; set; } = null!;

    public string? FotoUrl { get; set; }
}
