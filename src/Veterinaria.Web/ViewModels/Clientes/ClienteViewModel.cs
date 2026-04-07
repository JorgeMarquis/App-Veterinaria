using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Web.ViewModels.Clientes
{
    public class ClienteViewModel
    {
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "El tipo de identificación es obligatorio.")]
        [StringLength(30)]
        [Display(Name = "Tipo de identificación")]
        public string? TipoIdentificacion { get; set; }

        [Required(ErrorMessage = "El número de identificación es obligatorio.")]
        [StringLength(20, ErrorMessage = "Máximo 20 caracteres.")]
        [Display(Name = "Número de identificación")]
        public string? NumIdentificacion { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(150, ErrorMessage = "Máximo 150 caracteres.")]
        [Display(Name = "Nombre completo")]
        public string? NombreCompleto { get; set; }

        [Required(ErrorMessage = "El teléfono principal es obligatorio.")]
        [StringLength(20, ErrorMessage = "Máximo 20 caracteres.")]
        [Display(Name = "Teléfono principal")]
        public string? TelefonoPrincipal { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(300, ErrorMessage = "Máximo 300 caracteres.")]
        [Display(Name = "Dirección")]
        public string? Direccion { get; set; }

        [Required(ErrorMessage = "La ciudad es obligatoria.")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        [Display(Name = "Ciudad")]
        public string? Ciudad { get; set; }

        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        [StringLength(150, ErrorMessage = "Máximo 150 caracteres.")]
        [Display(Name = "Correo electrónico")]
        public string? Email { get; set; }

        [StringLength(20, ErrorMessage = "Máximo 20 caracteres.")]
        [Display(Name = "Teléfono alternativo")]
        public string? TelefonoAlternativo { get; set; }

        [StringLength(150, ErrorMessage = "Máximo 150 caracteres.")]
        [Display(Name = "Contacto de emergencia")]
        public string? ContactoEmergencia { get; set; }

        [StringLength(20, ErrorMessage = "Máximo 20 caracteres.")]
        [Display(Name = "Teléfono de emergencia")]
        public string? TelefonoEmergencia { get; set; }

        [StringLength(500, ErrorMessage = "Máximo 500 caracteres.")]
        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        // Lista estática de tipos de identificación para Perú
        public static readonly List<string> TiposIdentificacion = new()
        {
            "DNI",
            "RUC",
            "Carnet de Extranjería",
            "Pasaporte"
        };

        // Auditoría
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
