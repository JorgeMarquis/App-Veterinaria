using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Web.ViewModels.Especies
{
    public class EspecieViewModel
    {
        public int IdEspecie { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres")]
        [Display(Name = "Especie")]
        public string Nombre { get; set; } = null!;

        [StringLength(200, ErrorMessage = "La descripción no puede superar 200 caracteres.")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;
        public DateTime? FechaModificacion { get; set; }
    }
}
