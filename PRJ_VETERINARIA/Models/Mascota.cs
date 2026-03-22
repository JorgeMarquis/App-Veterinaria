using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRJ_VETERINARIA.Models;

public partial class Mascota
{
    [Key]
    [Display(Name = "ID Mascota")]
    public int IdMascota { get; set; }

    
    [Display(Name = "ID Cliente")]
    public int IdCliente { get; set; }

    
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = null!;

    
    [Display(Name = "ID Especie")]
    public int IdEspecie { get; set; }

    [Display(Name = "ID Raza")]
    public int? IdRaza { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Fecha de Nacimiento")]
    public DateOnly? FechaNacimiento { get; set; }

    [Display(Name = "Sexo")]
    public string Sexo { get; set; } = null!;

    [Display(Name = "Color de Pelaje")]
    public string? ColorPelaje { get; set; }

    [Display(Name = "Microchip ID")]
    public string MicrochipId { get; set; } = null!;

    [Display(Name = "Foto")]
    public string? FotoUrl { get; set; }

    [Display(Name = "Fecha de Fallecimiento")]
    public DateOnly? FechaFallecimiento { get; set; }

    [Display(Name = "Activo")]
    public bool Activo { get; set; }

    [Display(Name = "Fecha de Creacion")]
    public DateTime CreatedAt { get; set; }

    [Display(Name = "Creado Por")]
    public string? CreatedBy { get; set; }

    [Display(Name = "Fecha de Actualización")]
    public DateTime? UpdatedAt { get; set; }

    [Display(Name = "Actualizado Por")]
    public string? UpdatedBy { get; set; }

    public virtual ICollection<AtencionMascotaAdicional> AtencionMascotaAdicionals { get; set; } = new List<AtencionMascotaAdicional>();

    public virtual ICollection<Atencion> Atencions { get; set; } = new List<Atencion>();

    public virtual ICollection<Desparasitacion> Desparasitacions { get; set; } = new List<Desparasitacion>();

    public virtual ICollection<HistorialVacuna> HistorialVacunas { get; set; } = new List<HistorialVacuna>();

    [ForeignKey("IdCliente")]
    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    [ForeignKey("IdEspecie")]
    public virtual Especie IdEspecieNavigation { get; set; } = null!;

    [ForeignKey("IdRaza")]
    public virtual Raza? IdRazaNavigation { get; set; }
}
