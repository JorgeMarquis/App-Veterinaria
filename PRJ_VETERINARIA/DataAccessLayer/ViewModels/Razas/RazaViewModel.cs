
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PRJ_VETERINARIA.DataAccessLayer.ViewModels.Razas
{
    public class RazaViewModel
    {
        public int IdRaza { get; set; }

        [Required(ErrorMessage = "Debe seleecionar una especie")]
        [Display(Name = "Especie")]
        public int? IdEspecie { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        [Display(Name = "Nombre")]
        public string? Nombre { get; set; }

        [StringLength(20, ErrorMessage = "Máximo 20 caracteres.")]
        [Display(Name = "Dimensión")]
        public string? Dimension { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        public IEnumerable<SelectListItem> EspeciesDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();
    }
}
