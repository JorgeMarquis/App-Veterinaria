using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Web.ViewModels.Mascotas
{
    public class MascotaViewModel
    {
        public int IdMascota { get; set; }

        [Required(ErrorMessage = "El cliente es obligatorio.")]
        [Display(Name = "Cliente")]
        public int? IdCliente { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        [Display(Name = "Nombre")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "La especie es obligatoria.")]
        [Display(Name = "Especie")]
        public int? IdEspecie { get; set; }

        // no todas las mascotas tienen raza definida
        [Display(Name = "Raza")]
        public int? IdRaza { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de nacimiento")]
        public DateOnly? FechaNacimiento { get; set; }

        // M o H — Macho o Hembra
        [Required(ErrorMessage = "El sexo es obligatorio.")]
        [StringLength(1)]
        [Display(Name = "Sexo")]
        public string? Sexo { get; set; }

        [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        [Display(Name = "Color de pelaje")]
        public string? ColorPelaje { get; set; }

        // Único por mascota — constraint en BD
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres.")]
        [Display(Name = "Microchip ID")]
        public string? MicrochipId { get; set; }

        [StringLength(500)]
        [Display(Name = "Foto URL")]
        public string? FotoUrl { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de fallecimiento")]
        public DateOnly? FechaFallecimiento { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        
        public IEnumerable<SelectListItem> ClientesDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> EspeciesDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();


        // Todas las razas — JavaScript las filtra por IdEspecie
        public IEnumerable<SelectListItem> RazasDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();


        // Opciones fijas de sexo
        public static readonly List<SelectListItem> OpcionesSexo = new()
        {
            new SelectListItem { Value = "M", Text = "Macho" },
            new SelectListItem { Value = "F", Text = "Hembra" }
        };

        // Propiedades de solo lectura para el Index
        [Display(Name = "Cliente")]
        public string? ClienteNombre { get; set; }
        [Display(Name = "Especie")]
        public string? EspecieNombre { get; set; }
        [Display(Name = "Raza")]
        public string? RazaNombre { get; set; }
    }
}
