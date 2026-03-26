using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PRJ_VETERINARIA.DataAccessLayer.ViewModels.Servicios
{
    public class ServicioViewModel
    {
        public int IdServicio { get; set; }

        [Required(ErrorMessage = "El tipo de servicio es obligatorio.")]
        [Display(Name = "Tipo de servicio")]
        public int? IdTipoServicio { get; set; }

        [Required(ErrorMessage = "El código de servicio es obligatorio.")]
        [StringLength(20, ErrorMessage = "Máximo 20 caracteres.")]
        [Display(Name = "Código de servicio")]
        public string? CodigoServicio { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(200, ErrorMessage = "Máximo 200 caracteres.")]
        [Display(Name = "Nombre")]
        public string? Nombre { get; set; }

        [StringLength(1000, ErrorMessage = "Máximo 1000 caracteres.")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Range(0.01, 99999.99, ErrorMessage = "El precio base debe ser mayor a 0.")]
        [Display(Name = "Precio base")]
        public decimal PrecioBase { get; set; }

        [Display(Name = "¿Requiere ayuno?")]
        public bool RequiereAyuno { get; set; }

        [Display(Name = "¿Requiere preparación?")]
        public bool RequierePreparacion { get; set; }

        [StringLength(500, ErrorMessage = "Máximo 500 caracteres.")]
        [Display(Name = "Instrucciones de preparación")]
        public string? InstruccionesPreparacion { get; set; }

        [StringLength(500, ErrorMessage = "Máximo 500 caracteres.")]
        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        public IEnumerable<SelectListItem> TiposServicioDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();
    }
}
