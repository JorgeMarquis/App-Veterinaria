using Microsoft.EntityFrameworkCore;
using Veterinaria.Application.Abstractions;
using Veterinaria.Domain.Entities;

namespace Veterinaria.Application.Clientes;

public sealed class ClienteService : IClienteService
{
    private readonly IApplicationDbContext _context;

    public ClienteService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Cliente>> ListarActivosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .Where(c => c.Activo)
            .OrderBy(c => c.NombreCompleto)
            .ToListAsync(cancellationToken);
    }

    public Task<Cliente?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.Clientes
            .FirstOrDefaultAsync(m => m.IdCliente == id, cancellationToken);
    }

    public async Task<Cliente?> ObtenerParaEdicionAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Clientes.FindAsync(new object[] { id }, cancellationToken);
    }

    public Task<Cliente?> ObtenerParaEliminacionAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.Clientes
            .FirstOrDefaultAsync(m => m.IdCliente == id, cancellationToken);
    }

    public async Task<ClienteOperationResult> CrearAsync(ClienteCreateInput input, CancellationToken cancellationToken = default)
    {
        var cliente = new Cliente
        {
            TipoIdentificacion = input.TipoIdentificacion,
            NumIdentificacion = input.NumIdentificacion.Trim(),
            NombreCompleto = input.NombreCompleto.Trim(),
            TelefonoPrincipal = input.TelefonoPrincipal.Trim(),
            Direccion = input.Direccion.Trim(),
            Ciudad = input.Ciudad.Trim(),
            Email = input.Email?.Trim().ToLower(),
            TelefonoAlternativo = input.TelefonoAlternativo?.Trim(),
            ContactoEmergencia = input.ContactoEmergencia?.Trim(),
            TelefonoEmergencia = input.TelefonoEmergencia?.Trim(),
            Observaciones = input.Observaciones?.Trim(),
            Activo = true,
            CreatedAt = DateTime.Now
        };

        try
        {
            _context.Add(cliente);
            await _context.SaveChangesAsync(cancellationToken);
            return ClienteOperationResult.Ok();
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException?.Message.Contains("UK_Cliente_Identificacion") == true)
                return ClienteOperationResult.Fail(ClienteErrorKind.DuplicateIdentificacion);
            return ClienteOperationResult.Fail(ClienteErrorKind.GenericSave);
        }
    }

    public async Task<ClienteOperationResult> ActualizarAsync(ClienteUpdateInput input, CancellationToken cancellationToken = default)
    {
        var cliente = await _context.Clientes.FindAsync([input.IdCliente], cancellationToken);
        if (cliente is null)
            return ClienteOperationResult.Fail(ClienteErrorKind.NotFound);

        cliente.TipoIdentificacion = input.TipoIdentificacion;
        cliente.NumIdentificacion = input.NumIdentificacion.Trim();
        cliente.NombreCompleto = input.NombreCompleto.Trim();
        cliente.TelefonoPrincipal = input.TelefonoPrincipal.Trim();
        cliente.Direccion = input.Direccion.Trim();
        cliente.Ciudad = input.Ciudad.Trim();
        cliente.Email = input.Email?.Trim().ToLower();
        cliente.TelefonoAlternativo = input.TelefonoAlternativo?.Trim();
        cliente.ContactoEmergencia = input.ContactoEmergencia?.Trim();
        cliente.TelefonoEmergencia = input.TelefonoEmergencia?.Trim();
        cliente.Observaciones = input.Observaciones?.Trim();
        cliente.Activo = input.Activo;
        cliente.UpdatedAt = DateTime.Now;

        try
        {
            _context.Update(cliente);
            await _context.SaveChangesAsync(cancellationToken);
            return ClienteOperationResult.Ok();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ExisteAsync(input.IdCliente, cancellationToken))
                return ClienteOperationResult.Fail(ClienteErrorKind.NotFound);
            throw;
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException?.Message.Contains("UK_Cliente_Identificacion") == true)
                return ClienteOperationResult.Fail(ClienteErrorKind.DuplicateIdentificacion);
            return ClienteOperationResult.Fail(ClienteErrorKind.GenericUpdate);
        }
    }

    public async Task<ClienteOperationResult> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var cliente = await _context.Clientes.FindAsync(new object[] { id }, cancellationToken);
        if (cliente is null)
            return ClienteOperationResult.Fail(ClienteErrorKind.NotFound);

        try
        {
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync(cancellationToken);
            return ClienteOperationResult.Ok();
        }
        catch (DbUpdateException)
        {
            return ClienteOperationResult.Fail(ClienteErrorKind.DeleteConstraint);
        }
    }

    public Task<bool> ExisteAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.Clientes.AnyAsync(e => e.IdCliente == id, cancellationToken);
    }
}
