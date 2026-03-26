using System.ComponentModel.DataAnnotations;

namespace PRJ_VETERINARIA.ViewModels.Kardex
{
    public class KardexViewModel
    {
        public int IdProducto { get; set; }
        
        [Display(Name = "Producto")]
        public string NombreProducto { get; set; } = string.Empty;

        [Display(Name = "Código")]
        public string CodigoInterno { get; set; } = string.Empty;

        [Display(Name = "Stock Actual")]
        public int StockActual { get; set; }

        [Display(Name = "Precio Unitario")]
        [DataType(DataType.Currency)]
        public decimal PrecioUnitario { get; set; }

        [Display(Name = "Valor Total")]
        [DataType(DataType.Currency)]
        public decimal ValorTotal { get; set; }

        // Movimientos del producto
        public List<KardexMovimientoViewModel> Movimientos { get; set; } = new();
    }
}