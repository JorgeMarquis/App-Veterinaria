using System;
using System.Collections.Generic;

namespace Veterinaria.Domain.Entities;

public partial class DetalleAtencion
{
    public int IdDetalle { get; set; }

    public int IdAtencion { get; set; }

    public int? IdProducto { get; set; }

    public int? IdServicio { get; set; }

    public decimal Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal DescuentoPorcentaje { get; set; }

    public decimal? Subtotal { get; set; }

    public string TipoItem { get; set; } = null!;

    public string? Dosis { get; set; }

    public string? Frecuencia { get; set; }

    public int? DuracionDias { get; set; }

    public string? Instrucciones { get; set; }

    public string? Observaciones { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Atencion IdAtencionNavigation { get; set; } = null!;

    public virtual Producto? IdProductoNavigation { get; set; }

    public virtual Servicio? IdServicioNavigation { get; set; }
}
