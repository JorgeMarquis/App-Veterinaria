using System.ComponentModel.DataAnnotations;

namespace PRJ_VETERINARIA.ViewModels.Kardex
{
    public class KardexMovimientoViewModel
    {
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }

        [Display(Name = "Tipo")]
        public string TipoMovimiento { get; set; } = string.Empty;  // Entrada, Salida, Ajuste

        [Display(Name = "Referencia")]
        public string Referencia { get; set; } = string.Empty;  // N° Compra, N° Venta, etc.

        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }

        [Display(Name = "Precio Unit.")]
        [DataType(DataType.Currency)]
        public decimal PrecioUnitario { get; set; }

        [Display(Name = "Subtotal")]
        [DataType(DataType.Currency)]
        public decimal Subtotal { get; set; }

        [Display(Name = "Stock")]
        public int StockResultante { get; set; }

        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }
    }
}
