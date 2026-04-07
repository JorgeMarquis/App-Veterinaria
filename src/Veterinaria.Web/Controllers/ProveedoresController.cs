using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Veterinaria.Application.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Entities;
using Veterinaria.Web.ViewModels.Proveedores;

namespace Veterinaria.Web.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly IApplicationDbContext _context;

        public ProveedoresController(IApplicationDbContext context)
        {
            _context = context;
        }

        private static readonly List<string> TiposIdentificacion = new()
        {
            "RUC",
            "DNI",
            "Carnet de Extranjería",
            "Pasaporte"
        };

        // GET: Proveedores
        public async Task<IActionResult> Index()
        {
            var proveedores = await _context.Proveedors
                .OrderBy(p => p.RazonSocial)
                .ToListAsync();

            return View(proveedores);
        }

        // GET: Proveedores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var proveedor = await _context.Proveedors
                .FirstOrDefaultAsync(m => m.IdProveedor == id);

            if (proveedor == null) return NotFound();

            return View(proveedor);
        }

        // GET: Proveedores/Create
        public IActionResult Create()
        {
            ViewBag.TiposIdentificacion = new SelectList(TiposIdentificacion);
            return View(new ProveedorViewModel());
        }

        // POST: Proveedores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProveedorViewModel proveedorvm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TiposIdentificacion = TiposIdentificacion;
                return View(proveedorvm);
            }

            var proveedor = new Proveedor
            {
                TipoIdentificacion = proveedorvm.TipoIdentificacion!,
                NumIdentificacion = proveedorvm.NumIdentificacion!.Trim(),
                RazonSocial = proveedorvm.RazonSocial!.Trim(),
                NombreContacto = proveedorvm.NombreContacto?.Trim(),
                Email = proveedorvm.Email?.Trim().ToLower(),
                TelefonoPrincipal = proveedorvm.TelefonoPrincipal?.Trim(),
                Direccion = proveedorvm.Direccion?.Trim(),
                Distrito = proveedorvm.Distrito?.Trim(),
                GiroComercial = proveedorvm.GiroComercial?.Trim(),
                Observaciones = proveedorvm.Observaciones?.Trim(),
                Activo = true,
                CreatedAt = DateTime.Now
            };

            try
            {
                _context.Add(proveedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UK_Proveedor_Identificacion") == true)
                {
                    ModelState.AddModelError(string.Empty,
                        "Ya existe un proveedor con ese tipo y número de identificación.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Ocurrió un error al guardar. Intente nuevamente.");
                }

                ViewBag.TiposIdentificacion = TiposIdentificacion;
                return View(proveedorvm);
            }

        }

        // GET: Proveedores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var proveedor = await _context.Proveedors.FindAsync(id);
            if (proveedor == null) return NotFound();

            var v_proveedorvm = new ProveedorViewModel
            {
                IdProveedor = proveedor.IdProveedor,
                TipoIdentificacion = proveedor.TipoIdentificacion,
                NumIdentificacion = proveedor.NumIdentificacion,
                RazonSocial = proveedor.RazonSocial,
                NombreContacto = proveedor.NombreContacto,
                Email = proveedor.Email,
                TelefonoPrincipal = proveedor.TelefonoPrincipal,
                Direccion = proveedor.Direccion,
                Distrito = proveedor.Distrito,
                GiroComercial = proveedor.GiroComercial,
                Observaciones = proveedor.Observaciones,
                Activo = proveedor.Activo
            };

            ViewBag.TiposIdentificacion = TiposIdentificacion;
            return View(v_proveedorvm);
        }

        // POST: Proveedores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProveedorViewModel proveedorvm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TiposIdentificacion = TiposIdentificacion;
                return View(proveedorvm);
            }

            var proveedor = await _context.Proveedors.FindAsync(proveedorvm.IdProveedor);
            if (proveedor == null) return NotFound();

            proveedor.TipoIdentificacion = proveedorvm.TipoIdentificacion!;
            proveedor.NumIdentificacion = proveedorvm.NumIdentificacion!.Trim();
            proveedor.RazonSocial = proveedorvm.RazonSocial!.Trim();
            proveedor.NombreContacto = proveedorvm.NombreContacto?.Trim();
            proveedor.Email = proveedorvm.Email?.Trim().ToLower();
            proveedor.TelefonoPrincipal = proveedorvm.TelefonoPrincipal?.Trim();
            proveedor.Direccion = proveedorvm.Direccion?.Trim();
            proveedor.Distrito = proveedorvm.Distrito?.Trim();
            proveedor.GiroComercial = proveedorvm.GiroComercial?.Trim();
            proveedor.Observaciones = proveedorvm.Observaciones?.Trim();
            proveedor.Activo = proveedorvm.Activo;
            proveedor.UpdatedAt = DateTime.Now;

            try
            {
                _context.Update(proveedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProveedorExists(proveedorvm.IdProveedor)) return NotFound();
                else throw;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UK_Proveedor_Identificacion") == true)
                {
                    ModelState.AddModelError(string.Empty,
                        "Ya existe un proveedor con ese tipo y número de identificación.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Ocurrió un error al actualizar.");
                }

                ViewBag.TiposIdentificacion = TiposIdentificacion;
                return View(proveedorvm);
            }
        }

        // GET: Proveedores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var proveedor = await _context.Proveedors
                .FirstOrDefaultAsync(m => m.IdProveedor == id);

            if (proveedor == null) return NotFound();

            return View(proveedor);

        }

        // POST: Proveedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proveedor = await _context.Proveedors.FindAsync(id);
            if (proveedor == null) return NotFound();

            try
            {
                _context.Proveedors.Remove(proveedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty,
                    "No se puede eliminar este proveedor porque tiene compras asociadas.");
                return View(proveedor);
            }
        }

        private bool ProveedorExists(int id)
        {
            return _context.Proveedors.Any(e => e.IdProveedor == id);
        }
    }
}
