using System;
using System.Collections.Generic;

namespace PRJ_VETERINARIA.DataAccessLayer.Models;

public partial class Proveedor
{
    public int IdProveedor { get; set; }

    public string TipoIdentificacion { get; set; } = null!;

    public string NumIdentificacion { get; set; } = null!;

    public string RazonSocial { get; set; } = null!;

    public string? NombreContacto { get; set; }

    public string? Email { get; set; }

    public string? TelefonoPrincipal { get; set; }

    public string? Direccion { get; set; }

    public string? Distrito { get; set; }

    public string? GiroComercial { get; set; }

    public string? Observaciones { get; set; }

    public bool Activo { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
}
