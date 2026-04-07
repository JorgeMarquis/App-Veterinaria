using Microsoft.AspNetCore.Mvc;
using Veterinaria.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Entities;

namespace Veterinaria.Web.Controllers
{
    public class KardexsController : Controller
    {
        private readonly IApplicationDbContext _context;

        public KardexsController(IApplicationDbContext context)
        {
            _context = context;
        }

        // ──────────────────────────────────────────────
        // INDEX — Lista todos los movimientos
        // ──────────────────────────────────────────────
        public async Task<IActionResult> Index()
        {
            var movimientos = await _context.Kardices
                .Include(k => k.IdProductoNavigation)
                .Include(k => k.IdLoteNavigation)
                .OrderByDescending(k => k.Fecha)
                .ToListAsync();

            return View(movimientos);
        }

        // ──────────────────────────────────────────────
        // DETAILS — Detalle de un movimiento
        // ──────────────────────────────────────────────
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var kardex = await _context.Kardices
                .Include(k => k.IdProductoNavigation)
                .Include(k => k.IdLoteNavigation)
                .FirstOrDefaultAsync(m => m.IdKardex == id);

            if (kardex == null) return NotFound();

            return View(kardex);
        }

        // ──────────────────────────────────────────────
        // FILTRO POR PRODUCTO — útil para ver el historial
        // de un producto específico
        // ──────────────────────────────────────────────
        public async Task<IActionResult> PorProducto(int? idProducto)
        {
            if (idProducto == null) return RedirectToAction(nameof(Index));

            var producto = await _context.Productos.FindAsync(idProducto);
            if (producto == null) return NotFound();

            var movimientos = await _context.Kardices
                .Where(k => k.IdProducto == idProducto)
                .Include(k => k.IdLoteNavigation)
                .OrderByDescending(k => k.Fecha)
                .ToListAsync();

            ViewBag.Producto = producto.Nombre;
            return View(movimientos);
        }
    }
}
