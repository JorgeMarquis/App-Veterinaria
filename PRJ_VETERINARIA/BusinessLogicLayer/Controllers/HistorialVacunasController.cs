using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRJ_VETERINARIA.DataAccessLayer.Models;
using PRJ_VETERINARIA.DataAccessLayer.ViewModels.HistorialVacunas;

namespace PRJ_VETERINARIA.BusinessLogicLayer.Controllers
{
    public class HistorialVacunasController : Controller
    {
        private readonly BDVeterinariaContext _context;

        public HistorialVacunasController(BDVeterinariaContext context)
        {
            _context = context;
        }

        // ──────────────────────────────────────────────
        // Métodos privados — SelectLists
        // ──────────────────────────────────────────────
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

        private async Task<IEnumerable<SelectListItem>> GetVacunasSelectAsync(
            int? selectedId = null)
        {
            var vacunas = await _context.Vacunas
                .Where(v => v.Activo)
                .OrderBy(v => v.Nombre)
                .ToListAsync();

            return vacunas.Select(v => new SelectListItem
            {
                Value = v.IdVacuna.ToString(),
                Text = v.Nombre,
                Selected = v.IdVacuna == selectedId
            });
        }

        private async Task<IEnumerable<SelectListItem>> GetLotesSelectAsync(
            int? selectedId = null)
        {
            // Solo lotes con stock disponible y no vencidos
            var lotes = await _context.LoteProductos
                .Where(l => l.Activo &&
                            l.CantidadActual > 0 &&
                            l.FechaVencimiento > DateOnly.FromDateTime(DateTime.Today))
                .Include(l => l.IdProductoNavigation)
                .OrderBy(l => l.FechaVencimiento)
                .ToListAsync();

            return lotes.Select(l => new SelectListItem
            {
                Value = l.IdLote.ToString(),
                Text = $"{l.IdProductoNavigation.Nombre} - Lote: {l.NumeroLote} (Vence: {l.FechaVencimiento})",
                Selected = l.IdLote == selectedId
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

        private async Task LlenarListasViewModel(HistorialVacunaViewModel vm)
        {
            vm.MascotasDisponibles = await GetMascotasSelectAsync(vm.IdMascota);
            vm.VacunasDisponibles = await GetVacunasSelectAsync(vm.IdVacuna);
            vm.LotesDisponibles = await GetLotesSelectAsync(vm.IdLote);
            vm.VeterinariosDisponibles = await GetVeterinariosSelectAsync(vm.IdVeterinario);
        }

        // INDEX
        public async Task<IActionResult> Index()
        {
            var historiales = await _context.HistorialVacunas
                .Include(h => h.IdMascotaNavigation)
                .Include(h => h.IdVacunaNavigation)
                .Include(h => h.IdVeterinarioNavigation)
                .OrderByDescending(h => h.FechaAplicacion)
                .ToListAsync();

            return View(historiales);
        }

        // DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var historial = await _context.HistorialVacunas
                .Include(h => h.IdMascotaNavigation)
                .Include(h => h.IdVacunaNavigation)
                .Include(h => h.IdLoteNavigation)
                .Include(h => h.IdVeterinarioNavigation)
                .FirstOrDefaultAsync(m => m.IdHistorial == id);

            if (historial == null) return NotFound();

            return View(historial);
        }

        // CREATE GET
        public async Task<IActionResult> Create()
        {
            var vm = new HistorialVacunaViewModel();
            await LlenarListasViewModel(vm);
            return View(vm);
        }

        // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HistorialVacunaViewModel vm)
        {
            // Validación: fecha de refuerzo debe ser posterior a la aplicación
            if (vm.FechaProximoRefuerzo.HasValue &&
                vm.FechaProximoRefuerzo.Value <= DateOnly.FromDateTime(vm.FechaAplicacion))
            {
                ModelState.AddModelError(nameof(vm.FechaProximoRefuerzo),
                    "La fecha del próximo refuerzo debe ser posterior a la fecha de aplicación.");
            }

            if (!ModelState.IsValid)
            {
                await LlenarListasViewModel(vm);
                return View(vm);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var historial = new HistorialVacuna
                {
                    IdMascota = vm.IdMascota!.Value,
                    IdVacuna = vm.IdVacuna!.Value,
                    IdLote = vm.IdLote!.Value,
                    IdVeterinario = vm.IdVeterinario!.Value,
                    FechaAplicacion = vm.FechaAplicacion,
                    FechaProximoRefuerzo = vm.FechaProximoRefuerzo,
                    DosisNumero = vm.DosisNumero,
                    ReaccionesAdversas = vm.ReaccionesAdversas?.Trim(),
                    Observaciones = vm.Observaciones?.Trim(),
                    CreatedAt = DateTime.Now
                };

                _context.Add(historial);

                // Descontar del stock del lote
                var lote = await _context.LoteProductos.FindAsync(vm.IdLote!.Value);
                if (lote != null)
                {
                    if (lote.CantidadActual < 1)
                    {
                        ModelState.AddModelError(nameof(vm.IdLote),
                            "El lote seleccionado no tiene stock disponible.");
                        await transaction.RollbackAsync();
                        await LlenarListasViewModel(vm);
                        return View(vm);
                    }
                    lote.CantidadActual -= 1;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError(string.Empty,
                    "Ocurrió un error al guardar. Intente nuevamente.");
                await LlenarListasViewModel(vm);
                return View(vm);
            }
        }

        // DELETE GET
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var historial = await _context.HistorialVacunas
                .Include(h => h.IdMascotaNavigation)
                .Include(h => h.IdVacunaNavigation)
                .FirstOrDefaultAsync(m => m.IdHistorial == id);

            if (historial == null) return NotFound();

            return View(historial);
        }

        // DELETE POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var historial = await _context.HistorialVacunas.FindAsync(id);
            if (historial == null) return NotFound();

            try
            {
                _context.HistorialVacunas.Remove(historial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty,
                    "No se puede eliminar este historial.");
                return View(historial);
            }
        }
    }
}