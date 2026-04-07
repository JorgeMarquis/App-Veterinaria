using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Application.DTOs.ClienteDTOs;

/// <summary>
/// DTO para crear un nuevo Cliente
/// </summary>
public class CreateClienteDTO
{
    [Required(ErrorMessage = "El nombre completo es requerido")]
    [StringLength(100)]
    public string NombreCompleto { get; set; } = null!;

    [Required(ErrorMessage = "El tipo de identificación es requerido")]
    [StringLength(50)]
    public string TipoIdentificacion { get; set; } = null!;

    [Required(ErrorMessage = "El número de identificación es requerido")]
    [StringLength(50)]
    public string NumIdentificacion { get; set; } = null!;

    [EmailAddress]
    public string? Email { get; set; }

    [Required(ErrorMessage = "El teléfono principal es requerido")]
    [Phone]
    public string TelefonoPrincipal { get; set; } = null!;

    [Phone]
    public string? TelefonoAlternativo { get; set; }

    [Required(ErrorMessage = "La dirección es requerida")]
    [StringLength(200)]
    public string Direccion { get; set; } = null!;

    [Required(ErrorMessage = "La ciudad es requerida")]
    [StringLength(100)]
    public string Ciudad { get; set; } = null!;

    public string? ContactoEmergencia { get; set; }

    [Phone]
    public string? TelefonoEmergencia { get; set; }

    public string? Observaciones { get; set; }
}
