using Microsoft.AspNetCore.Mvc;
using Veterinaria.Application.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using ZXing;
using ZXing.SkiaSharp;
using Veterinaria.Web.ViewModels.Productos;
using Veterinaria.Domain.Entities;

namespace Veterinaria.Web.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IApplicationDbContext _context;

        public ProductosController(IApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<IEnumerable<SelectListItem>> GetCategoriasSelectAsync(int? selectedId = null)
        {
            var categorias = await _context.CategoriaProductos
                .Where(c => c.Activo)
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            return categorias.Select(c => new SelectListItem
            {
                Value = c.IdCategoria.ToString(),
                Text = c.Nombre,
                Selected = c.IdCategoria == selectedId
            });
        }

        // Método auxiliar que llena todas las listas del ViewModel
        private async Task LlenarListasViewModel(ProductoViewModel productovm)
        {
            productovm.CategoriasDisponibles = await GetCategoriasSelectAsync(productovm.IdCategoria);
        }

        // GET: Productos
        public async Task<IActionResult> Index()
        {
            var productos = await _context.Productos
                .Include(p => p.IdCategoriaNavigation)
                .Where(p => p.Activo)
                .OrderBy(p => p.Nombre)
                .ToListAsync();

            return View(productos);
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var producto = await _context.Productos
                .Include(p => p.IdCategoriaNavigation)
                .FirstOrDefaultAsync(m => m.IdProducto == id);

            if (producto == null) return NotFound();

            return View(producto);
        }

        // GET: Productos/Create
        public async Task<IActionResult> Create()
        {
            var v_productovm = new ProductoViewModel();
            await LlenarListasViewModel(v_productovm);
            return View(v_productovm);
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductoViewModel productovm)
        {
            if (!ModelState.IsValid)
            {
                await LlenarListasViewModel(productovm);
                return View(productovm);
            }

            var producto = new Producto
            {
                IdCategoria = productovm.IdCategoria!.Value,
                CodigoInterno = productovm.CodigoInterno!.Trim().ToUpper(),
                Nombre = productovm.Nombre!.Trim(),
                Descripcion = productovm.Descripcion?.Trim(),
                TipoProducto = productovm.TipoProducto!,
                PrecioVenta = productovm.PrecioVenta,
                PrecioCosto = productovm.PrecioCosto,
                UnidadMedida = productovm.UnidadMedida!,
                RequiereReceta = productovm.RequiereReceta,
                Activo = true,
                CreatedAt = DateTime.Now
            };

            try
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                // Dos constraints independientes — if / else if / else
                if (ex.InnerException?.Message.Contains("UK_Producto_CodigoBarras") == true)
                {
                    ModelState.AddModelError(nameof(productovm.CodigoBarras),
                        "El código de barras ya está registrado en otro producto.");
                }
                else if (ex.InnerException?.Message.Contains("UK_Producto_CodigoInterno") == true)
                {
                    ModelState.AddModelError(nameof(productovm.CodigoInterno),
                        "El código interno ya está registrado en otro producto.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Ocurrió un error al guardar. Intente nuevamente.");
                }

                await LlenarListasViewModel(productovm);
                return View(productovm);
            }
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            var vm = new ProductoViewModel
            {
                IdProducto = producto.IdProducto,
                IdCategoria = producto.IdCategoria,
                CodigoBarras = producto.CodigoBarras,
                CodigoInterno = producto.CodigoInterno,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                TipoProducto = producto.TipoProducto,
                PrecioVenta = producto.PrecioVenta,
                PrecioCosto = producto.PrecioCosto,
                UnidadMedida = producto.UnidadMedida,
                RequiereReceta = producto.RequiereReceta,
                Activo = producto.Activo
                ,CreatedAt = producto.CreatedAt
                ,CreatedBy = producto.CreatedBy
                ,UpdatedAt = producto.UpdatedAt
                ,UpdatedBy = producto.UpdatedBy
            };

            await LlenarListasViewModel(vm);
            return View(vm);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductoViewModel productovm)
        {
            if (!ModelState.IsValid)
            {
                await LlenarListasViewModel(productovm);
                return View(productovm);
            }

            var producto = await _context.Productos.FindAsync(productovm.IdProducto);
            if (producto == null) return NotFound();

            producto.IdCategoria = productovm.IdCategoria!.Value;
            producto.CodigoInterno = productovm.CodigoInterno!.Trim().ToUpper();
            producto.Nombre = productovm.Nombre!.Trim();
            producto.Descripcion = productovm.Descripcion?.Trim();
            producto.TipoProducto = productovm.TipoProducto!;
            producto.PrecioVenta = productovm.PrecioVenta;
            producto.PrecioCosto = productovm.PrecioCosto;
            producto.UnidadMedida = productovm.UnidadMedida!;
            producto.RequiereReceta = productovm.RequiereReceta;
            producto.Activo = productovm.Activo;
            producto.UpdatedAt = DateTime.Now;

            try
            {
                _context.Update(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductoExists(productovm.IdProducto)) return NotFound();
                else throw;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UK_Producto_CodigoBarras") == true)
                {
                    ModelState.AddModelError(nameof(productovm.CodigoBarras),
                        "El código de barras ya está registrado en otro producto.");
                }
                else if (ex.InnerException?.Message.Contains("UK_Producto_CodigoInterno") == true)
                {
                    ModelState.AddModelError(nameof(productovm.CodigoInterno),
                        "El código interno ya está registrado en otro producto.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Ocurrió un error al actualizar.");
                }

                await LlenarListasViewModel(productovm);
                return View(productovm);
            }
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var producto = await _context.Productos
                .Include(p => p.IdCategoriaNavigation)
                .FirstOrDefaultAsync(m => m.IdProducto == id);

            if (producto == null) return NotFound();

            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            try
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty,
                    "No se puede eliminar este producto porque tiene ventas o atenciones asociadas.");
                return View(producto);
            }
        }

        private async Task<bool> ProductoExists(int id)
        {
            return await _context.Productos.AnyAsync(e => e.IdProducto == id);
        }

        [HttpGet]
        public IActionResult GenerarImagenCodigoBarras(string codigoAlfanumerico)
        {
            if (string.IsNullOrWhiteSpace(codigoAlfanumerico))
            {
                return BadRequest("El código no puede estar vacío.");
            }

            // 1. Configuramos el tipo de código (CODE_128 es el estándar para textos alfanuméricos)
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 300,
                    Height = 100,
                    Margin = 10,
                    PureBarcode = false // True si NO quieres que el texto aparezca debajo de las barras
                }
            };

            // 2. Generamos la imagen usando SkiaSharp
            using var bitmap = writer.Write(codigoAlfanumerico);
            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);

            // 3. Lo convertimos a un flujo de bytes para enviarlo al navegador web
            using var stream = new MemoryStream();
            data.SaveTo(stream);

            return File(stream.ToArray(), "image/png");
        }
    }
}
