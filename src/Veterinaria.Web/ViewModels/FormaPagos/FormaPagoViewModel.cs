using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Web.ViewModels.FormaPagos
{
    public class FormaPagoViewModel
    {
        public int IdFormaPago { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres.")]
        [Display(Name = "Nombre")]
        public string? Nombre { get; set; }

        [Range(0, 100, ErrorMessage = "La comisión debe estar entre 0% y 100%.")]
        [Display(Name = "Comisión (%)")]
        public decimal ComisionPorcentaje { get; set; }

        [Display(Name = "¿Requiere autorización?")]
        public bool RequiereAutorizacion { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;
    }
}
