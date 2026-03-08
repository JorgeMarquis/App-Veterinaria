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
    public class FormaPagoesController : Controller
    {
        private readonly BDVeterinariaContext _context;

        public FormaPagoesController(BDVeterinariaContext context)
        {
            _context = context;
        }

        // GET: FormaPagoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.FormaPagos.ToListAsync());
        }

        // GET: FormaPagoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formaPago = await _context.FormaPagos
                .FirstOrDefaultAsync(m => m.IdFormaPago == id);
            if (formaPago == null)
            {
                return NotFound();
            }

            return View(formaPago);
        }

        // GET: FormaPagoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FormaPagoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFormaPago,Nombre,ComisionPorcentaje,RequiereAutorizacion,Activo,FechaCreacion,FechaModificacion")] FormaPago formaPago)
        {
            if (ModelState.IsValid)
            {
                _context.Add(formaPago);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(formaPago);
        }

        // GET: FormaPagoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formaPago = await _context.FormaPagos.FindAsync(id);
            if (formaPago == null)
            {
                return NotFound();
            }
            return View(formaPago);
        }

        // POST: FormaPagoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdFormaPago,Nombre,ComisionPorcentaje,RequiereAutorizacion,Activo,FechaCreacion,FechaModificacion")] FormaPago formaPago)
        {
            if (id != formaPago.IdFormaPago)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(formaPago);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FormaPagoExists(formaPago.IdFormaPago))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(formaPago);
        }

        // GET: FormaPagoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formaPago = await _context.FormaPagos
                .FirstOrDefaultAsync(m => m.IdFormaPago == id);
            if (formaPago == null)
            {
                return NotFound();
            }

            return View(formaPago);
        }

        // POST: FormaPagoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var formaPago = await _context.FormaPagos.FindAsync(id);
            if (formaPago != null)
            {
                _context.FormaPagos.Remove(formaPago);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FormaPagoExists(int id)
        {
            return _context.FormaPagos.Any(e => e.IdFormaPago == id);
        }
    }
}
