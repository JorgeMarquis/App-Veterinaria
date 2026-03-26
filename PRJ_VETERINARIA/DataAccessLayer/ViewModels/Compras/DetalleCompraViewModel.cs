using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PRJ_VETERINARIA.DataAccessLayer.ViewModels.Compras
{
    public class DetalleCompraViewModel
    {
        [Required(ErrorMessage = "El producto es obligatorio.")]
        [Display(Name = "Producto")]
        public int? IdProducto { get; set; }

        [Required(ErrorMessage = "El número de lote es obligatorio.")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres.")]
        [Display(Name = "Número de lote")]
        public string? Lote { get; set; }

        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de vencimiento")]
        public DateOnly FechaVencimiento { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de fabricación")]
        public DateOnly? FechaFabricacion { get; set; }

        [Range(0.01, 99999.99, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        [Display(Name = "Cantidad")]
        public decimal Cantidad { get; set; }

        [Range(0.01, 99999.99, ErrorMessage = "El precio debe ser mayor a 0.")]
        [Display(Name = "Precio unitario")]
        public decimal PrecioUnitario { get; set; }

        [StringLength(200)]
        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        // Calculado en JavaScript — no viene del usuario
        public decimal Subtotal => Cantidad * PrecioUnitario;

        // Lista de productos para el dropdown de cada fila
        public IEnumerable<SelectListItem> ProductosDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();
    }
}
