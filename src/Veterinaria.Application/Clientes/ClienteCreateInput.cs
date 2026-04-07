namespace Veterinaria.Application.Clientes;

public sealed record ClienteCreateInput(
    string TipoIdentificacion,
    string NumIdentificacion,
    string NombreCompleto,
    string TelefonoPrincipal,
    string Direccion,
    string Ciudad,
    string? Email,
    string? TelefonoAlternativo,
    string? ContactoEmergencia,
    string? TelefonoEmergencia,
    string? Observaciones);
