using System.ComponentModel.DataAnnotations;

namespace PRJ_VETERINARIA.ViewModels.CategoriaProductos
{
    public class CategoriaProductoViewModel
    {
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        [Display(Name = "Nombre")]
        public string? Nombre { get; set; }

        [StringLength(300, ErrorMessage = "Máximo 300 caracteres.")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Display(Name = "¿Es medicamento?")]
        public bool EsMedicamento { get; set; }

        [Display(Name = "¿Requiere receta?")]
        public bool RequiereReceta { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;
    }
}
