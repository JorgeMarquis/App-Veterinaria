using System;
using System.Collections.Generic;

namespace Veterinaria.Domain.Entities;

public partial class DetalleVenta
{
    public int IdDetalle { get; set; }

    public int IdVenta { get; set; }

    public int IdProducto { get; set; }

    public decimal Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal DescuentoPorcentaje { get; set; }

    public string? Lote { get; set; }

    public decimal? Subtotal { get; set; }

    public DateOnly? FechaVencimiento { get; set; }

    public string? Observaciones { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual Venta IdVentaNavigation { get; set; } = null!;
}
