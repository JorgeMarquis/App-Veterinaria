using System;
using System.Collections.Generic;

namespace Veterinaria.Domain.Entities;

public partial class Kardex
{
    public int IdKardex { get; set; }

    public int IdProducto { get; set; }

    public int IdLote { get; set; }

    public decimal Cantidad { get; set; }

    public decimal StockAnterior { get; set; }

    public decimal StockNuevo { get; set; }

    public string TipoMovimiento { get; set; } = null!;

    public int? IdDocumentoRef { get; set; }

    public string? TipoDocumentoRef { get; set; }

    public int? IdUsuario { get; set; }

    public string? NombreUsuario { get; set; }

    public DateTime Fecha { get; set; }

    public string? Observaciones { get; set; }

    public virtual LoteProducto IdLoteNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
