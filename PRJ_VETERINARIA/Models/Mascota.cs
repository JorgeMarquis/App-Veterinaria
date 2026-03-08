using System;
using System.Collections.Generic;

namespace PRJ_VETERINARIA.Models;

public partial class Mascota
{
    public int IdMascota { get; set; }

    public int IdCliente { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdEspecie { get; set; }

    public int? IdRaza { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public string Sexo { get; set; } = null!;

    public string? ColorPelaje { get; set; }

    public string MicrochipId { get; set; } = null!;

    public string? FotoUrl { get; set; }

    public DateOnly? FechaFallecimiento { get; set; }

    public bool Activo { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual ICollection<AtencionMascotaAdicional> AtencionMascotaAdicionals { get; set; } = new List<AtencionMascotaAdicional>();

    public virtual ICollection<Atencion> Atencions { get; set; } = new List<Atencion>();

    public virtual ICollection<Desparasitacion> Desparasitacions { get; set; } = new List<Desparasitacion>();

    public virtual ICollection<HistorialVacuna> HistorialVacunas { get; set; } = new List<HistorialVacuna>();

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Especie IdEspecieNavigation { get; set; } = null!;

    public virtual Raza? IdRazaNavigation { get; set; }
}
