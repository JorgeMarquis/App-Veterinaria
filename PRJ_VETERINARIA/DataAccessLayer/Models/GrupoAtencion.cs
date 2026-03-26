using System;
using System.Collections.Generic;

namespace PRJ_VETERINARIA.DataAccessLayer.Models;

public partial class GrupoAtencion
{
    public int IdGrupo { get; set; }

    public DateOnly Fecha { get; set; }

    public int IdVeterinario { get; set; }

    public string? Observaciones { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual AtencionMascotaAdicional? AtencionMascotaAdicional { get; set; }

    public virtual ICollection<Atencion> Atencions { get; set; } = new List<Atencion>();

    public virtual Usuario IdVeterinarioNavigation { get; set; } = null!;
}
