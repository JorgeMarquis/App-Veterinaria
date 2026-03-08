using System;
using System.Collections.Generic;

namespace PRJ_VETERINARIA.Models;

public partial class Servicio
{
    public int IdServicio { get; set; }

    public int IdTipoServicio { get; set; }

    public string CodigoServicio { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal PrecioBase { get; set; }

    public bool RequiereAyuno { get; set; }

    public bool RequierePreparacion { get; set; }

    public string? InstruccionesPreparacion { get; set; }

    public string? Observaciones { get; set; }

    public bool Activo { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual ICollection<DetalleAtencion> DetalleAtencions { get; set; } = new List<DetalleAtencion>();

    public virtual TipoServicio IdTipoServicioNavigation { get; set; } = null!;
}
