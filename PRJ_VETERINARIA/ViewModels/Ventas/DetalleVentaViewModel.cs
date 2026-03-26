using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PRJ_VETERINARIA.ViewModels.Ventas
{
    public class DetalleVentaViewModel
    {
        [Required(ErrorMessage = "El producto es obligatorio.")]
        [Display(Name = "Producto")]
        public int? IdProducto { get; set; }

        [StringLength(50)]
        [Display(Name = "Lote")]
        public string? Lote { get; set; }

        [Range(0.01, 9999.99, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        [Display(Name = "Cantidad")]
        public decimal Cantidad { get; set; } = 1;

        [Range(0.01, 99999.99, ErrorMessage = "El precio debe ser mayor a 0.")]
        [Display(Name = "Precio unitario")]
        public decimal PrecioUnitario { get; set; }

        [Range(0, 100, ErrorMessage = "El descuento debe estar entre 0 y 100.")]
        [Display(Name = "Descuento (%)")]
        public decimal DescuentoPorcentaje { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha vencimiento")]
        public DateOnly? FechaVencimiento { get; set; }

        [StringLength(200)]
        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        public IEnumerable<SelectListItem> ProductosDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();
    }
}