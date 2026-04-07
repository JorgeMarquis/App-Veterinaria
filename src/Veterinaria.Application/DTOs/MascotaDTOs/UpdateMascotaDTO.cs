using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Application.DTOs.MascotaDTOs;

/// <summary>
/// DTO para actualizar una Mascota existente
/// </summary>
public class UpdateMascotaDTO
{
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

    public string? FotoUrl { get; set; }

    public bool Activo { get; set; }
}
