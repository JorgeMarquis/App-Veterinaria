using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Web.ViewModels.Productos
{
    public class ProductoViewModel
    {
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [Display(Name = "Categoría")]
        public int? IdCategoria { get; set; }

        [StringLength(50, ErrorMessage = "Máximo 50 caracteres.")]
        [Display(Name = "Código de barras")]
        public string? CodigoBarras { get; set; }

        [Required(ErrorMessage = "El código interno es obligatorio.")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres.")]
        [Display(Name = "Código interno")]
        public string? CodigoInterno { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(200, ErrorMessage = "Máximo 200 caracteres.")]
        [Display(Name = "Nombre")]
        public string? Nombre { get; set; }

        [StringLength(1000, ErrorMessage = "Máximo 1000 caracteres.")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        // Tipo de producto — lista fija: Medicamento, Alimento, Accesorio, etc.
        [Required(ErrorMessage = "El tipo de producto es obligatorio.")]
        [StringLength(20)]
        [Display(Name = "Tipo de producto")]
        public string? TipoProducto { get; set; }

        [Range(0.00, 99999.99, ErrorMessage = "El precio de venta debe ser mayor a 0.")]
        [Display(Name = "Precio de venta")]
        public decimal PrecioVenta { get; set; }

        // Precio de costo — puede ser 0 en casos especiales
        [Range(0, 99999.99, ErrorMessage = "El precio de costo no puede ser negativo.")]
        [Display(Name = "Precio de costo")]
        public decimal PrecioCosto { get; set; }

        // Unidad de medida — lista fija
        [Required(ErrorMessage = "La unidad de medida es obligatoria.")]
        [StringLength(20)]
        [Display(Name = "Unidad de medida")]
        public string? UnidadMedida { get; set; } = "UNIDAD";

        [Display(Name = "¿Requiere receta?")]
        public bool RequiereReceta { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        // Auditoría
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        public IEnumerable<SelectListItem> CategoriasDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public static readonly List<SelectListItem> TiposProducto = new()
        {
            new SelectListItem { Value = "Medicamento",  Text = "Medicamento" },
            new SelectListItem { Value = "Alimento",     Text = "Alimento" },
            new SelectListItem { Value = "Accesorio",    Text = "Accesorio" },
            new SelectListItem { Value = "Higiene",      Text = "Higiene" },
            new SelectListItem { Value = "Suplemento",   Text = "Suplemento" },
            new SelectListItem { Value = "Otro",         Text = "Otro" }
        };

        public static readonly List<SelectListItem> UnidadesMedida = new()
        {
            new SelectListItem { Value = "UNIDAD",   Text = "Unidad" },
            new SelectListItem { Value = "CAJA",     Text = "Caja" },
            new SelectListItem { Value = "FRASCO",   Text = "Frasco" },
            new SelectListItem { Value = "AMPOLLA",  Text = "Ampolla" },
            new SelectListItem { Value = "TABLETA",  Text = "Tableta" },
            new SelectListItem { Value = "ML",       Text = "Mililitro (ml)" },
            new SelectListItem { Value = "KG",       Text = "Kilogramo (kg)" }
        };
    }
}
