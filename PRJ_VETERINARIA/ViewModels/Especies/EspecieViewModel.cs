using System.ComponentModel.DataAnnotations;

namespace PRJ_VETERINARIA.ViewModels.Especies
{
    public class EspecieViewModel
    {
        public int IdEspecie { get; set; }
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres")]
        [Display(Name = "Especie")]
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime? FechaModificacion { get; set; }
    }
}
