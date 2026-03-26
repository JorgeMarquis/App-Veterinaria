using System;
using System.Collections.Generic;

namespace PRJ_VETERINARIA.DataAccessLayer.Models;

public partial class FormaPago
{
    public int IdFormaPago { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal ComisionPorcentaje { get; set; }

    public bool RequiereAutorizacion { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
