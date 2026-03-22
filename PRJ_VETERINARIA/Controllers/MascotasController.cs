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
    public class MascotasController : Controller
    {
        private readonly BDVeterinariaContext _context;

        public MascotasController(BDVeterinariaContext context)
        {
            _context = context;
        }

        // GET: Mascotas
        public async Task<IActionResult> Index()
        {
            var bDVeterinariaContext = _context.Mascota
                .Include(m => m.IdClienteNavigation)
                .Include(m => m.IdEspecieNavigation)
                .Include(m => m.IdRazaNavigation);
            return View(await bDVeterinariaContext.ToListAsync());
        }

        // GET: Mascotas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mascota = await _context.Mascota
                .Include(m => m.IdClienteNavigation)
                .Include(m => m.IdEspecieNavigation)
                .Include(m => m.IdRazaNavigation)
                .FirstOrDefaultAsync(m => m.IdMascota == id);
            if (mascota == null)
            {
                return NotFound();
            }

            return View(mascota);
        }

        // GET: Mascotas/Create
        public IActionResult Create()
        {
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "NombreCompleto");
            ViewData["IdEspecie"] = new SelectList(_context.Especies, "IdEspecie", "Nombre");
            ViewData["IdRaza"] = new SelectList(_context.Razas, "IdRaza", "Nombre");
            return View();
        }

        // POST: Mascotas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMascota,IdCliente,Nombre,IdEspecie,IdRaza,FechaNacimiento,Sexo,ColorPelaje,MicrochipId,FotoUrl,FechaFallecimiento,Activo,CreatedAt,CreatedBy,UpdatedAt,UpdatedBy")] Mascota mascota)
        {
            if (ModelState.IsValid)
            {
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
                        ModelState.AddModelError("MicrochipId", "El microchip ya está registrado para otra mascota.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Ocurrió un error al guardar la mascota. Intente nuevamente.");
                    }
                }
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "IdCliente", mascota.IdCliente);
            ViewData["IdEspecie"] = new SelectList(_context.Especies, "IdEspecie", "IdEspecie", mascota.IdEspecie);
            ViewData["IdRaza"] = new SelectList(_context.Razas, "IdRaza", "IdRaza", mascota.IdRaza);
            return View(mascota);
        }

        // GET: Mascotas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mascota = await _context.Mascota.FindAsync(id);
            if (mascota == null)
            {
                return NotFound();
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "IdCliente", mascota.IdCliente);
            ViewData["IdEspecie"] = new SelectList(_context.Especies, "IdEspecie", "IdEspecie", mascota.IdEspecie);
            ViewData["IdRaza"] = new SelectList(_context.Razas, "IdRaza", "IdRaza", mascota.IdRaza);
            return View(mascota);
        }

        // POST: Mascotas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMascota,IdCliente,Nombre,IdEspecie,IdRaza,FechaNacimiento,Sexo,ColorPelaje,MicrochipId,FotoUrl,FechaFallecimiento,Activo,CreatedAt,CreatedBy,UpdatedAt,UpdatedBy")] Mascota mascota)
        {
            if (id != mascota.IdMascota)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mascota);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MascotaExists(mascota.IdMascota))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException?.Message.Contains("UK_Mascota_Microchip") == true)
                    {
                        ModelState.AddModelError("MicrochipId", "El microchip ya está registrado para otra mascota.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Ocurrió un error al actualizar la mascota.");
                    }
                }
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "IdCliente", mascota.IdCliente);
            ViewData["IdEspecie"] = new SelectList(_context.Especies, "IdEspecie", "IdEspecie", mascota.IdEspecie);
            ViewData["IdRaza"] = new SelectList(_context.Razas, "IdRaza", "IdRaza", mascota.IdRaza);
            return View(mascota);
        }

        // GET: Mascotas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mascota = await _context.Mascota
                .Include(m => m.IdClienteNavigation)
                .Include(m => m.IdEspecieNavigation)
                .Include(m => m.IdRazaNavigation)
                .FirstOrDefaultAsync(m => m.IdMascota == id);
            if (mascota == null)
            {
                return NotFound();
            }

            return View(mascota);
        }

        // POST: Mascotas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mascota = await _context.Mascota.FindAsync(id);
            if (mascota != null)
            {
                try
                {
                    _context.Mascota.Remove(mascota);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "No se puede eliminar la mascota porque tiene registros asociados (atenciones, vacunas, etc.).");
                    return View(mascota);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MascotaExists(int id)
        {
            return _context.Mascota.Any(e => e.IdMascota == id);
        }
    }
}
