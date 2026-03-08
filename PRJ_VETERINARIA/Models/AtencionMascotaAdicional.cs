using System;
using System.Collections.Generic;

namespace PRJ_VETERINARIA.Models;

public partial class AtencionMascotaAdicional
{
    public int IdGrupo { get; set; }

    public int IdMascota { get; set; }

    public bool EsPrincipal { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual GrupoAtencion IdGrupoNavigation { get; set; } = null!;

    public virtual Mascota IdMascotaNavigation { get; set; } = null!;
}
