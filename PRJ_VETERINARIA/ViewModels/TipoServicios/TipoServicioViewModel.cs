using System.ComponentModel.DataAnnotations;

namespace PRJ_VETERINARIA.ViewModels.TipoServicios
{
    public class TipoServicioViewModel
    {
        public int IdTipoServicio { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        [Display(Name = "Nombre")]
        public string? Nombre { get; set; }

        [Display(Name = "¿Es médico?")]
        public bool EsMedico { get; set; }

        // int? porque es opcional — algunos servicios no tienen duración estimada
        [Range(1, 480, ErrorMessage = "La duración debe estar entre 1 y 480 minutos.")]
        [Display(Name = "Duración estimada (min)")]
        public int? DuracionEstimadaMin { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;
    }
}
