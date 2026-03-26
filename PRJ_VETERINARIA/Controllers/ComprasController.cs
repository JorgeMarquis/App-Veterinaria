using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRJ_VETERINARIA.Models;
using PRJ_VETERINARIA.ViewModels.Compras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRJ_VETERINARIA.Controllers
{
    public class ComprasController : Controller
    {
        private readonly BDVeterinariaContext _context;

        public ComprasController(BDVeterinariaContext context)
        {
            _context = context;
        }

        private async Task<IEnumerable<SelectListItem>> GetProveedoresSelectAsync(
            int? selectedId = null)
        {
            var proveedores = await _context.Proveedors
                .Where(p => p.Activo)
                .OrderBy(p => p.RazonSocial)
                .ToListAsync();

            return proveedores.Select(p => new SelectListItem
            {
                Value = p.IdProveedor.ToString(),
                Text = p.RazonSocial,
                Selected = p.IdProveedor == selectedId
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
                Text = $"[{p.CodigoInterno}] {p.Nombre}",
                Selected = p.IdProducto == selectedId
            });
        }

        private async Task LlenarListasViewModel(CompraViewModel compravm)
        {
            compravm.ProveedoresDisponibles = await GetProveedoresSelectAsync(compravm.IdProveedor);
            compravm.FormasPagoDisponibles = await GetFormasPagoSelectAsync(compravm.IdFormaPago);
            compravm.ProductosDisponibles = await GetProductosSelectAsync();
        }

        // GET: Compras
        public async Task<IActionResult> Index()
        {
            var compras = await _context.Compras
                .Include(c => c.IdProveedorNavigation)
                .Include(c => c.IdFormaPagoNavigation)
                .OrderByDescending(c => c.Fecha)
                .ToListAsync();

            return View(compras);
        }

        // GET: Compras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var compra = await _context.Compras
                .Include(c => c.IdProveedorNavigation)
                .Include(c => c.IdFormaPagoNavigation)
                .Include(c => c.DetalleCompras)
                    .ThenInclude(d => d.IdProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdCompra == id);

            if (compra == null) return NotFound();

            return View(compra);
        }

        // GET: Compras/Create
        public async Task<IActionResult> Create()
        {
            var v_compravm = new CompraViewModel();
            await LlenarListasViewModel(v_compravm);
            return View(v_compravm);
        }

        // POST: Compras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompraViewModel compravm)
        {
            if (compravm.Detalles == null || !compravm.Detalles.Any())
            {
                ModelState.AddModelError(string.Empty,
                    "Debe agregar al menos un producto al detalle.");
                await LlenarListasViewModel(compravm);
                return View(compravm);
            }

            for (int i = 0; i < compravm.Detalles.Count; i++)
            {
                var detalle = compravm.Detalles[i];

                if (detalle.FechaFabricacion.HasValue &&
                    detalle.FechaFabricacion.Value >= detalle.FechaVencimiento)
                {
                    ModelState.AddModelError(
                        $"Detalles[{i}].FechaFabricacion",
                        "La fecha de fabricación debe ser anterior a la de vencimiento.");
                }

                if (detalle.FechaVencimiento <= DateOnly.FromDateTime(DateTime.Today))
                {
                    ModelState.AddModelError(
                        $"Detalles[{i}].FechaVencimiento",
                        "La fecha de vencimiento no puede ser pasada.");
                }
            }
            

            if (!ModelState.IsValid)
            {
                await LlenarListasViewModel(compravm);
                return View(compravm);
            }

            // ── Calcular totales ───────────────────────
            var subtotal = compravm.Detalles!.Sum(d => d.Cantidad * d.PrecioUnitario);
            var impuestos = subtotal * 0.18m;  // IGV 18% Perú
            var total = subtotal + impuestos;

            // ── Guardar cabecera ───────────────────────
            var compra = new Compra
            {
                IdProveedor = compravm.IdProveedor!.Value,
                IdFormaPago = compravm.IdFormaPago!.Value,
                IdUsuario = 1, // TODO: reemplazar con usuario autenticado en Fase 3
                NumeroFactura = compravm.NumeroFactura!.Trim().ToUpper(),
                Fecha = compravm.Fecha,
                EstadoPago = compravm.EstadoPago!,
                Subtotal = subtotal,
                Impuestos = impuestos,
                Total = total,
                Observaciones = compravm.Observaciones?.Trim(),
                CreatedAt = DateTime.Now
            };

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Guardamos la cabecera primero para obtener el IdCompra
                _context.Add(compra);
                await _context.SaveChangesAsync();

                // ── Guardar detalle y crear lotes ──────
                foreach (var item in compravm.Detalles)
                {
                    var detalle = new DetalleCompra
                    {
                        IdCompra = compra.IdCompra,
                        IdProducto = item.IdProducto!.Value,
                        Lote = item.Lote!.Trim().ToUpper(),
                        FechaVencimiento = item.FechaVencimiento,
                        FechaFabricacion = item.FechaFabricacion,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = item.PrecioUnitario,
                        Observaciones = item.Observaciones?.Trim(),
                        CreatedAt = DateTime.Now
                    };
                    _context.Add(detalle);

                    // Crear o actualizar el lote de producto
                    var loteExistente = await _context.LoteProductos
                        .FirstOrDefaultAsync(l =>
                            l.IdProducto == item.IdProducto &&
                            l.NumeroLote == item.Lote!.Trim().ToUpper());

                    if (loteExistente != null)
                    {
                        // Si el lote ya existe, sumamos la cantidad
                        loteExistente.CantidadActual += item.Cantidad;
                    }
                    else
                    {
                        // Si no existe, creamos un lote nuevo
                        var lote = new LoteProducto
                        {
                            IdProducto = item.IdProducto!.Value,
                            NumeroLote = item.Lote!.Trim().ToUpper(),
                            FechaVencimiento = item.FechaVencimiento,
                            FechaFabricacion = item.FechaFabricacion,
                            CantidadInicial = item.Cantidad,
                            CantidadActual = item.Cantidad,
                            FechaIngreso = DateTime.Now,
                            Activo = true
                        };
                        _context.Add(lote);
                    }
                }

                await _context.SaveChangesAsync();

                // Si todo salió bien, confirmamos la transacción
                await transaction.CommitAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                // Si algo falló, revertimos todo — ni la compra ni los lotes quedan a medias
                await transaction.RollbackAsync();

                if (ex.InnerException?.Message.Contains("UK_Compra_Factura") == true)
                {
                    ModelState.AddModelError(nameof(compravm.NumeroFactura),
                        "Ya existe una compra con ese número de factura para este proveedor.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Ocurrió un error al guardar. Intente nuevamente.");
                }

                await LlenarListasViewModel(compravm);
                return View(compravm);
            }
        }

        // GET: Compras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var compra = await _context.Compras
                .Include(c => c.IdProveedorNavigation)
                .Include(c => c.IdFormaPagoNavigation)
                .FirstOrDefaultAsync(m => m.IdCompra == id);

            if (compra == null) return NotFound();

            return View(compra);
        }

        // POST: Compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var compra = await _context.Compras.FindAsync(id);
            if (compra == null) return NotFound();

            try
            {
                _context.Compras.Remove(compra);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty,
                    "No se puede eliminar esta compra porque tiene detalles asociados.");
                return View(compra);
            }
        }
    }
}
