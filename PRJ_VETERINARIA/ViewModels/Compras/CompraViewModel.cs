using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PRJ_VETERINARIA.ViewModels.Compras
{
    public class CompraViewModel
    {
        [Required(ErrorMessage = "El proveedor es obligatorio.")]
        [Display(Name = "Proveedor")]
        public int? IdProveedor { get; set; }

        [Required(ErrorMessage = "La forma de pago es obligatoria.")]
        [Display(Name = "Forma de pago")]
        public int? IdFormaPago { get; set; }

        [Required(ErrorMessage = "El número de factura es obligatorio.")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres.")]
        [Display(Name = "Número de factura")]
        public string? NumeroFactura { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "El estado de pago es obligatorio.")]
        [Display(Name = "Estado de pago")]
        public string? EstadoPago { get; set; } = "Pendiente";

        [StringLength(500)]
        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        // Lista de filas del detalle — mínimo una línea
        public List<DetalleCompraViewModel> Detalles { get; set; }
            = new() { new DetalleCompraViewModel() };

        // ── Listas para dropdowns ──────────────────────
        public IEnumerable<SelectListItem> ProveedoresDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> FormasPagoDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> ProductosDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public static readonly List<SelectListItem> EstadosPago = new()
        {
            new SelectListItem { Value = "Pendiente",  Text = "Pendiente" },
            new SelectListItem { Value = "Pagado",     Text = "Pagado" },
            new SelectListItem { Value = "Anulado",    Text = "Anulado" }
        };
    }
}
