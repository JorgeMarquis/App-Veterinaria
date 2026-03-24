using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRJ_VETERINARIA.Models;
using PRJ_VETERINARIA.ViewModels.FormaPagos;

namespace PRJ_VETERINARIA.Controllers
{
    public class FormaPagosController : Controller
    {
        private readonly BDVeterinariaContext _context;

        public FormaPagosController(BDVeterinariaContext context)
        {
            _context = context;
        }

        // GET: FormaPagoes
        public async Task<IActionResult> Index()
        {
            var formasPago = await _context.FormaPagos
                .OrderBy(f => f.Nombre)
                .ToListAsync();

            return View(formasPago);
        }

        // GET: FormaPagoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var formaPago = await _context.FormaPagos
                .FirstOrDefaultAsync(m => m.IdFormaPago == id);

            if (formaPago == null) return NotFound();

            return View(formaPago);
        }

        // GET: FormaPagoes/Create
        public IActionResult Create()
        {
            return View(new FormaPagoViewModel());
        }

        // POST: FormaPagoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FormaPagoViewModel formapagovm)
        {
            if (!ModelState.IsValid)
                return View(formapagovm);

            var formaPago = new FormaPago
            {
                Nombre = formapagovm.Nombre!.Trim(),
                ComisionPorcentaje = formapagovm.ComisionPorcentaje,
                RequiereAutorizacion = formapagovm.RequiereAutorizacion,
                Activo = true,
                FechaCreacion = DateTime.Now
            };

            try
            {
                _context.Add(formaPago);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UK_FormaPago_Nombre") == true)
                {
                    ModelState.AddModelError(nameof(formapagovm.Nombre),
                        "Ya existe una forma de pago con ese nombre.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Ocurrió un error al guardar. Intente nuevamente.");
                }

                return View(formapagovm);
            }
        }

        // GET: FormaPagoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var formaPago = await _context.FormaPagos.FindAsync(id);
            if (formaPago == null) return NotFound();

            var v_formapagovm = new FormaPagoViewModel
            {
                IdFormaPago = formaPago.IdFormaPago,
                Nombre = formaPago.Nombre,
                ComisionPorcentaje = formaPago.ComisionPorcentaje,
                RequiereAutorizacion = formaPago.RequiereAutorizacion,
                Activo = formaPago.Activo
            };

            return View(v_formapagovm);
        }

        // POST: FormaPagoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FormaPagoViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var formaPago = await _context.FormaPagos.FindAsync(vm.IdFormaPago);
            if (formaPago == null) return NotFound();

            formaPago.Nombre = vm.Nombre!.Trim();
            formaPago.ComisionPorcentaje = vm.ComisionPorcentaje;
            formaPago.RequiereAutorizacion = vm.RequiereAutorizacion;
            formaPago.Activo = vm.Activo;
            formaPago.FechaModificacion = DateTime.Now;

            try
            {
                _context.Update(formaPago);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormaPagoExists(vm.IdFormaPago)) return NotFound();
                else throw;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UK_FormaPago_Nombre") == true)
                {
                    ModelState.AddModelError(nameof(vm.Nombre),
                        "Ya existe una forma de pago con ese nombre.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Ocurrió un error al actualizar.");
                }

                return View(vm);
            }
        }

        // GET: FormaPagoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var formaPago = await _context.FormaPagos
                .FirstOrDefaultAsync(m => m.IdFormaPago == id);

            if (formaPago == null) return NotFound();

            return View(formaPago);
        }

        // POST: FormaPagoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var formaPago = await _context.FormaPagos.FindAsync(id);
            if (formaPago == null) return NotFound();

            try
            {
                _context.FormaPagos.Remove(formaPago);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty,
                    "No se puede eliminar esta forma de pago porque tiene ventas o compras asociadas.");
                return View(formaPago);
            }
        }

        private bool FormaPagoExists(int id)
        {
            return _context.FormaPagos.Any(e => e.IdFormaPago == id);
        }
    }
}
