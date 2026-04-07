using Veterinaria.Domain.Entities;

namespace Veterinaria.Application.Clientes;

public interface IClienteService
{
    Task<IReadOnlyList<Cliente>> ListarActivosAsync(CancellationToken cancellationToken = default);

    Task<Cliente?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Cliente?> ObtenerParaEdicionAsync(int id, CancellationToken cancellationToken = default);

    Task<ClienteOperationResult> CrearAsync(ClienteCreateInput input, CancellationToken cancellationToken = default);

    Task<ClienteOperationResult> ActualizarAsync(ClienteUpdateInput input, CancellationToken cancellationToken = default);

    Task<Cliente?> ObtenerParaEliminacionAsync(int id, CancellationToken cancellationToken = default);

    Task<ClienteOperationResult> EliminarAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> ExisteAsync(int id, CancellationToken cancellationToken = default);
}
