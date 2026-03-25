using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRJ_VETERINARIA.Models;
using PRJ_VETERINARIA.ViewModels.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRJ_VETERINARIA.Controllers
{
    public class ServiciosController : Controller
    {
        private readonly BDVeterinariaContext _context;

        public ServiciosController(BDVeterinariaContext context)
        {
            _context = context;
        }

        private async Task<IEnumerable<SelectListItem>> GetTiposServicioSelectAsync(
            int? selectedId = null)
        {
            var tipos = await _context.TipoServicios
                .Where(t => t.Activo)
                .OrderBy(t => t.Nombre)
                .ToListAsync();

            return tipos.Select(t => new SelectListItem
            {
                Value = t.IdTipoServicio.ToString(),
                Text = t.Nombre,
                Selected = t.IdTipoServicio == selectedId
            });
        }

        private async Task LlenarListasViewModel(ServicioViewModel vm)
        {
            vm.TiposServicioDisponibles =
                await GetTiposServicioSelectAsync(vm.IdTipoServicio);
        }

        // GET: Servicios
        public async Task<IActionResult> Index()
        {
            var servicios = await _context.Servicios
                .Include(s => s.IdTipoServicioNavigation)
                .Where(s => s.Activo)
                .OrderBy(s => s.Nombre)
                .ToListAsync();

            return View(servicios);
        }

        // GET: Servicios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var servicio = await _context.Servicios
                .Include(s => s.IdTipoServicioNavigation)
                .FirstOrDefaultAsync(m => m.IdServicio == id);

            if (servicio == null) return NotFound();

            return View(servicio);
        }

        // GET: Servicios/Create
        public async Task<IActionResult> Create()
        {
            var v_serviciovm = new ServicioViewModel();
            await LlenarListasViewModel(v_serviciovm);
            return View(v_serviciovm);
        }

        // POST: Servicios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServicioViewModel servicovm)
        {
            if (servicovm.RequierePreparacion &&
                string.IsNullOrWhiteSpace(servicovm.InstruccionesPreparacion))
            {
                ModelState.AddModelError(nameof(servicovm.InstruccionesPreparacion),
                    "Si el servicio requiere preparación, debe ingresar las instrucciones.");
            }

            if (!ModelState.IsValid)
            {
                await LlenarListasViewModel(servicovm);
                return View(servicovm);
            }

            var servicio = new Servicio
            {
                IdTipoServicio = servicovm.IdTipoServicio!.Value,
                CodigoServicio = servicovm.CodigoServicio!.Trim().ToUpper(),
                Nombre = servicovm.Nombre!.Trim(),
                Descripcion = servicovm.Descripcion?.Trim(),
                PrecioBase = servicovm.PrecioBase,
                RequiereAyuno = servicovm.RequiereAyuno,
                RequierePreparacion = servicovm.RequierePreparacion,
                InstruccionesPreparacion = servicovm.InstruccionesPreparacion?.Trim(),
                Observaciones = servicovm.Observaciones?.Trim(),
                Activo = true,
                CreatedAt = DateTime.Now
            };

            try
            {
                _context.Add(servicio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UK_Servicio_Codigo") == true)
                {
                    ModelState.AddModelError(nameof(servicovm.CodigoServicio),
                        "El código de servicio ya está registrado.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Ocurrió un error al guardar. Intente nuevamente.");
                }

                await LlenarListasViewModel(servicovm);
                return View(servicovm);
            }
        }

        // GET: Servicios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null) return NotFound();

            var vm = new ServicioViewModel
            {
                IdServicio = servicio.IdServicio,
                IdTipoServicio = servicio.IdTipoServicio,
                CodigoServicio = servicio.CodigoServicio,
                Nombre = servicio.Nombre,
                Descripcion = servicio.Descripcion,
                PrecioBase = servicio.PrecioBase,
                RequiereAyuno = servicio.RequiereAyuno,
                RequierePreparacion = servicio.RequierePreparacion,
                InstruccionesPreparacion = servicio.InstruccionesPreparacion,
                Observaciones = servicio.Observaciones,
                Activo = servicio.Activo
            };

            await LlenarListasViewModel(vm);
            return View(vm);
        }

        // POST: Servicios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ServicioViewModel serviciovm)
        {
            if (serviciovm.RequierePreparacion &&
                string.IsNullOrWhiteSpace(serviciovm.InstruccionesPreparacion))
            {
                ModelState.AddModelError(nameof(serviciovm.InstruccionesPreparacion),
                    "Si el servicio requiere preparación, debe ingresar las instrucciones.");
            }

            if (!ModelState.IsValid)
            {
                await LlenarListasViewModel(serviciovm);
                return View(serviciovm);
            }

            var servicio = await _context.Servicios.FindAsync(serviciovm.IdServicio);
            if (servicio == null) return NotFound();

            servicio.IdTipoServicio = serviciovm.IdTipoServicio!.Value;
            servicio.CodigoServicio = serviciovm.CodigoServicio!.Trim().ToUpper();
            servicio.Nombre = serviciovm.Nombre!.Trim();
            servicio.Descripcion = serviciovm.Descripcion?.Trim();
            servicio.PrecioBase = serviciovm.PrecioBase;
            servicio.RequiereAyuno = serviciovm.RequiereAyuno;
            servicio.RequierePreparacion = serviciovm.RequierePreparacion;
            servicio.InstruccionesPreparacion = serviciovm.InstruccionesPreparacion?.Trim();
            servicio.Observaciones = serviciovm.Observaciones?.Trim();
            servicio.Activo = serviciovm.Activo;
            servicio.UpdatedAt = DateTime.Now;

            try
            {
                _context.Update(servicio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ServicioExists(serviciovm.IdServicio)) return NotFound();
                else throw;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UK_Servicio_Codigo") == true)
                {
                    ModelState.AddModelError(nameof(serviciovm.CodigoServicio),
                        "El código de servicio ya está registrado.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Ocurrió un error al actualizar.");
                }

                await LlenarListasViewModel(serviciovm);
                return View(serviciovm);
            }
        }

        // GET: Servicios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var servicio = await _context.Servicios
                .Include(s => s.IdTipoServicioNavigation)
                .FirstOrDefaultAsync(m => m.IdServicio == id);

            if (servicio == null) return NotFound();

            return View(servicio);
        }

        // POST: Servicios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null) return NotFound();

            try
            {
                _context.Servicios.Remove(servicio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty,
                    "No se puede eliminar este servicio porque tiene atenciones asociadas.");
                return View(servicio);
            }
        }

        private async Task<bool> ServicioExists(int id)
        {
            return await _context.Servicios.AnyAsync(e => e.IdServicio == id);
        }
    }
}
