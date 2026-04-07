using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Veterinaria.Web.ViewModels.Ventas
{
    public class VentaViewModel
    {
        [Required(ErrorMessage = "El cliente es obligatorio.")]
        [Display(Name = "Cliente")]
        public int? IdCliente { get; set; }

        // Opcional — la venta puede estar asociada a una atención
        [Display(Name = "Atención relacionada")]
        public int? IdAtencion { get; set; }

        [Required(ErrorMessage = "La forma de pago es obligatoria.")]
        [Display(Name = "Forma de pago")]
        public int? IdFormaPago { get; set; }

        [Required(ErrorMessage = "El número de comprobante es obligatorio.")]
        [StringLength(50)]
        [Display(Name = "Número de comprobante")]
        public string? NumeroComprobante { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; } = DateTime.Today;

        [Range(0, 100)]
        [Display(Name = "Descuento general (%)")]
        public decimal Descuento { get; set; }

        [Display(Name = "Estado de pago")]
        public string? EstadoPago { get; set; } = "Pendiente";

        [StringLength(500)]
        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        // Detalle
        public List<DetalleVentaViewModel> Detalles { get; set; }
            = new() { new DetalleVentaViewModel() };

        // Listas para dropdowns
        public IEnumerable<SelectListItem> ClientesDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> FormasPagoDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> AtencionesDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> ProductosDisponibles { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public static readonly List<SelectListItem> EstadosPago = new()
        {
            new SelectListItem { Value = "Pendiente", Text = "Pendiente" },
            new SelectListItem { Value = "Pagado",    Text = "Pagado" },
            new SelectListItem { Value = "Anulado",   Text = "Anulado" }
        };
    }
}