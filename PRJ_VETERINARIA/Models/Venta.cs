using System;
using System.Collections.Generic;

namespace PRJ_VETERINARIA.Models;

public partial class Venta
{
    public int IdVenta { get; set; }

    public int IdCliente { get; set; }

    public int? IdAtencion { get; set; }
    public int IdUsuario { get; set; }

    public DateTime Fecha { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Descuento { get; set; }

    public decimal Impuestos { get; set; }

    public decimal Total { get; set; }

    public int IdFormaPago { get; set; }

    public string EstadoPago { get; set; } = null!;

    public string NumeroComprobante { get; set; } = null!;

    public string? RutaPdf { get; set; }

    public string? Observaciones { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual ICollection<DetalleVentum> DetalleVenta { get; set; } = new List<DetalleVentum>();

    public virtual Atencion? IdAtencionNavigation { get; set; }

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual FormaPago IdFormaPagoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
