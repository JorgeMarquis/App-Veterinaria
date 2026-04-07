using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Veterinaria.Application.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Entities;
using Veterinaria.Web.ViewModels.Razas;

namespace Veterinaria.Web.Controllers
{
    public class RazasController : Controller
    {
        private readonly IApplicationDbContext _context;

        public RazasController(IApplicationDbContext context)
        {
            _context = context;
        }
        private async Task<IEnumerable<SelectListItem>> GetEspeciesSelectAsync(int? selectedId = null)
        {
            var especies = await _context.Especies
                .Where(e => e.Activo)
                .OrderBy(e => e.Nombre)
                .ToListAsync();

            return especies.Select(e => new SelectListItem
            {
                Value = e.IdEspecie.ToString(),
                Text = e.Nombre,
                Selected = e.IdEspecie == selectedId
            });
        }

        // GET: Razas
        public async Task<IActionResult> Index()
        {
            var razas = await _context.Razas
                .Include(r => r.IdEspecieNavigation)
                .OrderBy(r => r.IdEspecieNavigation.Nombre)
                .ThenBy(r => r.Nombre)
                .ToListAsync();
            return View(razas);
        }

        // GET: Razas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var raza = await _context.Razas
                .Include(r => r.IdEspecieNavigation)
                .FirstOrDefaultAsync(m => m.IdRaza == id);

            if (raza == null) return NotFound();

            return View(raza);
        }

        // GET: Razas/Create
        public async Task<IActionResult> Create()
        {
            var razasvm = new RazaViewModel
            {
                EspeciesDisponibles = await GetEspeciesSelectAsync()
            };

            return View(razasvm);
        }

        // POST: Razas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RazaViewModel razaView)
        {
            if (!ModelState.IsValid)
            {
                razaView.EspeciesDisponibles = await GetEspeciesSelectAsync(razaView.IdEspecie);
            }

            var raza = new Raza
            {
                IdEspecie = razaView.IdEspecie!.Value,
                Nombre = razaView.Nombre!.Trim(),
                Dimension = razaView.Dimension?.Trim(),
                Activo = true,
                FechaCreacion = DateTime.Now
            };

            try
            {
                _context.Add(raza);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UK_Raza_Nombre") == true)
                {
                    ModelState.AddModelError(nameof(razaView.Nombre),
                        "Ya existe una raza con ese nombre para esta especie.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Ocurrió un error al guardar. Intente nuevamente.");
                }

                razaView.EspeciesDisponibles = await GetEspeciesSelectAsync(razaView.IdEspecie);
                return View(razaView);
            }
        }

        // GET: Razas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var raza = await _context.Razas.FindAsync(id);
            if (raza == null) return NotFound();

            var razavm = new RazaViewModel
            {
                IdRaza = raza.IdRaza,
                IdEspecie = raza.IdEspecie,
                Nombre = raza.Nombre,
                Dimension = raza.Dimension,
                Activo = raza.Activo,
                EspeciesDisponibles = await GetEspeciesSelectAsync(raza.IdEspecie)
            };
            
            return View(razavm);
        }

        // POST: Razas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RazaViewModel razavm)
        {
            if (!ModelState.IsValid)
            {
                razavm.EspeciesDisponibles = await GetEspeciesSelectAsync(razavm.IdEspecie);
                return View(razavm);
            }

            var raza = await _context.Razas.FindAsync(razavm.IdRaza);
            if (raza == null) return NotFound();

            raza.IdEspecie = razavm.IdEspecie!.Value;
            raza.Nombre = razavm.Nombre!.Trim();
            raza.Dimension = razavm.Dimension?.Trim();
            raza.Activo = razavm.Activo;
            raza.FechaModificacion = DateTime.Now;

            try
            {
                _context.Update(raza);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RazaExists(razavm.IdRaza)) return NotFound();
                else throw;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UK_Raza_Nombre") == true)
                {
                    ModelState.AddModelError(nameof(razavm.Nombre),
                        "Ya existe una raza con ese nombre para esta especie.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Ocurrió un error al actualizar.");
                }

                razavm.EspeciesDisponibles = await GetEspeciesSelectAsync(razavm.IdEspecie);
                return View(razavm);
            }
        }

        // GET: Razas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var raza = await _context.Razas
                .Include(r => r.IdEspecieNavigation)
                .FirstOrDefaultAsync(m => m.IdRaza == id);

            if (raza == null) return NotFound();

            return View(raza);
        }

        // POST: Razas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var raza = await _context.Razas.FindAsync(id);
            if (raza == null) return NotFound();

            try
            {
                _context.Razas.Remove(raza);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty,
                    "No se puede eliminar esta raza porque tiene mascotas asociadas.");
                return View(raza);
            }
        }

        private bool RazaExists(int id)
        {
            return _context.Razas.Any(e => e.IdRaza == id);
        }
    }
}
