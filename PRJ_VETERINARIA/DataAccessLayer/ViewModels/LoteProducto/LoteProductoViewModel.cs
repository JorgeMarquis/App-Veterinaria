using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PRJ_VETERINARIA.DataAccessLayer.ViewModels.LoteProducto
{
    public class LoteProductoViewModel
    {
        public int IdLote { get; set; }

        [Required(ErrorMessage = "El producto es obligatorio.")]
        [Display(Name = "Producto")]
        public int? IdProducto { get; set; }

        [Required(ErrorMessage = "El número de lote es obligatorio.")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres.")]
        [Display(Name = "Número de lote")]
        public string? NumeroLote { get; set; }

        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de vencimiento")]
        public DateOnly FechaVencimiento { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de fabricación")]
        public DateOnly? FechaFabricacion { get; set; }

        [Range(0.01, 99999.99, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        [Display(Name = "Cantidad inicial")]
        public decimal CantidadInicial { get; set; }

        [StringLength(200, ErrorMessage = "Máximo 200 caracteres.")]
        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        public IEnumerable<SelectListItem> ProductosDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();
    }
}
