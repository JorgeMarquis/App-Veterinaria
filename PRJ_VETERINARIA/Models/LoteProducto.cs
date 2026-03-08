using System;
using System.Collections.Generic;

namespace PRJ_VETERINARIA.Models;

public partial class LoteProducto
{
    public int IdLote { get; set; }

    public int IdProducto { get; set; }

    public string NumeroLote { get; set; } = null!;

    public DateOnly FechaVencimiento { get; set; }

    public DateOnly? FechaFabricacion { get; set; }

    public decimal CantidadInicial { get; set; }

    public decimal CantidadActual { get; set; }

    public DateTime FechaIngreso { get; set; }

    public bool Activo { get; set; }

    public string? Observaciones { get; set; }

    public virtual ICollection<HistorialVacuna> HistorialVacunas { get; set; } = new List<HistorialVacuna>();

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual ICollection<Kardex> Kardices { get; set; } = new List<Kardex>();
}
