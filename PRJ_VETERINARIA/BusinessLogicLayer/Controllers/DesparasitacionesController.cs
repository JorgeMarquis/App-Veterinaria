using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRJ_VETERINARIA.DataAccessLayer.Models;
using PRJ_VETERINARIA.DataAccessLayer.ViewModels.Desparasitaciones;

namespace PRJ_VETERINARIA.BusinessLogicLayer.Controllers
{
    public class DesparasitacionesController : Controller
    {
        private readonly BDVeterinariaContext _context;

        public DesparasitacionesController(BDVeterinariaContext context)
        {
            _context = context;
        }

        private async Task<IEnumerable<SelectListItem>> GetMascotasSelectAsync(
            int? selectedId = null)
        {
            var mascotas = await _context.Mascota
                .Where(m => m.Activo)
                .Include(m => m.IdClienteNavigation)
                .OrderBy(m => m.Nombre)
                .ToListAsync();

            return mascotas.Select(m => new SelectListItem
            {
                Value = m.IdMascota.ToString(),
                Text = $"{m.Nombre} ({m.IdClienteNavigation.NombreCompleto})",
                Selected = m.IdMascota == selectedId
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

        private async Task<IEnumerable<SelectListItem>> GetVeterinariosSelectAsync(
            int? selectedId = null)
        {
            var veterinarios = await _context.Usuarios
                .Where(u => u.Activo && u.Rol == "Veterinario")
                .OrderBy(u => u.NombreCompleto)
                .ToListAsync();

            return veterinarios.Select(u => new SelectListItem
            {
                Value = u.IdUsuario.ToString(),
                Text = u.NombreCompleto,
                Selected = u.IdUsuario == selectedId
            });
        }

        private async Task LlenarListasViewModel(DesparasitacionViewModel vm)
        {
            vm.MascotasDisponibles = await GetMascotasSelectAsync(vm.IdMascota);
            vm.ProductosDisponibles = await GetProductosSelectAsync(vm.IdProducto);
            vm.VeterinariosDisponibles = await GetVeterinariosSelectAsync(vm.IdVeterinario);
        }

        // ──────────────────────────────────────────────
        // INDEX
        // ──────────────────────────────────────────────
        public async Task<IActionResult> Index()
        {
            var desparasitaciones = await _context.Desparasitacions
                .Include(d => d.IdMascotaNavigation)
                .Include(d => d.IdProductoNavigation)
                .Include(d => d.IdVeterinarioNavigation)
                .OrderByDescending(d => d.FechaAplicacion)
                .ToListAsync();

            return View(desparasitaciones);
        }

        // ──────────────────────────────────────────────
        // DETAILS
        // ──────────────────────────────────────────────
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var desparasitacion = await _context.Desparasitacions
                .Include(d => d.IdMascotaNavigation)
                .Include(d => d.IdProductoNavigation)
                .Include(d => d.IdVeterinarioNavigation)
                .FirstOrDefaultAsync(m => m.IdDesparasitacion == id);

            if (desparasitacion == null) return NotFound();

            return View(desparasitacion);
        }

        // ──────────────────────────────────────────────
        // CREATE GET
        // ──────────────────────────────────────────────
        public async Task<IActionResult> Create()
        {
            var vm = new DesparasitacionViewModel();
            await LlenarListasViewModel(vm);
            return View(vm);
        }

        // ──────────────────────────────────────────────
        // CREATE POST
        // ──────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DesparasitacionViewModel vm)
        {
            if (vm.FechaProxima.HasValue &&
                vm.FechaProxima.Value <= DateOnly.FromDateTime(vm.FechaAplicacion))
            {
                ModelState.AddModelError(nameof(vm.FechaProxima),
                    "La fecha próxima debe ser posterior a la fecha de aplicación.");
            }

            if (!ModelState.IsValid)
            {
                await LlenarListasViewModel(vm);
                return View(vm);
            }

            var desparasitacion = new Desparasitacion
            {
                IdMascota = vm.IdMascota!.Value,
                IdProducto = vm.IdProducto!.Value,
                IdVeterinario = vm.IdVeterinario!.Value,
                TipoDesparasitacion = vm.TipoDesparasitacion!,
                FechaAplicacion = vm.FechaAplicacion,
                FechaProxima = vm.FechaProxima,
                PesoAplicacion = vm.PesoAplicacion,
                DosisAplicada = vm.DosisAplicada?.Trim(),
                Observaciones = vm.Observaciones?.Trim(),
                CreatedAt = DateTime.Now
            };

            try
            {
                _context.Add(desparasitacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty,
                    "Ocurrió un error al guardar. Intente nuevamente.");
                await LlenarListasViewModel(vm);
                return View(vm);
            }
        }

        // ──────────────────────────────────────────────
        // EDIT GET
        // ──────────────────────────────────────────────
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var desparasitacion = await _context.Desparasitacions.FindAsync(id);
            if (desparasitacion == null) return NotFound();

            var vm = new DesparasitacionViewModel
            {
                IdDesparasitacion = desparasitacion.IdDesparasitacion,
                IdMascota = desparasitacion.IdMascota,
                IdProducto = desparasitacion.IdProducto,
                IdVeterinario = desparasitacion.IdVeterinario,
                TipoDesparasitacion = desparasitacion.TipoDesparasitacion,
                FechaAplicacion = desparasitacion.FechaAplicacion,
                FechaProxima = desparasitacion.FechaProxima,
                PesoAplicacion = desparasitacion.PesoAplicacion,
                DosisAplicada = desparasitacion.DosisAplicada,
                Observaciones = desparasitacion.Observaciones
            };

            await LlenarListasViewModel(vm);
            return View(vm);
        }

        // ──────────────────────────────────────────────
        // EDIT POST
        // ──────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DesparasitacionViewModel vm)
        {
            if (vm.FechaProxima.HasValue &&
                vm.FechaProxima.Value <= DateOnly.FromDateTime(vm.FechaAplicacion))
            {
                ModelState.AddModelError(nameof(vm.FechaProxima),
                    "La fecha próxima debe ser posterior a la fecha de aplicación.");
            }

            if (!ModelState.IsValid)
            {
                await LlenarListasViewModel(vm);
                return View(vm);
            }

            var desparasitacion = await _context.Desparasitacions
                .FindAsync(vm.IdDesparasitacion);
            if (desparasitacion == null) return NotFound();

            desparasitacion.IdMascota = vm.IdMascota!.Value;
            desparasitacion.IdProducto = vm.IdProducto!.Value;
            desparasitacion.IdVeterinario = vm.IdVeterinario!.Value;
            desparasitacion.TipoDesparasitacion = vm.TipoDesparasitacion!;
            desparasitacion.FechaAplicacion = vm.FechaAplicacion;
            desparasitacion.FechaProxima = vm.FechaProxima;
            desparasitacion.PesoAplicacion = vm.PesoAplicacion;
            desparasitacion.DosisAplicada = vm.DosisAplicada?.Trim();
            desparasitacion.Observaciones = vm.Observaciones?.Trim();

            try
            {
                _context.Update(desparasitacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Desparasitacions.Any(e =>
                    e.IdDesparasitacion == vm.IdDesparasitacion))
                    return NotFound();
                else throw;
            }
        }

        // ──────────────────────────────────────────────
        // DELETE GET
        // ──────────────────────────────────────────────
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var desparasitacion = await _context.Desparasitacions
                .Include(d => d.IdMascotaNavigation)
                .Include(d => d.IdProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdDesparasitacion == id);

            if (desparasitacion == null) return NotFound();

            return View(desparasitacion);
        }

        // ──────────────────────────────────────────────
        // DELETE POST
        // ──────────────────────────────────────────────
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var desparasitacion = await _context.Desparasitacions.FindAsync(id);
            if (desparasitacion == null) return NotFound();

            try
            {
                _context.Desparasitacions.Remove(desparasitacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty,
                    "No se puede eliminar este registro.");
                return View(desparasitacion);
            }
        }
    }
}