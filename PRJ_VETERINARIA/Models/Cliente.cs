using System;
using System.Collections.Generic;

namespace PRJ_VETERINARIA.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string TipoIdentificacion { get; set; } = null!;

    public string NumIdentificacion { get; set; } = null!;

    public string? Email { get; set; }

    public string TelefonoPrincipal { get; set; } = null!;

    public string? TelefonoAlternativo { get; set; }

    public string Direccion { get; set; } = null!;

    public string Ciudad { get; set; } = null!;

    public string? ContactoEmergencia { get; set; }

    public string? TelefonoEmergencia { get; set; }

    public string? Observaciones { get; set; }

    public bool Activo { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual ICollection<Mascota> Mascota { get; set; } = new List<Mascota>();

    public virtual ICollection<Ventum> Venta { get; set; } = new List<Ventum>();
}
