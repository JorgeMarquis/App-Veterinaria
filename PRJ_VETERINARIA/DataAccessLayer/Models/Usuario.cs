using System;
using System.Collections.Generic;

namespace PRJ_VETERINARIA.DataAccessLayer.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string NombreCompleto { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public string? Especialidad { get; set; }

    public string? NumeroColegiado { get; set; }

    public string? Telefono { get; set; }

    public DateTime? UltimoAcceso { get; set; }

    public bool Activo { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual ICollection<Atencion> Atencions { get; set; } = new List<Atencion>();

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual ICollection<Desparasitacion> Desparasitacions { get; set; } = new List<Desparasitacion>();

    public virtual ICollection<GrupoAtencion> GrupoAtencions { get; set; } = new List<GrupoAtencion>();

    public virtual ICollection<HistorialVacuna> HistorialVacunas { get; set; } = new List<HistorialVacuna>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
