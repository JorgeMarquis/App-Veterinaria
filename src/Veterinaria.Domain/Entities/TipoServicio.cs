using System;
using System.Collections.Generic;

namespace Veterinaria.Domain.Entities;

public partial class TipoServicio
{
    public int IdTipoServicio { get; set; }

    public string Nombre { get; set; } = null!;

    public bool EsMedico { get; set; }

    public int? DuracionEstimadaMin { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}
