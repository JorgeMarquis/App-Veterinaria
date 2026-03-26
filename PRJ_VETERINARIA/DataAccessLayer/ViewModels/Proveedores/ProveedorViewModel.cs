using System.ComponentModel.DataAnnotations;

namespace PRJ_VETERINARIA.DataAccessLayer.ViewModels.Proveedores
{
    public class ProveedorViewModel
    {
        public int IdProveedor { get; set; }

        [Required(ErrorMessage = "El tipo de identificación es obligatorio.")]
        [StringLength(30)]
        [Display(Name = "Tipo de identificación")]
        public string? TipoIdentificacion { get; set; }

        [Required(ErrorMessage = "El número de identificación es obligatorio.")]
        [StringLength(20, ErrorMessage = "Máximo 20 caracteres.")]
        [Display(Name = "Número de identificación")]
        public string? NumIdentificacion { get; set; }

        [Required(ErrorMessage = "La razón social es obligatoria.")]
        [StringLength(150, ErrorMessage = "Máximo 150 caracteres.")]
        [Display(Name = "Razón social")]
        public string? RazonSocial { get; set; }

        [StringLength(150, ErrorMessage = "Máximo 150 caracteres.")]
        [Display(Name = "Nombre de contacto")]
        public string? NombreContacto { get; set; }

        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        [StringLength(150, ErrorMessage = "Máximo 150 caracteres.")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [StringLength(20, ErrorMessage = "Máximo 20 caracteres.")]
        [Display(Name = "Teléfono principal")]
        public string? TelefonoPrincipal { get; set; }

        [StringLength(300, ErrorMessage = "Máximo 300 caracteres.")]
        [Display(Name = "Dirección")]
        public string? Direccion { get; set; }

        [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        [Display(Name = "Distrito")]
        public string? Distrito { get; set; }

        // Rubro al que se dedica el proveedor — ej: "Farmacéutico", "Alimentos"
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        [Display(Name = "Giro comercial")]
        public string? GiroComercial { get; set; }

        [StringLength(500, ErrorMessage = "Máximo 500 caracteres.")]
        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;
    }
}
