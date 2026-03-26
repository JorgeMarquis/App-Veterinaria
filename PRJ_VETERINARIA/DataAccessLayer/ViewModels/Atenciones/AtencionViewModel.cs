using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PRJ_VETERINARIA.DataAccessLayer.ViewModels.Atenciones
{
    public class AtencionViewModel
    {
        public int IdAtencion { get; set; }

        [Required(ErrorMessage = "La mascota es obligatoria.")]
        [Display(Name = "Mascota")]
        public int? IdMascota { get; set; }

        [Required(ErrorMessage = "El veterinario es obligatorio.")]
        [Display(Name = "Veterinario")]
        public int? IdVeterinario { get; set; }

        [Required(ErrorMessage = "El tipo de atención es obligatorio.")]
        [StringLength(50)]
        [Display(Name = "Tipo de atención")]
        public string? TipoAtencion { get; set; }

        [Required(ErrorMessage = "El motivo de consulta es obligatorio.")]
        [StringLength(1000)]
        [Display(Name = "Motivo de consulta")]
        public string? MotivoConsulta { get; set; }

        [StringLength(2000)]
        [Display(Name = "Síntomas")]
        public string? Sintomas { get; set; }

        [StringLength(2000)]
        [Display(Name = "Examen físico")]
        public string? ExamenFisico { get; set; }

        [StringLength(2000)]
        [Display(Name = "Diagnóstico")]
        public string? Diagnostico { get; set; }

        [StringLength(2000)]
        [Display(Name = "Tratamiento")]
        public string? Tratamiento { get; set; }

        [StringLength(2000)]
        [Display(Name = "Recomendaciones")]
        public string? Recomendaciones { get; set; }

        [Range(0.01, 999.99)]
        [Display(Name = "Peso (kg)")]
        public decimal? PesoAtencion { get; set; }

        [Range(35.0, 43.0)]
        [Display(Name = "Temperatura (°C)")]
        public decimal? Temperatura { get; set; }

        [Range(40, 250)]
        [Display(Name = "Frecuencia cardíaca")]
        public int? FrecuenciaCardiaca { get; set; }

        [Range(10, 100)]
        [Display(Name = "Frecuencia respiratoria")]
        public int? FrecuenciaRespiratoria { get; set; }

        [Display(Name = "Estado")]
        public string? Estado { get; set; } = "Programada";

        [StringLength(500)]
        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        // Detalle de la atención
        public List<DetalleAtencionViewModel> Detalles { get; set; }
            = new() { new DetalleAtencionViewModel() };

        // Listas para dropdowns
        public IEnumerable<SelectListItem> MascotasDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> VeterinariosDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> ProductosDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> ServiciosDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public static readonly List<SelectListItem> TiposAtencion = new()
        {
            new SelectListItem { Value = "Consulta",      Text = "Consulta" },
            new SelectListItem { Value = "Emergencia",    Text = "Emergencia" },
            new SelectListItem { Value = "Cirugia",       Text = "Cirugía" },
            new SelectListItem { Value = "Control",       Text = "Control" },
            new SelectListItem { Value = "Vacunacion",    Text = "Vacunación" },
            new SelectListItem { Value = "Desparasitacion", Text = "Desparasitación" }
        };

        public static readonly List<SelectListItem> EstadosAtencion = new()
        {
            new SelectListItem { Value = "Programada",  Text = "Programada" },
            new SelectListItem { Value = "En curso",    Text = "En curso" },
            new SelectListItem { Value = "Completada",  Text = "Completada" },
            new SelectListItem { Value = "Cancelada",   Text = "Cancelada" }
        };
    }
}