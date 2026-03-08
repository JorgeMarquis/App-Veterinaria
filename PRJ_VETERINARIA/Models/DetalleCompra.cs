using System;
using System.Collections.Generic;

namespace PRJ_VETERINARIA.Models;

public partial class DetalleCompra
{
    public int IdDetalle { get; set; }

    public int IdCompra { get; set; }

    public int IdProducto { get; set; }

    public decimal Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public string Lote { get; set; } = null!;

    public DateOnly FechaVencimiento { get; set; }

    public DateOnly? FechaFabricacion { get; set; }

    public string? Observaciones { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Compra IdCompraNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
