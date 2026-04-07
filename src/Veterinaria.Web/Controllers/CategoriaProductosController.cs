using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Veterinaria.Application.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Entities;
using Veterinaria.Web.ViewModels.CategoriaProductos;

namespace Veterinaria.Web.Controllers
{
    public class CategoriaProductosController : Controller
    {
        private readonly IApplicationDbContext _context;

        public CategoriaProductosController(IApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CategoriaProductoes
        public async Task<IActionResult> Index()
        {
            var categorias = await _context.CategoriaProductos
                .OrderBy(c => c.Nombre)
                .Select(c => new CategoriaProductoViewModel
                {
                    IdCategoria = c.IdCategoria,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion,
                    EsMedicamento = c.EsMedicamento,
                    RequiereReceta = c.RequiereReceta,
                    Activo = c.Activo
                })
                .ToListAsync();
            return View(categorias);
        }

        // GET: CategoriaProductoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var categoriaProducto = await _context.CategoriaProductos
                .FirstOrDefaultAsync(m => m.IdCategoria == id);

            if (categoriaProducto == null) return NotFound();

            return View(categoriaProducto);
        }

        // GET: CategoriaProductoes/Create
        public IActionResult Create()
        {
            return View(new CategoriaProductoViewModel());
        }

        // POST: CategoriaProductoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoriaProductoViewModel catprodVM)
        {
            if (!ModelState.IsValid) return View(catprodVM);

            var categoria = new CategoriaProducto
            {
                Nombre = catprodVM.Nombre!.Trim(),
                Descripcion = catprodVM.Descripcion?.Trim(),
                EsMedicamento = catprodVM.EsMedicamento,
                RequiereReceta = catprodVM.RequiereReceta,
                Activo = true,
                FechaCreacion = DateTime.Now
            };

            try
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UK_CategoriaProducto_Nombre") == true)
                {
                    ModelState.AddModelError(nameof(catprodVM.Nombre),
                        "Ya existe una categoría con ese nombre.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Ocurrió un error al guardar. Intente nuevamente.");
                }

                return View("CreateEdit", catprodVM);
            }
        }

        // GET: CategoriaProductoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.CategoriaProductos.FindAsync(id);
            if (categoria == null) return NotFound();

            var viewmodel = new CategoriaProductoViewModel
            {
                IdCategoria = categoria.IdCategoria,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                EsMedicamento = categoria.EsMedicamento,
                RequiereReceta = categoria.RequiereReceta,
                Activo = categoria.Activo
            };

            return View("CreateEdit", viewmodel);
        }

        // POST: CategoriaProductoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoriaProductoViewModel cpViewModel)
        {
            if (!ModelState.IsValid) return View("CreateEdit", cpViewModel);

            var categoria = await _context.CategoriaProductos.FindAsync(cpViewModel.IdCategoria);
            if (categoria == null) return NotFound();

            categoria.Nombre = cpViewModel.Nombre!.Trim();
            categoria.Descripcion = cpViewModel.Descripcion?.Trim();
            categoria.EsMedicamento = cpViewModel.EsMedicamento;
            categoria.RequiereReceta = cpViewModel.RequiereReceta;
            categoria.Activo = cpViewModel.Activo;
            categoria.FechaModificacion = DateTime.Now;

            try
            {
                _context.Update(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CategoriaProductoExists(cpViewModel.IdCategoria)) return NotFound();
                else throw;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UK_CategoriaProducto_Nombre") == true)
                {
                    ModelState.AddModelError(nameof(cpViewModel.Nombre),
                        "Ya existe una categoría con ese nombre.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Ocurrió un error al actualizar.");
                }

                return View("CreateEdit", cpViewModel);
            }
        }

        // GET: CategoriaProductoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            
            var categoriaProducto = await _context.CategoriaProductos
                .FirstOrDefaultAsync(m => m.IdCategoria == id);

            if (categoriaProducto == null) return NotFound();

            return View(categoriaProducto);
        }

        // POST: CategoriaProductoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.CategoriaProductos.FindAsync(id);
            if (categoria == null) return NotFound();

            try
            {
                _context.CategoriaProductos.Remove(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty,
                    "No se puede eliminar esta categoría porque tiene productos asociados.");
                return View(categoria);
            }
        }

        private async Task<bool> CategoriaProductoExists(int id)
        {
            return await _context.CategoriaProductos.AnyAsync(e => e.IdCategoria == id);
        }
    }
}
