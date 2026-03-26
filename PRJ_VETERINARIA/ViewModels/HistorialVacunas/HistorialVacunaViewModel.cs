using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PRJ_VETERINARIA.ViewModels.HistorialVacunas
{
    public class HistorialVacunaViewModel
    {
        public int IdHistorial { get; set; }

        [Required(ErrorMessage = "La mascota es obligatoria.")]
        [Display(Name = "Mascota")]
        public int? IdMascota { get; set; }

        [Required(ErrorMessage = "La vacuna es obligatoria.")]
        [Display(Name = "Vacuna")]
        public int? IdVacuna { get; set; }

        [Required(ErrorMessage = "El lote es obligatorio.")]
        [Display(Name = "Lote")]
        public int? IdLote { get; set; }

        [Required(ErrorMessage = "El veterinario es obligatorio.")]
        [Display(Name = "Veterinario")]
        public int? IdVeterinario { get; set; }

        [Required(ErrorMessage = "La fecha de aplicación es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de aplicación")]
        public DateTime FechaAplicacion { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        [Display(Name = "Fecha próximo refuerzo")]
        public DateOnly? FechaProximoRefuerzo { get; set; }

        [Range(1, 10, ErrorMessage = "El número de dosis debe estar entre 1 y 10.")]
        [Display(Name = "Número de dosis")]
        public int DosisNumero { get; set; } = 1;

        [StringLength(500)]
        [Display(Name = "Reacciones adversas")]
        public string? ReaccionesAdversas { get; set; }

        [StringLength(500)]
        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        // Listas para dropdowns
        public IEnumerable<SelectListItem> MascotasDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> VacunasDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> LotesDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> VeterinariosDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();
    }
}