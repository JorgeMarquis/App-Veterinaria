using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRJ_VETERINARIA.DataAccessLayer.Models;
using PRJ_VETERINARIA.DataAccessLayer.ViewModels.Mascotas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRJ_VETERINARIA.BusinessLogicLayer.Controllers
{
    public class MascotasController : Controller
    {
        private readonly BDVeterinariaContext _context;

        public MascotasController(BDVeterinariaContext context)
        {
            _context = context;
        }

        private async Task<IEnumerable<SelectListItem>> GetClientesSelectAsync(int? selectedId = null)
        {
            var clientes = await _context.Clientes
                .Where(c => c.Activo)
                .OrderBy(c => c.NombreCompleto)
                .ToListAsync();

            return clientes.Select(c => new SelectListItem
            {
                Value = c.IdCliente.ToString(),
                Text = $"{c.NombreCompleto} ({c.NumIdentificacion})",
                Selected = c.IdCliente == selectedId
            });
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

        // Carga TODAS las razas activas con su IdEspecie incluido
        // JavaScript usará IdEspecie para filtrar en el cliente
        private async Task<IEnumerable<SelectListItem>> GetRazasSelectAsync(int? selectedId = null)
        {
            var razas = await _context.Razas
                .Where(r => r.Activo)
                .OrderBy(r => r.Nombre)
                .ToListAsync();

            return razas.Select(r => new SelectListItem
            {
                Value = r.IdRaza.ToString(),
                // Guardamos IdEspecie en el atributo data- para que JS lo use
                Text = r.Nombre,
                Selected = r.IdRaza == selectedId,
                // Pasamos IdEspecie como parte del Value no es suficiente
                // Lo manejamos en la vista con data-especie
            });
        }

        // Método auxiliar que llena las 3 listas del ViewModel
        private async Task LlenarListasViewModel(MascotaViewModel mascotavm)
        {
            mascotavm.ClientesDisponibles = await GetClientesSelectAsync(mascotavm.IdCliente);
            mascotavm.EspeciesDisponibles = await GetEspeciesSelectAsync(mascotavm.IdEspecie);
            mascotavm.RazasDisponibles = await GetRazasSelectAsync(mascotavm.IdRaza);

            var razasJson = await _context.Razas
                .Where(r => r.Activo)
                .OrderBy(r => r.Nombre)
                .Select(r => new {
                    idRaza = r.IdRaza,
                    nombre = r.Nombre,
                    idEspecie = r.IdEspecie
                })
                .ToListAsync();

            ViewBag.RazasJson = System.Text.Json.JsonSerializer.Serialize(razasJson);
        }


        [HttpGet]
        public async Task<IActionResult> GetRazasPorEspecie()
        {
            var razas = await _context.Razas
                .Where(r => r.Activo)
                .OrderBy(r => r.Nombre)
                .Select(r => new {
                    idRaza = r.IdRaza,
                    nombre = r.Nombre,
                    idEspecie = r.IdEspecie
                })
                .ToListAsync();

            // Lo serializamos a JSON para pasarlo a la vista
            ViewBag.RazasJson = System.Text.Json.JsonSerializer.Serialize(razas);
            return Json(razas);
        }


        // GET: Mascotas
        public async Task<IActionResult> Index()
        {
            var mascotas = await _context.Mascota
                .Include(m => m.IdClienteNavigation)
                .Include(m => m.IdEspecieNavigation)
                .Include(m => m.IdRazaNavigation)
                .Where(m => m.Activo)
                .OrderBy(m => m.Nombre)
                .ToListAsync();

            return View(mascotas);
        }

        // GET: Mascotas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var mascota = await _context.Mascota
                .Include(m => m.IdClienteNavigation)
                .Include(m => m.IdEspecieNavigation)
                .Include(m => m.IdRazaNavigation)
                .FirstOrDefaultAsync(m => m.IdMascota == id);

            if (mascota == null) return NotFound();

            return View(mascota);
        }

        // GET: Mascotas/Create
        public async Task<IActionResult> Create()
        {
            var v_mascotavm = new MascotaViewModel();
            await LlenarListasViewModel(v_mascotavm);
            return View(v_mascotavm);
        }

        // POST: Mascotas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MascotaViewModel mascotavm)
        {
            if (!ModelState.IsValid)
            {
                await LlenarListasViewModel(mascotavm);
                return View(mascotavm);
            }

            var mascota = new Mascota
            {
                IdCliente = mascotavm.IdCliente!.Value,
                Nombre = mascotavm.Nombre!.Trim(),
                IdEspecie = mascotavm.IdEspecie!.Value,
                IdRaza = mascotavm.IdRaza,
                FechaNacimiento = mascotavm.FechaNacimiento,
                Sexo = mascotavm.Sexo!,
                ColorPelaje = mascotavm.ColorPelaje?.Trim(),
                MicrochipId = mascotavm.MicrochipId?.Trim() ?? string.Empty,
                FotoUrl = mascotavm.FotoUrl?.Trim(),
                FechaFallecimiento = mascotavm.FechaFallecimiento,
                Activo = true,
                CreatedAt = DateTime.Now
            };

            try
            {
                _context.Add(mascota);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UK_Mascota_Microchip") == true)
                {
                    ModelState.AddModelError(nameof(mascotavm.MicrochipId),
                        "El microchip ya está registrado para otra mascota.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Ocurrió un error al guardar. Intente nuevamente.");
                }

                await LlenarListasViewModel(mascotavm);
                return View(mascotavm);
            }
        }

        // GET: Mascotas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var mascota = await _context.Mascota.FindAsync(id);
            if (mascota == null) return NotFound();

            var v_mascotavm = new MascotaViewModel
            {
                IdMascota = mascota.IdMascota,
                IdCliente = mascota.IdCliente,
                Nombre = mascota.Nombre,
                IdEspecie = mascota.IdEspecie,
                IdRaza = mascota.IdRaza,
                FechaNacimiento = mascota.FechaNacimiento,
                Sexo = mascota.Sexo,
                ColorPelaje = mascota.ColorPelaje,
                MicrochipId = mascota.MicrochipId,
                FotoUrl = mascota.FotoUrl,
                FechaFallecimiento = mascota.FechaFallecimiento,
                Activo = mascota.Activo
            };

            await LlenarListasViewModel(v_mascotavm);
            return View(v_mascotavm);
        }

        // POST: Mascotas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MascotaViewModel mascotavm)
        {
            if (!ModelState.IsValid)
            {
                await LlenarListasViewModel(mascotavm);
                return View(mascotavm);
            }

            var mascota = await _context.Mascota.FindAsync(mascotavm.IdMascota);
            if (mascota == null) return NotFound();

            mascota.IdCliente = mascotavm.IdCliente!.Value;
            mascota.Nombre = mascotavm.Nombre!.Trim();
            mascota.IdEspecie = mascotavm.IdEspecie!.Value;
            mascota.IdRaza = mascotavm.IdRaza;
            mascota.FechaNacimiento = mascotavm.FechaNacimiento;
            mascota.Sexo = mascotavm.Sexo!;
            mascota.ColorPelaje = mascotavm.ColorPelaje?.Trim();
            mascota.MicrochipId = mascotavm.MicrochipId?.Trim() ?? string.Empty;
            mascota.FotoUrl = mascotavm.FotoUrl?.Trim();
            mascota.FechaFallecimiento = mascotavm.FechaFallecimiento;
            mascota.Activo = mascotavm.Activo;
            mascota.UpdatedAt = DateTime.Now;

            try
            {
                _context.Update(mascota);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await MascotaExistsAsync(mascotavm.IdMascota)) return NotFound();
                else throw;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UK_Mascota_Microchip") == true)
                {
                    ModelState.AddModelError(nameof(mascotavm.MicrochipId),
                        "El microchip ya está registrado para otra mascota.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Ocurrió un error al actualizar.");
                }

                await LlenarListasViewModel(mascotavm);
                return View(mascotavm);
            }
        }

        // GET: Mascotas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var mascota = await _context.Mascota
                .Include(m => m.IdClienteNavigation)
                .Include(m => m.IdEspecieNavigation)
                .Include(m => m.IdRazaNavigation)
                .FirstOrDefaultAsync(m => m.IdMascota == id);

            if (mascota == null) return NotFound();

            return View(mascota);
        }

        // POST: Mascotas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mascota = await _context.Mascota.FindAsync(id);
            if (mascota == null) return NotFound();

            try
            {
                _context.Mascota.Remove(mascota);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty,
                    "No se puede eliminar esta mascota porque tiene atenciones o vacunas asociadas.");
                return View(mascota);
            }
        }

        private async Task<bool> MascotaExistsAsync(int id)
        {
            return await _context.Mascota.AnyAsync(e => e.IdMascota == id);
        }

    }
}
