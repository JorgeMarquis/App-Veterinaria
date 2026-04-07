using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Veterinaria.Application.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Entities;
using Veterinaria.Web.ViewModels.TipoServicios;

namespace Veterinaria.Web.Controllers
{
    public class TipoServiciosController : Controller
    {
        private readonly IApplicationDbContext _context;

        public TipoServiciosController(IApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TipoServicios
        public async Task<IActionResult> Index()
        {
            var tiposServicio = await _context.TipoServicios
                .OrderBy(t => t.Nombre)
                .Select(t => new TipoServicioViewModel
                {
                    IdTipoServicio = t.IdTipoServicio,
                    Nombre = t.Nombre,
                    EsMedico = t.EsMedico,
                    DuracionEstimadaMin = t.DuracionEstimadaMin,
                    Activo = t.Activo
                })
                .ToListAsync();

            return View(tiposServicio);
        }

        // GET: TipoServicios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null) return NotFound();

            var tipoServicio = await _context.TipoServicios
                .FirstOrDefaultAsync(m => m.IdTipoServicio == id);

            if (tipoServicio == null) return NotFound();

            return View(tipoServicio);
        }

        // GET: TipoServicios/Create
        public IActionResult Create()
        {
            return View(new TipoServicioViewModel());
        }

        // POST: TipoServicios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TipoServicioViewModel tsViewModel)
        {
            if (!ModelState.IsValid) return View(tsViewModel);

            var tipoServicio = new TipoServicio
            {
                Nombre = tsViewModel.Nombre!.Trim(),
                EsMedico = tsViewModel.EsMedico,
                DuracionEstimadaMin = tsViewModel.DuracionEstimadaMin,
                Activo = true,
                FechaCreacion = DateTime.Now
            };

            try
            {
                _context.Add(tipoServicio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UK_TipoServicio_Nombre") == true)
                {
                    ModelState.AddModelError(nameof(tsViewModel.Nombre),
                        "Ya existe un tipo de servicio con ese nombre.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Ocurrió un error al guardar. Intente nuevamente.");
                }

                return View(tsViewModel);
            }
        }

        // GET: TipoServicios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var tipoServicio = await _context.TipoServicios.FindAsync(id);
            if (tipoServicio == null) return NotFound();

            var v_tsViewModel = new TipoServicioViewModel
            {
                IdTipoServicio = tipoServicio.IdTipoServicio,
                Nombre = tipoServicio.Nombre,
                EsMedico = tipoServicio.EsMedico,
                DuracionEstimadaMin = tipoServicio.DuracionEstimadaMin,
                Activo = tipoServicio.Activo
            };

            return View(v_tsViewModel);
        }

        // POST: TipoServicios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TipoServicioViewModel tsViewModel)
        {
            if (!ModelState.IsValid)
                return View(tsViewModel);

            var tipoServicio = await _context.TipoServicios.FindAsync(tsViewModel.IdTipoServicio);
            if (tipoServicio == null) return NotFound();

            tipoServicio.Nombre = tsViewModel.Nombre!.Trim();
            tipoServicio.EsMedico = tsViewModel.EsMedico;
            tipoServicio.DuracionEstimadaMin = tsViewModel.DuracionEstimadaMin;
            tipoServicio.Activo = tsViewModel.Activo;
            tipoServicio.FechaModificacion = DateTime.Now;

            try
            {
                _context.Update(tipoServicio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoServicioExists(tsViewModel.IdTipoServicio)) return NotFound();
                else throw;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UK_TipoServicio_Nombre") == true)
                {
                    ModelState.AddModelError(nameof(tsViewModel.Nombre),
                        "Ya existe un tipo de servicio con ese nombre.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Ocurrió un error al actualizar.");
                }

                return View(tsViewModel);
            }
        }

        // GET: TipoServicios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var tipoServicio = await _context.TipoServicios
                .FirstOrDefaultAsync(m => m.IdTipoServicio == id);

            if (tipoServicio == null) return NotFound();

            return View(tipoServicio);
        }

        // POST: TipoServicios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoServicio = await _context.TipoServicios.FindAsync(id);
            if (tipoServicio == null) return NotFound();

            try
            {
                _context.TipoServicios.Remove(tipoServicio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty,
                    "No se puede eliminar este tipo de servicio porque tiene servicios asociados.");
                return View(tipoServicio);
            }
        }

        private bool TipoServicioExists(int id)
        {
            return _context.TipoServicios.Any(e => e.IdTipoServicio == id);
        }
    }
}
