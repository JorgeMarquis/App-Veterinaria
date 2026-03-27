using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PRJ_VETERINARIA.DataAccessLayer.ViewModels.Atenciones
{
    public class DetalleAtencionViewModel
    {
        public int IdDetalle { get; set; }

        // Solo uno de los dos debe venir — producto O servicio
        [Display(Name = "Producto")]
        public int? IdProducto { get; set; }

        [Display(Name = "Servicio")]
        public int? IdServicio { get; set; }

        [Required(ErrorMessage = "El tipo de item es obligatorio.")]
        [Display(Name = "Tipo")]
        public string? TipoItem { get; set; }

        [Range(0.01, 9999.99, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        [Display(Name = "Cantidad")]
        public decimal Cantidad { get; set; } = 1;

        [Range(0.00, 99999.99, ErrorMessage = "El precio debe ser mayor a 0.")]
        [Display(Name = "Precio unitario")]
        public decimal PrecioUnitario { get; set; }

        [Range(0, 100, ErrorMessage = "El descuento debe estar entre 0 y 100.")]
        [Display(Name = "Descuento (%)")]
        public decimal DescuentoPorcentaje { get; set; }

        [StringLength(100)]
        [Display(Name = "Dosis")]
        public string? Dosis { get; set; }

        [StringLength(100)]
        [Display(Name = "Frecuencia")]
        public string? Frecuencia { get; set; }

        [Range(1, 365)]
        [Display(Name = "Duración (días)")]
        public int? DuracionDias { get; set; }

        [StringLength(500)]
        [Display(Name = "Instrucciones")]
        public string? Instrucciones { get; set; }

        [StringLength(500)]
        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        // Listas para los dropdowns de cada fila
        public IEnumerable<SelectListItem> ProductosDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> ServiciosDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public static readonly List<SelectListItem> TiposItem = new()
        {
            new SelectListItem { Value = "PRODUCTO", Text = "Producto" },
            new SelectListItem { Value = "SERVICIO", Text = "Servicio" }
        };
    }
}