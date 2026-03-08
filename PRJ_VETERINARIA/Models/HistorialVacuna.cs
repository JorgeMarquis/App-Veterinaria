using System;
using System.Collections.Generic;

namespace PRJ_VETERINARIA.Models;

public partial class HistorialVacuna
{
    public int IdHistorial { get; set; }

    public int IdMascota { get; set; }

    public int IdVacuna { get; set; }

    public DateTime FechaAplicacion { get; set; }

    public DateOnly? FechaProximoRefuerzo { get; set; }

    public int IdVeterinario { get; set; }

    public int IdLote { get; set; }

    public int DosisNumero { get; set; }

    public string? ReaccionesAdversas { get; set; }

    public string? Observaciones { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual LoteProducto IdLoteNavigation { get; set; } = null!;

    public virtual Mascota IdMascotaNavigation { get; set; } = null!;

    public virtual Vacuna IdVacunaNavigation { get; set; } = null!;

    public virtual Usuario IdVeterinarioNavigation { get; set; } = null!;
}
