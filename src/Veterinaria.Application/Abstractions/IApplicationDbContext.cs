using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Veterinaria.Domain.Entities;

namespace Veterinaria.Application.Abstractions;

public interface IApplicationDbContext
{
    DatabaseFacade Database { get; }

    EntityEntry Add(object entity);

    EntityEntry Update(object entity);

    DbSet<Atencion> Atencions { get; }
    DbSet<AtencionMascotaAdicional> AtencionMascotaAdicionals { get; }
    DbSet<AuditLog> AuditLogs { get; }
    DbSet<CategoriaProducto> CategoriaProductos { get; }
    DbSet<Cliente> Clientes { get; }
    DbSet<Compra> Compras { get; }
    DbSet<Desparasitacion> Desparasitacions { get; }
    DbSet<DetalleAtencion> DetalleAtencions { get; }
    DbSet<DetalleCompra> DetalleCompras { get; }
    DbSet<DetalleVenta> DetalleVenta { get; }
    DbSet<Especie> Especies { get; }
    DbSet<FormaPago> FormaPagos { get; }
    DbSet<GrupoAtencion> GrupoAtencions { get; }
    DbSet<HistorialVacuna> HistorialVacunas { get; }
    DbSet<Kardex> Kardices { get; }
    DbSet<LoteProducto> LoteProductos { get; }
    DbSet<Mascota> Mascota { get; }
    DbSet<Producto> Productos { get; }
    DbSet<Proveedor> Proveedors { get; }
    DbSet<Raza> Razas { get; }
    DbSet<Servicio> Servicios { get; }
    DbSet<TipoServicio> TipoServicios { get; }
    DbSet<Usuario> Usuarios { get; }
    DbSet<Vacuna> Vacunas { get; }
    DbSet<Venta> Venta { get; }

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
