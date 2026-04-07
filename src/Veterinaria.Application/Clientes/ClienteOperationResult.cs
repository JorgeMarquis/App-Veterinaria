namespace Veterinaria.Application.Clientes;

public sealed class ClienteOperationResult
{
    public bool Success { get; private init; }
    public ClienteErrorKind? Error { get; private init; }

    public static ClienteOperationResult Ok() => new() { Success = true };

    public static ClienteOperationResult Fail(ClienteErrorKind error) =>
        new() { Success = false, Error = error };
}
