using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Veterinaria.Application.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Entities;
using Veterinaria.Web.ViewModels.Vacunas;

namespace Veterinaria.Web.Controllers
{
    public class VacunasController : Controller
    {
        private readonly IApplicationDbContext _context;

        public VacunasController(IApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vacunas
        public async Task<IActionResult> Index()
        {
            var vacunas = await _context.Vacunas
                            .OrderBy(v => v.Nombre)
                            .ToListAsync();

            return View(vacunas);
        }

        // GET: Vacunas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var vacuna = await _context.Vacunas
                .FirstOrDefaultAsync(m => m.IdVacuna == id);

            if (vacuna == null) return NotFound();

            return View(vacuna);
        }

        // GET: Vacunas/Create
        public IActionResult Create()
        {
            return View(new VacunaViewModel());
        }

        // POST: Vacunas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VacunaViewModel vacunavm)
        {
            if (!ModelState.IsValid)
                return View(vacunavm);

            var vacuna = new Vacuna
            {
                Nombre = vacunavm.Nombre!.Trim(),
                Tipo = vacunavm.Tipo!.Trim(),
                FrecuenciaRefuerzoMeses = vacunavm.FrecuenciaRefuerzoMeses,
                EdadPrimeraDosisSemanas = vacunavm.EdadPrimeraDosisSemanas,
                Laboratorio = vacunavm.Laboratorio?.Trim(),
                EnfermedadesPrevine = vacunavm.EnfermedadesPrevine?.Trim(),
                Activo = true,
                FechaCreacion = DateTime.Now
            };

            try
            {
                _context.Add(vacuna);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UK_Vacuna_Nombre") == true)
                {
                    ModelState.AddModelError(nameof(vacunavm.Nombre),
                        "Ya existe una vacuna con ese nombre.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Ocurrió un error al guardar. Intente nuevamente.");
                }

                return View(vacunavm);
            }
        }

        // GET: Vacunas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var vacuna = await _context.Vacunas.FindAsync(id);
            if (vacuna == null) return NotFound();

            var v_vacunavm = new VacunaViewModel
            {
                IdVacuna = vacuna.IdVacuna,
                Nombre = vacuna.Nombre,
                Tipo = vacuna.Tipo,
                FrecuenciaRefuerzoMeses = vacuna.FrecuenciaRefuerzoMeses,
                EdadPrimeraDosisSemanas = vacuna.EdadPrimeraDosisSemanas,
                Laboratorio = vacuna.Laboratorio,
                EnfermedadesPrevine = vacuna.EnfermedadesPrevine,
                Activo = vacuna.Activo
            };

            return View(v_vacunavm);
        }

        // POST: Vacunas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VacunaViewModel vacunavm)
        {
            if (!ModelState.IsValid)
                return View(vacunavm);

            var vacuna = await _context.Vacunas.FindAsync(vacunavm.IdVacuna);
            if (vacuna == null) return NotFound();

            vacuna.Nombre = vacunavm.Nombre!.Trim();
            vacuna.Tipo = vacunavm.Tipo!.Trim();
            vacuna.FrecuenciaRefuerzoMeses = vacunavm.FrecuenciaRefuerzoMeses;
            vacuna.EdadPrimeraDosisSemanas = vacunavm.EdadPrimeraDosisSemanas;
            vacuna.Laboratorio = vacunavm.Laboratorio?.Trim();
            vacuna.EnfermedadesPrevine = vacunavm.EnfermedadesPrevine?.Trim();
            vacuna.Activo = vacunavm.Activo;
            vacuna.FechaModificacion = DateTime.Now;

            try
            {
                _context.Update(vacuna);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VacunaExists(vacunavm.IdVacuna)) return NotFound();
                else throw;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UK_Vacuna_Nombre") == true)
                {
                    ModelState.AddModelError(nameof(vacunavm.Nombre),
                        "Ya existe una vacuna con ese nombre.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Ocurrió un error al actualizar.");
                }

                return View(vacunavm);
            }
        }

        // GET: Vacunas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var vacuna = await _context.Vacunas
                .FirstOrDefaultAsync(m => m.IdVacuna == id);

            if (vacuna == null) return NotFound();

            return View(vacuna);
        }

        // POST: Vacunas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vacuna = await _context.Vacunas.FindAsync(id);
            if (vacuna == null) return NotFound();

            try
            {
                _context.Vacunas.Remove(vacuna);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty,
                    "No se puede eliminar esta vacuna porque tiene historiales asociados.");
                return View(vacuna);
            }
        }

        private bool VacunaExists(int id)
        {
            return _context.Vacunas.Any(e => e.IdVacuna == id);
        }
    }
}
