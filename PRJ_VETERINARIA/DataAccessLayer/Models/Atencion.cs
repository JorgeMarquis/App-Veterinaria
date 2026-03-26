using System;
using System.Collections.Generic;

namespace PRJ_VETERINARIA.DataAccessLayer.Models;

public partial class Atencion
{
    public int IdAtencion { get; set; }

    public int IdMascota { get; set; }

    public int IdVeterinario { get; set; }

    public int? IdGrupo { get; set; }

    public DateTime FechaHoraInicio { get; set; }

    public DateTime? FechaHoraFin { get; set; }

    public string TipoAtencion { get; set; } = null!;

    public string MotivoConsulta { get; set; } = null!;

    public string? Sintomas { get; set; }

    public string? ExamenFisico { get; set; }

    public string? Diagnostico { get; set; }

    public string? Tratamiento { get; set; }

    public string? Recomendaciones { get; set; }

    public decimal? PesoAtencion { get; set; }

    public decimal? Temperatura { get; set; }

    public int? FrecuenciaCardiaca { get; set; }

    public int? FrecuenciaRespiratoria { get; set; }

    public string Estado { get; set; } = null!;

    public string? Observaciones { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual ICollection<DetalleAtencion> DetalleAtencions { get; set; } = new List<DetalleAtencion>();

    public virtual GrupoAtencion? IdGrupoNavigation { get; set; }

    public virtual Mascota IdMascotaNavigation { get; set; } = null!;

    public virtual Usuario IdVeterinarioNavigation { get; set; } = null!;

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
