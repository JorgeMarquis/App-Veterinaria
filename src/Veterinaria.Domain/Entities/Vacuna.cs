using System;
using System.Collections.Generic;

namespace Veterinaria.Domain.Entities;

public partial class Vacuna
{
    public int IdVacuna { get; set; }

    public string Nombre { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public int? FrecuenciaRefuerzoMeses { get; set; }

    public int? EdadPrimeraDosisSemanas { get; set; }

    public string? Laboratorio { get; set; }

    public string? EnfermedadesPrevine { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual ICollection<HistorialVacuna> HistorialVacunas { get; set; } = new List<HistorialVacuna>();
}
