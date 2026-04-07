using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Veterinaria.Application.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Entities;
using Veterinaria.Web.ViewModels.Especies;

namespace Veterinaria.Web.Controllers
{
    public class EspeciesController : Controller
    {
        private readonly IApplicationDbContext _context;

        public EspeciesController(IApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Especies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Especies.ToListAsync());
        }

        // GET: Especies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var especie = await _context.Especies
                .FirstOrDefaultAsync(m => m.IdEspecie == id);
            if (especie == null)
            {
                return NotFound();
            }

            return View(especie);
        }

        // GET: Especies/Create
        public IActionResult Create()
        {
            return View(new EspecieViewModel());
        }

        // POST: Especies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EspecieViewModel especieViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(especieViewModel);
            }
            
            var especie = new Especie
            {
                Nombre = especieViewModel.Nombre.Trim(),
                Descripcion = especieViewModel.Descripcion,
                Activo = true,
                FechaCreacion = DateTime.Now,
            };

            try
            {
                _context.Add(especie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UK_Especie_Nombre") == true)
                {
                    ModelState.AddModelError(nameof(especieViewModel.Nombre),
                        "Ya existe una especie con ese nombre.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Ocurrió un error al guardar. Intente nuevamente.");
                }
                return View(especieViewModel);
            }
        }

        // GET: Especies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var especie = await _context.Especies.FindAsync(id);
            if (especie == null) return NotFound();

            var especieViewModel = new EspecieViewModel
            {
                IdEspecie = especie.IdEspecie,
                Nombre = especie.Nombre,
                Descripcion = especie.Descripcion,
                Activo = especie.Activo
            };

            return View(especieViewModel);
        }

        // POST: Especies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EspecieViewModel especieView)
        {
            if (!ModelState.IsValid) return View(especieView);

            var especie = await _context.Especies.FindAsync(especieView.IdEspecie);
            if (especie == null) return NotFound();

            especie.Nombre = especieView.Nombre!.Trim();
            especie.Descripcion = especieView.Descripcion?.Trim();
            especie.Activo = especieView.Activo;
            especie.FechaModificacion = DateTime.Now;
            
            try
            {
                _context.Update(especie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EspecieExists(especieView.IdEspecie)) return NotFound();
                else throw;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UK_Especie_Nombre") == true)
                {
                    ModelState.AddModelError(nameof(especieView.Nombre),
                        "Ya existe una especie con ese nombre.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Ocurrió un error al guardar. Intente nuevamente.");
                }
                return View(especieView);
            }
        }

        // GET: Especies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var especie = await _context.Especies
                .FirstOrDefaultAsync(m => m.IdEspecie == id);
            if (especie == null)
            {
                return NotFound();
            }

            return View(especie);
        }

        // POST: Especies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var especie = await _context.Especies.FindAsync(id);
            if (especie == null) return NotFound();
            try
            {
                _context.Especies.Remove(especie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty,
                    "No se pudo eliminar la especie. Asegúrese de que no esté asociada a mascotas o razas.");
                return View(especie);
            }

        }

        private bool EspecieExists(int id)
        {
            return _context.Especies.Any(e => e.IdEspecie == id);
        }
    }
}
