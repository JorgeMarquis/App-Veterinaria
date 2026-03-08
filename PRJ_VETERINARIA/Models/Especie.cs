using System;
using System.Collections.Generic;

namespace PRJ_VETERINARIA.Models;

public partial class Especie
{
    public int IdEspecie { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual ICollection<Mascota> Mascota { get; set; } = new List<Mascota>();

    public virtual ICollection<Raza> Razas { get; set; } = new List<Raza>();
}
