using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRJ_VETERINARIA.DataAccessLayer.Models;
using PRJ_VETERINARIA.DataAccessLayer.ViewModels.Atenciones;

namespace PRJ_VETERINARIA.BusinessLogicLayer.Controllers
{
    public class AtencionesController : Controller
    {
        private readonly BDVeterinariaContext _context;

        public AtencionesController(BDVeterinariaContext context)
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

        private async Task<IEnumerable<SelectListItem>> GetServiciosSelectAsync(
            int? selectedId = null)
        {
            var servicios = await _context.Servicios
                .Where(s => s.Activo)
                .OrderBy(s => s.Nombre)
                .ToListAsync();

            return servicios.Select(s => new SelectListItem
            {
                Value = s.IdServicio.ToString(),
                Text = $"[{s.CodigoServicio}] {s.Nombre}",
                Selected = s.IdServicio == selectedId
            });
        }

        private async Task LlenarListasViewModel(AtencionViewModel vm)
        {
            vm.MascotasDisponibles = await GetMascotasSelectAsync(vm.IdMascota);
            vm.VeterinariosDisponibles = await GetVeterinariosSelectAsync(vm.IdVeterinario);
            vm.ProductosDisponibles = await GetProductosSelectAsync();
            vm.ServiciosDisponibles = await GetServiciosSelectAsync();
        }

        // ──────────────────────────────────────────────
        // INDEX
        // ──────────────────────────────────────────────
        public async Task<IActionResult> Index()
        {
            var atenciones = await _context.Atencions
                .Include(a => a.IdMascotaNavigation)
                .Include(a => a.IdVeterinarioNavigation)
                .OrderByDescending(a => a.FechaHoraInicio)
                .ToListAsync();

            return View(atenciones);
        }

        // ──────────────────────────────────────────────
        // DETAILS
        // ──────────────────────────────────────────────
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var atencion = await _context.Atencions
                .Include(a => a.IdMascotaNavigation)
                .Include(a => a.IdVeterinarioNavigation)
                .Include(a => a.DetalleAtencions)
                    .ThenInclude(d => d.IdProductoNavigation)
                .Include(a => a.DetalleAtencions)
                    .ThenInclude(d => d.IdServicioNavigation)
                .FirstOrDefaultAsync(m => m.IdAtencion == id);

            if (atencion == null) return NotFound();

            return View(atencion);
        }

        // ──────────────────────────────────────────────
        // CREATE GET
        // ──────────────────────────────────────────────
        public async Task<IActionResult> Create()
        {
            var vm = new AtencionViewModel();
            await LlenarListasViewModel(vm);
            return View(vm);
        }

        // ──────────────────────────────────────────────
        // CREATE POST
        // ──────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AtencionViewModel vm)
        {
            // Validación: al menos un detalle
            if (vm.Detalles == null || !vm.Detalles.Any())
            {
                ModelState.AddModelError(string.Empty,
                    "Debe agregar al menos un producto o servicio al detalle.");
            }

            // Validación: cada detalle debe tener producto O servicio según TipoItem
            if (vm.Detalles != null)
            {
                for (int i = 0; i < vm.Detalles.Count; i++)
                {
                    var d = vm.Detalles[i];
                    if (d.TipoItem == "Producto" && d.IdProducto == null)
                    {
                        ModelState.AddModelError(
                            $"Detalles[{i}].IdProducto",
                            "Debe seleccionar un producto.");
                    }
                    if (d.TipoItem == "Servicio" && d.IdServicio == null)
                    {
                        ModelState.AddModelError(
                            $"Detalles[{i}].IdServicio",
                            "Debe seleccionar un servicio.");
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                await LlenarListasViewModel(vm);
                return View(vm);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var atencion = new Atencion
                {
                    IdMascota = vm.IdMascota!.Value,
                    IdVeterinario = vm.IdVeterinario!.Value,
                    TipoAtencion = vm.TipoAtencion!,
                    MotivoConsulta = vm.MotivoConsulta!.Trim(),
                    Sintomas = vm.Sintomas?.Trim(),
                    ExamenFisico = vm.ExamenFisico?.Trim(),
                    Diagnostico = vm.Diagnostico?.Trim(),
                    Tratamiento = vm.Tratamiento?.Trim(),
                    Recomendaciones = vm.Recomendaciones?.Trim(),
                    PesoAtencion = vm.PesoAtencion,
                    Temperatura = vm.Temperatura,
                    FrecuenciaCardiaca = vm.FrecuenciaCardiaca,
                    FrecuenciaRespiratoria = vm.FrecuenciaRespiratoria,
                    Estado = vm.Estado!,
                    Observaciones = vm.Observaciones?.Trim(),
                    FechaHoraInicio = DateTime.Now,
                    CreatedAt = DateTime.Now
                };

                _context.Add(atencion);
                await _context.SaveChangesAsync();

                // Guardar detalles
                foreach (var item in vm.Detalles!)
                {
                    var detalle = new DetalleAtencion
                    {
                        IdAtencion = atencion.IdAtencion,
                        IdProducto = item.TipoItem == "Producto"
                                              ? item.IdProducto : null,
                        IdServicio = item.TipoItem == "Servicio"
                                              ? item.IdServicio : null,
                        TipoItem = item.TipoItem!,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = item.PrecioUnitario,
                        DescuentoPorcentaje = item.DescuentoPorcentaje,
                        Dosis = item.Dosis?.Trim(),
                        Frecuencia = item.Frecuencia?.Trim(),
                        DuracionDias = item.DuracionDias,
                        Instrucciones = item.Instrucciones?.Trim(),
                        Observaciones = item.Observaciones?.Trim(),
                        CreatedAt = DateTime.Now
                    };
                    _context.Add(detalle);
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

        // ──────────────────────────────────────────────
        // DELETE GET
        // ──────────────────────────────────────────────
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var atencion = await _context.Atencions
                .Include(a => a.IdMascotaNavigation)
                .Include(a => a.IdVeterinarioNavigation)
                .FirstOrDefaultAsync(m => m.IdAtencion == id);

            if (atencion == null) return NotFound();

            return View(atencion);
        }

        // ──────────────────────────────────────────────
        // DELETE POST
        // ──────────────────────────────────────────────
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var atencion = await _context.Atencions.FindAsync(id);
            if (atencion == null) return NotFound();

            try
            {
                _context.Atencions.Remove(atencion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty,
                    "No se puede eliminar esta atención porque tiene ventas asociadas.");
                return View(atencion);
            }
        }
    }
}