using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PRJ_VETERINARIA.Models;

public partial class Cliente
{
    [Key]
    [DisplayName("ID Cliente")]
    public int IdCliente { get; set; }

    
    [DisplayName("Nombre Completo")]
    public string NombreCompleto { get; set; } = null!;

    
    [DisplayName("Tipo de Identificación")]
    public string TipoIdentificacion { get; set; } = null!;

    
    [DisplayName("Número de Identificación")]
    public string NumIdentificacion { get; set; } = null!;

    
    [DisplayName("Correo Electrónico")]
    public string? Email { get; set; }

    
    [DisplayName("Teléfono Principal")]
    public string TelefonoPrincipal { get; set; } = null!;

    [DisplayName("Teléfono Alternativo")]
    public string? TelefonoAlternativo { get; set; }

    
    [DisplayName("Dirección")]
    public string Direccion { get; set; } = null!;

    
    public string Ciudad { get; set; } = null!;

    [DisplayName("Contacto de Emergencia")]
    public string? ContactoEmergencia { get; set; }

    [DisplayName("Teléfono de Emergencia")]
    public string? TelefonoEmergencia { get; set; }

    public string? Observaciones { get; set; }

    public bool Activo { get; set; }
    
    [DisplayName("Fecha de Creación")]
    public DateTime CreatedAt { get; set; }

    [DisplayName("Creado Por")]
    public string? CreatedBy { get; set; }

    [DisplayName("Fecha de Actualizacion")]
    public DateTime? UpdatedAt { get; set; }

    [DisplayName("Actualizado Por")]
    public string? UpdatedBy { get; set; }

    public virtual ICollection<Mascota> Mascota { get; set; } = new List<Mascota>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
