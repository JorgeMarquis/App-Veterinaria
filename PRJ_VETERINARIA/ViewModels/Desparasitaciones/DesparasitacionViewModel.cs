using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PRJ_VETERINARIA.ViewModels.Desparasitaciones
{
    public class DesparasitacionViewModel
    {
        public int IdDesparasitacion { get; set; }

        [Required(ErrorMessage = "La mascota es obligatoria.")]
        [Display(Name = "Mascota")]
        public int? IdMascota { get; set; }

        [Required(ErrorMessage = "El producto es obligatorio.")]
        [Display(Name = "Producto")]
        public int? IdProducto { get; set; }

        [Required(ErrorMessage = "El veterinario es obligatorio.")]
        [Display(Name = "Veterinario")]
        public int? IdVeterinario { get; set; }

        [Required(ErrorMessage = "El tipo es obligatorio.")]
        [StringLength(20)]
        [Display(Name = "Tipo de desparasitación")]
        public string? TipoDesparasitacion { get; set; }

        [Required(ErrorMessage = "La fecha de aplicación es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de aplicación")]
        public DateTime FechaAplicacion { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        [Display(Name = "Fecha próxima")]
        public DateOnly? FechaProxima { get; set; }

        [Range(0.01, 999.99, ErrorMessage = "El peso debe ser mayor a 0.")]
        [Display(Name = "Peso en aplicación (kg)")]
        public decimal? PesoAplicacion { get; set; }

        [StringLength(100)]
        [Display(Name = "Dosis aplicada")]
        public string? DosisAplicada { get; set; }

        [StringLength(500)]
        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        // Listas para dropdowns
        public IEnumerable<SelectListItem> MascotasDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> ProductosDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> VeterinariosDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public static readonly List<SelectListItem> TiposDesparasitacion = new()
        {
            new SelectListItem { Value = "Interna",  Text = "Interna" },
            new SelectListItem { Value = "Externa",  Text = "Externa" },
            new SelectListItem { Value = "Mixta",    Text = "Mixta" }
        };
    }
}