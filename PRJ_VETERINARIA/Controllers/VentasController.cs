using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRJ_VETERINARIA.Models;
using PRJ_VETERINARIA.ViewModels.Ventas;

namespace PRJ_VETERINARIA.Controllers
{
    public class VentasController : Controller
    {
        private readonly BDVeterinariaContext _context;

        public VentasController(BDVeterinariaContext context)
        {
            _context = context;
        }

        // ──────────────────────────────────────────────
        // Métodos privados — SelectLists
        // ──────────────────────────────────────────────
        private async Task<IEnumerable<SelectListItem>> GetClientesSelectAsync(
            int? selectedId = null)
        {
            var clientes = await _context.Clientes
                .Where(c => c.Activo)
                .OrderBy(c => c.NombreCompleto)
                .ToListAsync();

            return clientes.Select(c => new SelectListItem
            {
                Value = c.IdCliente.ToString(),
                Text = $"{c.NombreCompleto} ({c.NumIdentificacion})",
                Selected = c.IdCliente == selectedId
            });
        }

        private async Task<IEnumerable<SelectListItem>> GetFormasPagoSelectAsync(
            int? selectedId = null)
        {
            var formasPago = await _context.FormaPagos
                .Where(f => f.Activo)
                .OrderBy(f => f.Nombre)
                .ToListAsync();

            return formasPago.Select(f => new SelectListItem
            {
                Value = f.IdFormaPago.ToString(),
                Text = f.Nombre,
                Selected = f.IdFormaPago == selectedId
            });
        }

        private async Task<IEnumerable<SelectListItem>> GetAtencionesSelectAsync(
            int? selectedId = null)
        {
            // Solo atenciones completadas sin venta asociada aún
            var atenciones = await _context.Atencions
                .Where(a => a.Estado == "Completada" && !a.Venta.Any())
                .Include(a => a.IdMascotaNavigation)
                .OrderByDescending(a => a.FechaHoraInicio)
                .ToListAsync();

            return atenciones.Select(a => new SelectListItem
            {
                Value = a.IdAtencion.ToString(),
                Text = $"#{a.IdAtencion} - {a.IdMascotaNavigation.Nombre} ({a.FechaHoraInicio:dd/MM/yyyy})",
                Selected = a.IdAtencion == selectedId
            });
        }

        private async Task<IEnumerable<SelectListItem>> GetProductosSelectAsync(
            int? selectedId = null)
        {
            var productos = await _context.Productos
                .Where(p => p.Activo)
                .OrderBy(p => p.Nombre)
                .ToListAsync();

            return productos.Select(p => new SelectListItem
            {
                Value = p.IdProducto.ToString(),
                Text = $"[{p.CodigoInterno}] {p.Nombre} - S/{p.PrecioVenta:F2}",
                Selected = p.IdProducto == selectedId
            });
        }

        private async Task LlenarListasViewModel(VentaViewModel vm)
        {
            vm.ClientesDisponibles = await GetClientesSelectAsync(vm.IdCliente);
            vm.FormasPagoDisponibles = await GetFormasPagoSelectAsync(vm.IdFormaPago);
            vm.AtencionesDisponibles = await GetAtencionesSelectAsync(vm.IdAtencion);
            vm.ProductosDisponibles = await GetProductosSelectAsync();
        }

        // ──────────────────────────────────────────────
        // INDEX
        // ──────────────────────────────────────────────
        public async Task<IActionResult> Index()
        {
            var ventas = await _context.Venta
                .Include(v => v.IdClienteNavigation)
                .Include(v => v.IdFormaPagoNavigation)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();

            return View(ventas);
        }

        // ──────────────────────────────────────────────
        // DETAILS
        // ──────────────────────────────────────────────
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var venta = await _context.Venta
                .Include(v => v.IdClienteNavigation)
                .Include(v => v.IdFormaPagoNavigation)
                .Include(v => v.DetalleVenta)
                    .ThenInclude(d => d.IdProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdVenta == id);

            if (venta == null) return NotFound();

            return View(venta);
        }

        // ──────────────────────────────────────────────
        // CREATE GET
        // ──────────────────────────────────────────────
        public async Task<IActionResult> Create()
        {
            var vm = new VentaViewModel();
            await LlenarListasViewModel(vm);
            return View(vm);
        }

        // ──────────────────────────────────────────────
        // CREATE POST
        // ──────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VentaViewModel vm)
        {
            if (vm.Detalles == null || !vm.Detalles.Any())
            {
                ModelState.AddModelError(string.Empty,
                    "Debe agregar al menos un producto al detalle.");
            }

            if (!ModelState.IsValid)
            {
                await LlenarListasViewModel(vm);
                return View(vm);
            }

            // Calcular totales
            var subtotal = vm.Detalles!.Sum(d =>
                d.Cantidad * d.PrecioUnitario * (1 - d.DescuentoPorcentaje / 100));
            var descuento = subtotal * (vm.Descuento / 100);
            var baseImponible = subtotal - descuento;
            var impuestos = baseImponible * 0.18m;
            var total = baseImponible + impuestos;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var venta = new Venta
                {
                    IdCliente = vm.IdCliente!.Value,
                    IdAtencion = vm.IdAtencion,
                    IdUsuario = 1, // TODO: usuario autenticado
                    IdFormaPago = vm.IdFormaPago!.Value,
                    NumeroComprobante = vm.NumeroComprobante!.Trim().ToUpper(),
                    Fecha = vm.Fecha,
                    Subtotal = subtotal,
                    Descuento = descuento,
                    Impuestos = impuestos,
                    Total = total,
                    EstadoPago = vm.EstadoPago!,
                    Observaciones = vm.Observaciones?.Trim(),
                    CreatedAt = DateTime.Now
                };

                _context.Add(venta);
                await _context.SaveChangesAsync();

                // Guardar detalle y descontar stock
                foreach (var item in vm.Detalles!)
                {
                    var detalle = new DetalleVenta
                    {
                        IdVenta = venta.IdVenta,
                        IdProducto = item.IdProducto!.Value,
                        Lote = item.Lote?.Trim().ToUpper(),
                        Cantidad = item.Cantidad,
                        PrecioUnitario = item.PrecioUnitario,
                        DescuentoPorcentaje = item.DescuentoPorcentaje,
                        FechaVencimiento = item.FechaVencimiento,
                        Observaciones = item.Observaciones?.Trim(),
                        CreatedAt = DateTime.Now
                    };
                    _context.Add(detalle);

                    // Descontar del lote correspondiente
                    if (!string.IsNullOrEmpty(item.Lote))
                    {
                        var lote = await _context.LoteProductos
                            .FirstOrDefaultAsync(l =>
                                l.IdProducto == item.IdProducto &&
                                l.NumeroLote == item.Lote.Trim().ToUpper());

                        if (lote != null)
                            lote.CantidadActual -= item.Cantidad;
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();

                if (ex.InnerException?.Message.Contains("UK_Venta_Comprobante") == true)
                {
                    ModelState.AddModelError(nameof(vm.NumeroComprobante),
                        "Ya existe una venta con ese número de comprobante.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Ocurrió un error al guardar. Intente nuevamente.");
                }

                await LlenarListasViewModel(vm);
                return View(vm);
            }
        }

        // ──────────────────────────────────────────────
        // DELETE GET
        // ──────────────────────────────────────────────
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var venta = await _context.Venta
                .Include(v => v.IdClienteNavigation)
                .Include(v => v.IdFormaPagoNavigation)
                .FirstOrDefaultAsync(m => m.IdVenta == id);

            if (venta == null) return NotFound();

            return View(venta);
        }

        // ──────────────────────────────────────────────
        // DELETE POST
        // ──────────────────────────────────────────────
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venta = await _context.Venta.FindAsync(id);
            if (venta == null) return NotFound();

            try
            {
                _context.Venta.Remove(venta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty,
                    "No se puede eliminar esta venta porque tiene detalles asociados.");
                return View(venta);
            }
        }
    }
}