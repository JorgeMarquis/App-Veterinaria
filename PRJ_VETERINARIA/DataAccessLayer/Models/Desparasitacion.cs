using System;
using System.Collections.Generic;

namespace PRJ_VETERINARIA.DataAccessLayer.Models;

public partial class Desparasitacion
{
    public int IdDesparasitacion { get; set; }

    public int IdMascota { get; set; }

    public string TipoDesparasitacion { get; set; } = null!;

    public int IdProducto { get; set; }

    public DateTime FechaAplicacion { get; set; }

    public DateOnly? FechaProxima { get; set; }

    public int IdVeterinario { get; set; }

    public decimal? PesoAplicacion { get; set; }

    public string? DosisAplicada { get; set; }

    public string? Observaciones { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Mascota IdMascotaNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual Usuario IdVeterinarioNavigation { get; set; } = null!;
}
