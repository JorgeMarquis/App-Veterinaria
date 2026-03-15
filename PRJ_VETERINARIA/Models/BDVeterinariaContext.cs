using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PRJ_VETERINARIA.Models;

public partial class BDVeterinariaContext : DbContext
{
    public BDVeterinariaContext()
    {
    }

    public BDVeterinariaContext(DbContextOptions<BDVeterinariaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Atencion> Atencions { get; set; }

    public virtual DbSet<AtencionMascotaAdicional> AtencionMascotaAdicionals { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<CategoriaProducto> CategoriaProductos { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<Desparasitacion> Desparasitacions { get; set; }

    public virtual DbSet<DetalleAtencion> DetalleAtencions { get; set; }

    public virtual DbSet<DetalleCompra> DetalleCompras { get; set; }

    public virtual DbSet<DetalleVentum> DetalleVenta { get; set; }

    public virtual DbSet<Especie> Especies { get; set; }

    public virtual DbSet<FormaPago> FormaPagos { get; set; }

    public virtual DbSet<GrupoAtencion> GrupoAtencions { get; set; }

    public virtual DbSet<HistorialVacuna> HistorialVacunas { get; set; }

    public virtual DbSet<Kardex> Kardices { get; set; }

    public virtual DbSet<LoteProducto> LoteProductos { get; set; }

    public virtual DbSet<Mascota> Mascota { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Proveedor> Proveedors { get; set; }

    public virtual DbSet<Raza> Razas { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<TipoServicio> TipoServicios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Vacuna> Vacunas { get; set; }

    public virtual DbSet<Venta> Venta { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Atencion>(entity =>
        {
            entity.HasKey(e => e.IdAtencion);

            entity.ToTable("Atencion");

            entity.HasIndex(e => e.FechaHoraInicio, "IX_Atencion_Fecha").HasFilter("([Estado]<>'Cancelada')");

            entity.HasIndex(e => e.IdMascota, "IX_Atencion_Mascota");

            entity.HasIndex(e => new { e.IdVeterinario, e.FechaHoraInicio }, "IX_Atencion_Veterinario");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.CreatedBy).HasMaxLength(150);
            entity.Property(e => e.Diagnostico).HasMaxLength(2000);
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValue("Programada");
            entity.Property(e => e.ExamenFisico).HasMaxLength(2000);
            entity.Property(e => e.FechaHoraInicio).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.MotivoConsulta).HasMaxLength(1000);
            entity.Property(e => e.Observaciones).HasMaxLength(500);
            entity.Property(e => e.PesoAtencion).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Recomendaciones).HasMaxLength(2000);
            entity.Property(e => e.Sintomas).HasMaxLength(2000);
            entity.Property(e => e.Temperatura).HasColumnType("decimal(4, 1)");
            entity.Property(e => e.TipoAtencion).HasMaxLength(50);
            entity.Property(e => e.Tratamiento).HasMaxLength(2000);
            entity.Property(e => e.UpdatedBy).HasMaxLength(150);

            entity.HasOne(d => d.IdGrupoNavigation).WithMany(p => p.Atencions)
                .HasForeignKey(d => d.IdGrupo)
                .HasConstraintName("FK_Atencion_Grupo");

            entity.HasOne(d => d.IdMascotaNavigation).WithMany(p => p.Atencions)
                .HasForeignKey(d => d.IdMascota)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Atencion_Mascota");

            entity.HasOne(d => d.IdVeterinarioNavigation).WithMany(p => p.Atencions)
                .HasForeignKey(d => d.IdVeterinario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Atencion_Vet");
        });

        modelBuilder.Entity<AtencionMascotaAdicional>(entity =>
        {
            entity.HasKey(e => new { e.IdGrupo, e.IdMascota });

            entity.ToTable("AtencionMascotaAdicional");

            entity.HasIndex(e => e.IdGrupo, "UX_AtencionMascota_UnPrincipal")
                .IsUnique()
                .HasFilter("([EsPrincipal]=(1))");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.IdGrupoNavigation).WithOne(p => p.AtencionMascotaAdicional)
                .HasForeignKey<AtencionMascotaAdicional>(d => d.IdGrupo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AtencionMascota_Grupo");

            entity.HasOne(d => d.IdMascotaNavigation).WithMany(p => p.AtencionMascotaAdicionals)
                .HasForeignKey(d => d.IdMascota)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AtencionMascota_Mascota");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.IdAuditLog);

            entity.ToTable("AuditLog");

            entity.HasIndex(e => new { e.IdRegistro, e.Tabla }, "IX_AuditLog_Registro");

            entity.HasIndex(e => new { e.Tabla, e.Fecha }, "IX_AuditLog_Tabla_Fecha");

            entity.Property(e => e.Accion).HasMaxLength(10);
            entity.Property(e => e.Fecha).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.IpOrigen).HasMaxLength(50);
            entity.Property(e => e.NombreUsuario).HasMaxLength(150);
            entity.Property(e => e.Tabla).HasMaxLength(100);
        });

        modelBuilder.Entity<CategoriaProducto>(entity =>
        {
            entity.HasKey(e => e.IdCategoria);

            entity.ToTable("CategoriaProducto");

            entity.HasIndex(e => e.Nombre, "UK_CategoriaProducto_Nombre").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion).HasMaxLength(300);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente);

            entity.ToTable("Cliente");

            entity.HasIndex(e => e.NombreCompleto, "IX_Cliente_Nombre").HasFilter("([Activo]=(1))");

            entity.HasIndex(e => e.NumIdentificacion, "IX_Cliente_NumIdentificacion").HasFilter("([Activo]=(1))");

            entity.HasIndex(e => new { e.TipoIdentificacion, e.NumIdentificacion }, "UK_Cliente_Identificacion").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Ciudad).HasMaxLength(100);
            entity.Property(e => e.ContactoEmergencia).HasMaxLength(150);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.CreatedBy).HasMaxLength(150);
            entity.Property(e => e.Direccion).HasMaxLength(300);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.NombreCompleto).HasMaxLength(150);
            entity.Property(e => e.NumIdentificacion).HasMaxLength(20);
            entity.Property(e => e.Observaciones).HasMaxLength(500);
            entity.Property(e => e.TelefonoAlternativo).HasMaxLength(20);
            entity.Property(e => e.TelefonoEmergencia).HasMaxLength(20);
            entity.Property(e => e.TelefonoPrincipal).HasMaxLength(20);
            entity.Property(e => e.TipoIdentificacion).HasMaxLength(30);
            entity.Property(e => e.UpdatedBy).HasMaxLength(150);
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.IdCompra);

            entity.ToTable("Compra");

            entity.HasIndex(e => e.Fecha, "IX_Compra_Fecha");

            entity.HasIndex(e => new { e.IdProveedor, e.Fecha }, "IX_Compra_Proveedor");

            entity.HasIndex(e => new { e.IdProveedor, e.NumeroFactura }, "UK_Compra_Factura").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.CreatedBy).HasMaxLength(150);
            entity.Property(e => e.EstadoPago)
                .HasMaxLength(20)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.Fecha).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Impuestos).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.NumeroFactura).HasMaxLength(50);
            entity.Property(e => e.Observaciones).HasMaxLength(500);
            entity.Property(e => e.RutaDocumento).HasMaxLength(500);
            entity.Property(e => e.Subtotal).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.UpdatedBy).HasMaxLength(150);

            entity.HasOne(d => d.IdFormaPagoNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdFormaPago)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Compra_FormaPago");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdProveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Compra_Proveedor");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Compra_Usuario");
        });

        modelBuilder.Entity<Desparasitacion>(entity =>
        {
            entity.HasKey(e => e.IdDesparasitacion);

            entity.ToTable("Desparasitacion");

            entity.HasIndex(e => new { e.IdMascota, e.FechaAplicacion }, "IX_Desparasitacion_Mascota");

            entity.HasIndex(e => e.FechaProxima, "IX_Desparasitacion_Proxima").HasFilter("([FechaProxima] IS NOT NULL)");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.DosisAplicada).HasMaxLength(100);
            entity.Property(e => e.FechaAplicacion).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Observaciones).HasMaxLength(500);
            entity.Property(e => e.PesoAplicacion).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.TipoDesparasitacion).HasMaxLength(20);

            entity.HasOne(d => d.IdMascotaNavigation).WithMany(p => p.Desparasitacions)
                .HasForeignKey(d => d.IdMascota)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Desparasitacion_Mascota");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Desparasitacions)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Desparasitacion_Producto");

            entity.HasOne(d => d.IdVeterinarioNavigation).WithMany(p => p.Desparasitacions)
                .HasForeignKey(d => d.IdVeterinario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Desparasitacion_Vet");
        });

        modelBuilder.Entity<DetalleAtencion>(entity =>
        {
            entity.HasKey(e => e.IdDetalle);

            entity.ToTable("DetalleAtencion");

            entity.HasIndex(e => e.IdAtencion, "IX_DetalleAtencion_Atencion");

            entity.HasIndex(e => e.IdProducto, "IX_DetalleAtencion_Producto").HasFilter("([IdProducto] IS NOT NULL)");

            entity.HasIndex(e => e.IdServicio, "IX_DetalleAtencion_Servicio").HasFilter("([IdServicio] IS NOT NULL)");

            entity.Property(e => e.Cantidad).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.DescuentoPorcentaje).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Dosis).HasMaxLength(100);
            entity.Property(e => e.Frecuencia).HasMaxLength(100);
            entity.Property(e => e.Instrucciones).HasMaxLength(500);
            entity.Property(e => e.Observaciones).HasMaxLength(500);
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Subtotal)
                .HasComputedColumnSql("(([Cantidad]*[PrecioUnitario])*((1)-[DescuentoPorcentaje]/(100.0)))", true)
                .HasColumnType("numeric(34, 12)");
            entity.Property(e => e.TipoItem).HasMaxLength(20);

            entity.HasOne(d => d.IdAtencionNavigation).WithMany(p => p.DetalleAtencions)
                .HasForeignKey(d => d.IdAtencion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleAtencion_Atencion");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleAtencions)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK_DetalleAtencion_Producto");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.DetalleAtencions)
                .HasForeignKey(d => d.IdServicio)
                .HasConstraintName("FK_DetalleAtencion_Servicio");
        });

        modelBuilder.Entity<DetalleCompra>(entity =>
        {
            entity.HasKey(e => e.IdDetalle);

            entity.ToTable("DetalleCompra");

            entity.HasIndex(e => e.IdCompra, "IX_DetalleCompra_Compra");

            entity.HasIndex(e => new { e.IdProducto, e.Lote }, "IX_DetalleCompra_Prod_Lote");

            entity.Property(e => e.Cantidad).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Lote).HasMaxLength(50);
            entity.Property(e => e.Observaciones).HasMaxLength(200);
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.DetalleCompras)
                .HasForeignKey(d => d.IdCompra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleCompra_Compra");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleCompras)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleCompra_Prod");
        });

        modelBuilder.Entity<DetalleVentum>(entity =>
        {
            entity.HasKey(e => e.IdDetalle);

            entity.HasIndex(e => e.IdProducto, "IX_DetalleVenta_Producto");

            entity.HasIndex(e => e.IdVenta, "IX_DetalleVenta_Venta");

            entity.Property(e => e.Cantidad).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.DescuentoPorcentaje).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Lote).HasMaxLength(50);
            entity.Property(e => e.Observaciones).HasMaxLength(200);
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Subtotal)
                .HasComputedColumnSql("(([Cantidad]*[PrecioUnitario])*((1)-[DescuentoPorcentaje]/(100.0)))", true)
                .HasColumnType("numeric(34, 12)");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleVenta_Prod");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdVenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleVenta_Venta");
        });

        modelBuilder.Entity<Especie>(entity =>
        {
            entity.HasKey(e => e.IdEspecie);

            entity.ToTable("Especie");

            entity.HasIndex(e => e.Nombre, "UK_Especie_Nombre").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<FormaPago>(entity =>
        {
            entity.HasKey(e => e.IdFormaPago);

            entity.ToTable("FormaPago");

            entity.HasIndex(e => e.Nombre, "UK_FormaPago_Nombre").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.ComisionPorcentaje).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<GrupoAtencion>(entity =>
        {
            entity.HasKey(e => e.IdGrupo);

            entity.ToTable("GrupoAtencion");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Observaciones).HasMaxLength(500);

            entity.HasOne(d => d.IdVeterinarioNavigation).WithMany(p => p.GrupoAtencions)
                .HasForeignKey(d => d.IdVeterinario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GrupoAtencion_Vet");
        });

        modelBuilder.Entity<HistorialVacuna>(entity =>
        {
            entity.HasKey(e => e.IdHistorial);

            entity.HasIndex(e => e.IdMascota, "IX_HistVac_Mascota");

            entity.HasIndex(e => e.FechaProximoRefuerzo, "IX_HistVac_Refuerzo").HasFilter("([FechaProximoRefuerzo] IS NOT NULL)");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.DosisNumero).HasDefaultValue(1);
            entity.Property(e => e.FechaAplicacion).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Observaciones).HasMaxLength(500);
            entity.Property(e => e.ReaccionesAdversas).HasMaxLength(500);

            entity.HasOne(d => d.IdLoteNavigation).WithMany(p => p.HistorialVacunas)
                .HasForeignKey(d => d.IdLote)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistorialVacunas_Lotes");

            entity.HasOne(d => d.IdMascotaNavigation).WithMany(p => p.HistorialVacunas)
                .HasForeignKey(d => d.IdMascota)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistorialVacunas_Mascota");

            entity.HasOne(d => d.IdVacunaNavigation).WithMany(p => p.HistorialVacunas)
                .HasForeignKey(d => d.IdVacuna)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistorialVacunas_Vacuna");

            entity.HasOne(d => d.IdVeterinarioNavigation).WithMany(p => p.HistorialVacunas)
                .HasForeignKey(d => d.IdVeterinario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistorialVacunas_Vet");
        });

        modelBuilder.Entity<Kardex>(entity =>
        {
            entity.HasKey(e => e.IdKardex);

            entity.ToTable("Kardex");

            entity.HasIndex(e => e.Fecha, "IX_Kardex_Fecha");

            entity.HasIndex(e => new { e.IdProducto, e.Fecha }, "IX_Kardex_Prod_Fecha");

            entity.Property(e => e.Cantidad).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Fecha).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.NombreUsuario).HasMaxLength(150);
            entity.Property(e => e.Observaciones).HasMaxLength(500);
            entity.Property(e => e.StockAnterior).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.StockNuevo).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TipoDocumentoRef).HasMaxLength(20);
            entity.Property(e => e.TipoMovimiento).HasMaxLength(20);

            entity.HasOne(d => d.IdLoteNavigation).WithMany(p => p.Kardices)
                .HasForeignKey(d => d.IdLote)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Kardex_Lote");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Kardices)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Kardex_Producto");
        });

        modelBuilder.Entity<LoteProducto>(entity =>
        {
            entity.HasKey(e => e.IdLote);

            entity.ToTable("LoteProducto");

            entity.HasIndex(e => new { e.IdProducto, e.FechaVencimiento }, "IX_LoteProducto_Producto").HasFilter("([CantidadActual]>(0))");

            entity.HasIndex(e => e.FechaVencimiento, "IX_LoteProducto_Venc").HasFilter("([Activo]=(1) AND [CantidadActual]>(0))");

            entity.HasIndex(e => new { e.IdProducto, e.NumeroLote }, "UK_LoteProducto_ProdLote").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CantidadActual).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CantidadInicial).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.FechaIngreso).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.NumeroLote).HasMaxLength(50);
            entity.Property(e => e.Observaciones).HasMaxLength(200);

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.LoteProductos)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LoteProducto_Prod");
        });

        modelBuilder.Entity<Mascota>(entity =>
        {
            entity.HasKey(e => e.IdMascota);

            entity.HasIndex(e => e.IdCliente, "IX_Mascota_IdCliente");

            entity.HasIndex(e => e.Nombre, "IX_Mascota_Nombre").HasFilter("([Activo]=(1))");

            entity.HasIndex(e => e.MicrochipId, "UK_Mascota_Microchip").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.ColorPelaje).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.CreatedBy).HasMaxLength(150);
            entity.Property(e => e.FotoUrl).HasMaxLength(500);
            entity.Property(e => e.MicrochipId).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Sexo)
                .HasMaxLength(1)
                .HasDefaultValue("M")
                .IsFixedLength();
            entity.Property(e => e.UpdatedBy).HasMaxLength(150);

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Mascota)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Mascota_Cliente");

            entity.HasOne(d => d.IdEspecieNavigation).WithMany(p => p.Mascota)
                .HasForeignKey(d => d.IdEspecie)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Mascota_Especie");

            entity.HasOne(d => d.IdRazaNavigation).WithMany(p => p.Mascota)
                .HasForeignKey(d => d.IdRaza)
                .HasConstraintName("FK_Mascota_Raza");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto);

            entity.ToTable("Producto");

            entity.HasIndex(e => e.IdCategoria, "IX_Producto_Categoria");

            entity.HasIndex(e => e.Nombre, "IX_Producto_Nombre").HasFilter("([Activo]=(1))");

            entity.HasIndex(e => e.CodigoBarras, "UK_Producto_CodigoBarras").IsUnique();

            entity.HasIndex(e => e.CodigoInterno, "UK_Producto_CodigoInterno").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CodigoBarras).HasMaxLength(50);
            entity.Property(e => e.CodigoInterno).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.CreatedBy).HasMaxLength(150);
            entity.Property(e => e.Descripcion).HasMaxLength(1000);
            entity.Property(e => e.Nombre).HasMaxLength(200);
            entity.Property(e => e.PrecioCosto).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PrecioVenta).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TipoProducto).HasMaxLength(20);
            entity.Property(e => e.UnidadMedida)
                .HasMaxLength(20)
                .HasDefaultValue("UNIDAD");
            entity.Property(e => e.UpdatedBy).HasMaxLength(150);

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Producto_Categoria");
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.HasKey(e => e.IdProveedor);

            entity.ToTable("Proveedor");

            entity.HasIndex(e => new { e.TipoIdentificacion, e.NumIdentificacion }, "UK_Proveedor_Identificacion").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.CreatedBy).HasMaxLength(150);
            entity.Property(e => e.Direccion).HasMaxLength(300);
            entity.Property(e => e.Distrito).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.GiroComercial).HasMaxLength(100);
            entity.Property(e => e.NombreContacto).HasMaxLength(150);
            entity.Property(e => e.NumIdentificacion).HasMaxLength(20);
            entity.Property(e => e.Observaciones).HasMaxLength(500);
            entity.Property(e => e.RazonSocial).HasMaxLength(150);
            entity.Property(e => e.TelefonoPrincipal).HasMaxLength(20);
            entity.Property(e => e.TipoIdentificacion).HasMaxLength(30);
            entity.Property(e => e.UpdatedBy).HasMaxLength(150);
        });

        modelBuilder.Entity<Raza>(entity =>
        {
            entity.HasKey(e => e.IdRaza);

            entity.ToTable("Raza");

            entity.HasIndex(e => new { e.Nombre, e.IdEspecie }, "UK_Raza_Nombre").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Dimension).HasMaxLength(20);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Nombre).HasMaxLength(100);

            entity.HasOne(d => d.IdEspecieNavigation).WithMany(p => p.Razas)
                .HasForeignKey(d => d.IdEspecie)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Raza_Especie");
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.IdServicio);

            entity.ToTable("Servicio");

            entity.HasIndex(e => e.IdTipoServicio, "IX_Servicio_Tipo").HasFilter("([Activo]=(1))");

            entity.HasIndex(e => e.CodigoServicio, "UK_Servicio_Codigo").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CodigoServicio).HasMaxLength(20);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.CreatedBy).HasMaxLength(150);
            entity.Property(e => e.Descripcion).HasMaxLength(1000);
            entity.Property(e => e.InstruccionesPreparacion).HasMaxLength(500);
            entity.Property(e => e.Nombre).HasMaxLength(200);
            entity.Property(e => e.Observaciones).HasMaxLength(500);
            entity.Property(e => e.PrecioBase).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdatedBy).HasMaxLength(150);

            entity.HasOne(d => d.IdTipoServicioNavigation).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.IdTipoServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Servicio_Tipo");
        });

        modelBuilder.Entity<TipoServicio>(entity =>
        {
            entity.HasKey(e => e.IdTipoServicio);

            entity.ToTable("TipoServicio");

            entity.HasIndex(e => e.Nombre, "UK_TipoServicio_Nombre").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.EsMedico).HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.Rol, "IX_Usuario_Rol").HasFilter("([Activo]=(1))");

            entity.HasIndex(e => e.Email, "UK_Usuario_Email").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.CreatedBy).HasMaxLength(150);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Especialidad).HasMaxLength(100);
            entity.Property(e => e.NombreCompleto).HasMaxLength(150);
            entity.Property(e => e.NumeroColegiado).HasMaxLength(50);
            entity.Property(e => e.PasswordHash).HasMaxLength(256);
            entity.Property(e => e.Rol).HasMaxLength(30);
            entity.Property(e => e.Telefono).HasMaxLength(20);
            entity.Property(e => e.UpdatedBy).HasMaxLength(150);
        });

        modelBuilder.Entity<Vacuna>(entity =>
        {
            entity.HasKey(e => e.IdVacuna);

            entity.ToTable("Vacuna");

            entity.HasIndex(e => e.Nombre, "UK_Vacuna_Nombre").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.EnfermedadesPrevine).HasMaxLength(500);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Laboratorio).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Tipo).HasMaxLength(30);
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.IdVenta);

            entity.HasIndex(e => e.IdAtencion, "IX_Venta_Atencion").HasFilter("([IdAtencion] IS NOT NULL)");

            entity.HasIndex(e => new { e.IdCliente, e.Fecha }, "IX_Venta_Cliente");

            entity.HasIndex(e => e.Fecha, "IX_Venta_Fecha");

            entity.HasIndex(e => e.NumeroComprobante, "UK_Venta_Comprobante").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.CreatedBy).HasMaxLength(150);
            entity.Property(e => e.Descuento).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.EstadoPago)
                .HasMaxLength(20)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.Fecha).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Impuestos).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.NumeroComprobante).HasMaxLength(50);
            entity.Property(e => e.Observaciones).HasMaxLength(500);
            entity.Property(e => e.RutaPdf)
                .HasMaxLength(500)
                .HasColumnName("RutaPDF");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.UpdatedBy).HasMaxLength(150);

            entity.HasOne(d => d.IdAtencionNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdAtencion)
                .HasConstraintName("FK_Venta_Atencion");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Venta_Cliente");

            entity.HasOne(d => d.IdFormaPagoNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdFormaPago)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Venta_FormaPago");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Venta_Usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
