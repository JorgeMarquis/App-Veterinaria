namespace Veterinaria.Application.DTOs.ClienteDTOs;

/// <summary>
/// DTO para lectura de Cliente
/// </summary>
public class ClienteDTO
{
    public int IdCliente { get; set; }
    public string NombreCompleto { get; set; } = null!;
    public string TipoIdentificacion { get; set; } = null!;
    public string NumIdentificacion { get; set; } = null!;
    public string? Email { get; set; }
    public string TelefonoPrincipal { get; set; } = null!;
    public string? TelefonoAlternativo { get; set; }
    public string Direccion { get; set; } = null!;
    public string Ciudad { get; set; } = null!;
    public string? ContactoEmergencia { get; set; }
    public string? TelefonoEmergencia { get; set; }
    public string? Observaciones { get; set; }
}
