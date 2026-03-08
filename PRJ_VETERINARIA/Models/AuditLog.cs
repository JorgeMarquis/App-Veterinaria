using System;
using System.Collections.Generic;

namespace PRJ_VETERINARIA.Models;

public partial class AuditLog
{
    public long IdAuditLog { get; set; }

    public string Tabla { get; set; } = null!;

    public int IdRegistro { get; set; }

    public string Accion { get; set; } = null!;

    public string? ValoresAnteriores { get; set; }

    public string? ValoresNuevos { get; set; }

    public int? IdUsuario { get; set; }

    public string? NombreUsuario { get; set; }

    public string? IpOrigen { get; set; }

    public DateTime Fecha { get; set; }
}
