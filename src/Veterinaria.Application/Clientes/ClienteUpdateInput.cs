namespace Veterinaria.Application.Clientes;

public sealed record ClienteUpdateInput(
    int IdCliente,
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
    string? Observaciones,
    bool Activo);
