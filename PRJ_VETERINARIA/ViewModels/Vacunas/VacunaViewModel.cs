using System.ComponentModel.DataAnnotations;

namespace PRJ_VETERINARIA.ViewModels.Vacunas
{
    public class VacunaViewModel
    {
        public int IdVacuna { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        [Display(Name = "Nombre")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El tipo es obligatorio.")]
        [StringLength(30, ErrorMessage = "Máximo 30 caracteres.")]
        [Display(Name = "Tipo")]
        public string? Tipo { get; set; }

        [Range(1, 36, ErrorMessage = "La frecuencia debe estar entre 1 y 36 meses.")]
        [Display(Name = "Frecuencia de refuerzo (meses)")]
        public int? FrecuenciaRefuerzoMeses { get; set; }

        [Range(1, 52, ErrorMessage = "La edad debe estar entre 1 y 52 semanas.")]
        [Display(Name = "Edad primera dosis (semanas)")]
        public int? EdadPrimeraDosisSemanas { get; set; }

        [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        [Display(Name = "Laboratorio")]
        public string? Laboratorio { get; set; }

        [StringLength(500, ErrorMessage = "Máximo 500 caracteres.")]
        [Display(Name = "Enfermedades que previene")]
        public string? EnfermedadesPrevine { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;
    }
}
