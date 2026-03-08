using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRJ_VETERINARIA.Models;

namespace PRJ_VETERINARIA.Controllers
{
    public class RazasController : Controller
    {
        private readonly BDVeterinariaContext _context;

        public RazasController(BDVeterinariaContext context)
        {
            _context = context;
        }

        // GET: Razas
        public async Task<IActionResult> Index()
        {
            var bDVeterinariaContext = _context.Razas.Include(r => r.IdEspecieNavigation);
            return View(await bDVeterinariaContext.ToListAsync());
        }

        // GET: Razas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raza = await _context.Razas
                .Include(r => r.IdEspecieNavigation)
                .FirstOrDefaultAsync(m => m.IdRaza == id);
            if (raza == null)
            {
                return NotFound();
            }

            return View(raza);
        }

        // GET: Razas/Create
        public IActionResult Create()
        {
            ViewData["IdEspecie"] = new SelectList(_context.Especies, "IdEspecie", "IdEspecie");
            return View();
        }

        // POST: Razas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRaza,IdEspecie,Nombre,Dimension,Activo,FechaCreacion,FechaModificacion")] Raza raza)
        {
            if (ModelState.IsValid)
            {
                _context.Add(raza);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEspecie"] = new SelectList(_context.Especies, "IdEspecie", "IdEspecie", raza.IdEspecie);
            return View(raza);
        }

        // GET: Razas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raza = await _context.Razas.FindAsync(id);
            if (raza == null)
            {
                return NotFound();
            }
            ViewData["IdEspecie"] = new SelectList(_context.Especies, "IdEspecie", "IdEspecie", raza.IdEspecie);
            return View(raza);
        }

        // POST: Razas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRaza,IdEspecie,Nombre,Dimension,Activo,FechaCreacion,FechaModificacion")] Raza raza)
        {
            if (id != raza.IdRaza)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(raza);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RazaExists(raza.IdRaza))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEspecie"] = new SelectList(_context.Especies, "IdEspecie", "IdEspecie", raza.IdEspecie);
            return View(raza);
        }

        // GET: Razas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raza = await _context.Razas
                .Include(r => r.IdEspecieNavigation)
                .FirstOrDefaultAsync(m => m.IdRaza == id);
            if (raza == null)
            {
                return NotFound();
            }

            return View(raza);
        }

        // POST: Razas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var raza = await _context.Razas.FindAsync(id);
            if (raza != null)
            {
                _context.Razas.Remove(raza);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RazaExists(int id)
        {
            return _context.Razas.Any(e => e.IdRaza == id);
        }
    }
}
