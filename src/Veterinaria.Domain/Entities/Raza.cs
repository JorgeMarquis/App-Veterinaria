using System;
using System.Collections.Generic;

namespace Veterinaria.Domain.Entities;

public partial class Raza
{
    public int IdRaza { get; set; }

    public int IdEspecie { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Dimension { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual Especie IdEspecieNavigation { get; set; } = null!;

    public virtual ICollection<Mascota> Mascota { get; set; } = new List<Mascota>();
}
