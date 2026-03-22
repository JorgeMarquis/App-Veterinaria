using System;
using System.Collections.Generic;

namespace PRJ_VETERINARIA.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public int IdCategoria { get; set; }

    public string CodigoBarras { get; set; } = null!;

    public string CodigoInterno { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string TipoProducto { get; set; } = null!;

    public decimal PrecioVenta { get; set; }

    public decimal PrecioCosto { get; set; }

    public string UnidadMedida { get; set; } = null!;

    public bool RequiereReceta { get; set; }

    public bool Activo { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual ICollection<Desparasitacion> Desparasitacions { get; set; } = new List<Desparasitacion>();

    public virtual ICollection<DetalleAtencion> DetalleAtencions { get; set; } = new List<DetalleAtencion>();

    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();

    public virtual CategoriaProducto IdCategoriaNavigation { get; set; } = null!;

    public virtual ICollection<Kardex> Kardices { get; set; } = new List<Kardex>();

    public virtual ICollection<LoteProducto> LoteProductos { get; set; } = new List<LoteProducto>();
}
