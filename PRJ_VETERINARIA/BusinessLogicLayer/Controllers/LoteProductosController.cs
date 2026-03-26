using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRJ_VETERINARIA.DataAccessLayer.Models;
using PRJ_VETERINARIA.DataAccessLayer.ViewModels.LoteProducto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRJ_VETERINARIA.BusinessLogicLayer.Controllers
{
    public class LoteProductosController : Controller
    {
        private readonly BDVeterinariaContext _context;

        public LoteProductosController(BDVeterinariaContext context)
        {
            _context = context;
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
                // Mostramos código interno + nombre para identificar mejor
                Text = $"[{p.CodigoInterno}] {p.Nombre}",
                Selected = p.IdProducto == selectedId
            });
        }

        private async Task LlenarListasViewModel(LoteProductoViewModel loteproductovm)
        {
            loteproductovm.ProductosDisponibles = await GetProductosSelectAsync(loteproductovm.IdProducto);
        }

        // GET: LoteProductos
        public async Task<IActionResult> Index()
        {
            var lotes = await _context.LoteProductos
                .Include(l => l.IdProductoNavigation)
                .Where(l => l.Activo)
                .OrderBy(l => l.FechaVencimiento)
                .ToListAsync();

            return View(lotes);
        }

        // GET: LoteProductos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var lote = await _context.LoteProductos
                .Include(l => l.IdProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdLote == id);

            if (lote == null) return NotFound();

            return View(lote);
        }

        // GET: LoteProductos/Create
        public async Task<IActionResult> Create()
        {
            var v_loteproductos = new LoteProductoViewModel();
            await LlenarListasViewModel(v_loteproductos);
            return View(v_loteproductos);
        }

        // POST: LoteProductos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LoteProductoViewModel loteproductovm)
        {
            if (loteproductovm.FechaFabricacion.HasValue &&
                loteproductovm.FechaFabricacion.Value >= loteproductovm.FechaVencimiento)
            {
                ModelState.AddModelError(nameof(loteproductovm.FechaFabricacion),
                    "La fecha de fabricación debe ser anterior a la fecha de vencimiento.");
            }

            // Validación de negocio: el lote no debería ingresar vencido
            if (loteproductovm.FechaVencimiento <= DateOnly.FromDateTime(DateTime.Today))
            {
                ModelState.AddModelError(nameof(loteproductovm.FechaVencimiento),
                    "No se puede registrar un lote con fecha de vencimiento pasada.");
            }

            if (!ModelState.IsValid)
            {
                await LlenarListasViewModel(loteproductovm);
                return View(loteproductovm);
            }

            var lote = new LoteProducto
            {
                IdProducto = loteproductovm.IdProducto!.Value,
                NumeroLote = loteproductovm.NumeroLote!.Trim().ToUpper(),
                FechaVencimiento = loteproductovm.FechaVencimiento,
                FechaFabricacion = loteproductovm.FechaFabricacion,
                CantidadInicial = loteproductovm.CantidadInicial,
                // CantidadActual parte igual a CantidadInicial — el Controller lo asigna
                CantidadActual = loteproductovm.CantidadInicial,
                // FechaIngreso la asigna el Controller — no el usuario
                FechaIngreso = DateTime.Now,
                Activo = true,
                Observaciones = loteproductovm.Observaciones?.Trim()
            };

            try
            {
                _context.Add(lote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                // Constraint compuesta — error en string.Empty
                if (ex.InnerException?.Message.Contains("UK_LoteProducto_ProdLote") == true)
                {
                    ModelState.AddModelError(string.Empty,
                        "Ya existe un lote con ese número para este producto.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Ocurrió un error al guardar. Intente nuevamente.");
                }

                await LlenarListasViewModel(loteproductovm);
                return View(loteproductovm);
            }
        }

        // GET: LoteProductos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var lote = await _context.LoteProductos.FindAsync(id);
            if (lote == null) return NotFound();

            var v_loteproductovm = new LoteProductoViewModel
            {
                IdLote = lote.IdLote,
                IdProducto = lote.IdProducto,
                NumeroLote = lote.NumeroLote,
                FechaVencimiento = lote.FechaVencimiento,
                FechaFabricacion = lote.FechaFabricacion,
                CantidadInicial = lote.CantidadInicial,
                Observaciones = lote.Observaciones,
                Activo = lote.Activo
            };

            await LlenarListasViewModel(v_loteproductovm);
            return View(v_loteproductovm);
        }

        // POST: LoteProductos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LoteProductoViewModel loteproductovm)
        {
            if (loteproductovm.FechaFabricacion.HasValue &&
                loteproductovm.FechaFabricacion.Value >= loteproductovm.FechaVencimiento)
            {
                ModelState.AddModelError(nameof(loteproductovm.FechaFabricacion),
                    "La fecha de fabricación debe ser anterior a la fecha de vencimiento.");
            }

            if (!ModelState.IsValid)
            {
                await LlenarListasViewModel(loteproductovm);
                return View(loteproductovm);
            }

            var lote = await _context.LoteProductos.FindAsync(loteproductovm.IdLote);
            if (lote == null) return NotFound();

            lote.IdProducto = loteproductovm.IdProducto!.Value;
            lote.NumeroLote = loteproductovm.NumeroLote!.Trim().ToUpper();
            lote.FechaVencimiento = loteproductovm.FechaVencimiento;
            lote.FechaFabricacion = loteproductovm.FechaFabricacion;
            lote.Observaciones = loteproductovm.Observaciones?.Trim();
            lote.Activo = loteproductovm.Activo;

            try
            {
                _context.Update(lote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await LoteProductoExists(loteproductovm.IdLote)) return NotFound();
                else throw;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UK_LoteProducto_ProdLote") == true)
                {
                    ModelState.AddModelError(string.Empty,
                        "Ya existe un lote con ese número para este producto.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Ocurrió un error al actualizar.");
                }

                await LlenarListasViewModel(loteproductovm);
                return View(loteproductovm);
            }
        }

        // GET: LoteProductos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var lote = await _context.LoteProductos
                .Include(l => l.IdProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdLote == id);

            if (lote == null) return NotFound();

            return View(lote);
        }

        // POST: LoteProductos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lote = await _context.LoteProductos.FindAsync(id);
            if (lote == null) return NotFound();

            try
            {
                _context.LoteProductos.Remove(lote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty,
                    "No se puede eliminar este lote porque tiene movimientos de inventario asociados.");
                return View(lote);
            }
        }

        private async Task<bool> LoteProductoExists(int id)
        {
            return await _context.LoteProductos.AnyAsync(e => e.IdLote == id);
        }
    }
}
