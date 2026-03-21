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
    public class ClientesController : Controller
    {
        private readonly BDVeterinariaContext _context;

        public ClientesController(BDVeterinariaContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clientes.ToListAsync());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.IdCliente == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCliente,NombreCompleto,TipoIdentificacion,NumIdentificacion,Email,TelefonoPrincipal,TelefonoAlternativo,Direccion,Ciudad,ContactoEmergencia,TelefonoEmergencia,Observaciones,Activo,CreatedAt,CreatedBy,UpdatedAt,UpdatedBy")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(cliente);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException?.Message.Contains("UK_Cliente_Identificacion") == true)
                    {
                        ModelState.AddModelError("", "Ya existe un cliente con el mismo tipo y número de identificación.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Ocurrió un error al guardar el cliente. Intente nuevamente.");
                    }
                    throw;
                }
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCliente,NombreCompleto,TipoIdentificacion,NumIdentificacion,Email,TelefonoPrincipal,TelefonoAlternativo,Direccion,Ciudad,ContactoEmergencia,TelefonoEmergencia,Observaciones,Activo,CreatedAt,CreatedBy,UpdatedAt,UpdatedBy")] Cliente cliente)
        {
            if (id != cliente.IdCliente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.IdCliente))
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
                    if (ex.InnerException?.Message.Contains("UK_Cliente_Identificacion") == true)
                    {
                        ModelState.AddModelError("", "Ya existe un cliente con el mismo tipo y número de identificación.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Ocurrió un error al actualizar el cliente.");
                    }
                }
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.IdCliente == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                try
                {
                    _context.Clientes.Remove(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "No se puede eliminar el cliente porque tiene registros asociados (mascotas, ventas, etc.).");
                    return View(cliente);
                }
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.IdCliente == id);
        }
    }
}
