using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PRJ_VETERINARIA.ViewModels.Desparasitaciones
{
    public class DesparasitacionViewModel
    {
        public int IdDesparasitacion { get; set; }

        [Required(ErrorMessage = "La mascota es obligatoria.")]
        [Display(Name = "Mascota")]
        public int? IdMascota { get; set; }

        [Display(Name = "Mascota (Lectura)")]
        public string NombreMascota { get; set; } = string.Empty;

        [Required(ErrorMessage = "El veterinario es obligatorio.")]
        [Display(Name = "Veterinario")]
        public int? IdVeterinario { get; set; }

        [Display(Name = "Veterinario (Lectura)")]
        public string NombreVeterinario { get; set; } = string.Empty;

        [Required(ErrorMessage = "El producto es obligatorio.")]
        [Display(Name = "Producto")]
        public int? IdProducto { get; set; }

        [Display(Name = "Producto (Lectura)")]
        public string NombreProducto { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "La dosis es obligatoria.")]
        [Display(Name = "Dosis")]
        [StringLength(100)]
        public string? Dosis { get; set; }

        [Display(Name = "Vía de Administración")]
        [StringLength(50)]
        public string? ViaAdministracion { get; set; }

        [Display(Name = "Próxima Desparasitación")]
        [DataType(DataType.Date)]
        public DateTime? ProximaFecha { get; set; }

        [Display(Name = "Observaciones")]
        [StringLength(500)]
        public string? Observaciones { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        // Listas para dropdowns
        public IEnumerable<SelectListItem> MascotasDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> VeterinariasDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> ProductosDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();
    }
}