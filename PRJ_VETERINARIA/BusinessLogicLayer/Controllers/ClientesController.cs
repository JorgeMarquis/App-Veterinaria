using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRJ_VETERINARIA.DataAccessLayer.Models;
using PRJ_VETERINARIA.DataAccessLayer.ViewModels.Clientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRJ_VETERINARIA.BusinessLogicLayer.Controllers
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
            var clientes = await _context.Clientes
                .Where(c => c.Activo)
                .OrderBy(c => c.NombreCompleto)
                .ToListAsync();

            return View(clientes);
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.IdCliente == id);

            if (cliente == null) return NotFound();

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            ViewBag.TiposIdentificacion = ClienteViewModel.TiposIdentificacion;
            return View("Createedit", new ClienteViewModel());
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClienteViewModel clientevm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TiposIdentificacion = ClienteViewModel.TiposIdentificacion;
                return View("Createedit", clientevm);
            }

            var cliente = new Cliente
            {
                TipoIdentificacion = clientevm.TipoIdentificacion!,
                NumIdentificacion = clientevm.NumIdentificacion!.Trim(),
                NombreCompleto = clientevm.NombreCompleto!.Trim(),
                TelefonoPrincipal = clientevm.TelefonoPrincipal!.Trim(),
                Direccion = clientevm.Direccion!.Trim(),
                Ciudad = clientevm.Ciudad!.Trim(),
                Email = clientevm.Email?.Trim().ToLower(),
                TelefonoAlternativo = clientevm.TelefonoAlternativo?.Trim(),
                ContactoEmergencia = clientevm.ContactoEmergencia?.Trim(),
                TelefonoEmergencia = clientevm.TelefonoEmergencia?.Trim(),
                Observaciones = clientevm.Observaciones?.Trim(),
                Activo = true,
                CreatedAt = DateTime.Now
            };

            try
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                // Constraint compuesta — error pertenece a los dos campos juntos
                if (ex.InnerException?.Message.Contains("UK_Cliente_Identificacion") == true)
                {
                    ModelState.AddModelError(string.Empty,
                        "Ya existe un cliente con ese tipo y número de identificación.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Ocurrió un error al guardar. Intente nuevamente.");
                }

                ViewBag.TiposIdentificacion = ClienteViewModel.TiposIdentificacion;
                return View("Createedit", clientevm);
            }
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();

            var vm = new ClienteViewModel
            {
                IdCliente = cliente.IdCliente,
                TipoIdentificacion = cliente.TipoIdentificacion,
                NumIdentificacion = cliente.NumIdentificacion,
                NombreCompleto = cliente.NombreCompleto,
                TelefonoPrincipal = cliente.TelefonoPrincipal,
                Direccion = cliente.Direccion,
                Ciudad = cliente.Ciudad,
                Email = cliente.Email,
                TelefonoAlternativo = cliente.TelefonoAlternativo,
                ContactoEmergencia = cliente.ContactoEmergencia,
                TelefonoEmergencia = cliente.TelefonoEmergencia,
                Observaciones = cliente.Observaciones,
                Activo = cliente.Activo
                ,CreatedAt = cliente.CreatedAt
                ,UpdatedAt = cliente.UpdatedAt
            };

            ViewBag.TiposIdentificacion = ClienteViewModel.TiposIdentificacion;
            return View("Createedit", vm);
        }

        // POST: Clientes/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClienteViewModel clientevm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TiposIdentificacion = ClienteViewModel.TiposIdentificacion;
                return View("Createedit", clientevm);
            }

            var cliente = await _context.Clientes.FindAsync(clientevm.IdCliente);
            if (cliente == null) return NotFound();

            cliente.TipoIdentificacion = clientevm.TipoIdentificacion!;
            cliente.NumIdentificacion = clientevm.NumIdentificacion!.Trim();
            cliente.NombreCompleto = clientevm.NombreCompleto!.Trim();
            cliente.TelefonoPrincipal = clientevm.TelefonoPrincipal!.Trim();
            cliente.Direccion = clientevm.Direccion!.Trim();
            cliente.Ciudad = clientevm.Ciudad!.Trim();
            cliente.Email = clientevm.Email?.Trim().ToLower();
            cliente.TelefonoAlternativo = clientevm.TelefonoAlternativo?.Trim();
            cliente.ContactoEmergencia = clientevm.ContactoEmergencia?.Trim();
            cliente.TelefonoEmergencia = clientevm.TelefonoEmergencia?.Trim();
            cliente.Observaciones = clientevm.Observaciones?.Trim();
            cliente.Activo = clientevm.Activo;
            cliente.UpdatedAt = DateTime.Now;

            try
            {
                _context.Update(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ClienteExists(clientevm.IdCliente)) return NotFound();
                else throw;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UK_Cliente_Identificacion") == true)
                {
                    ModelState.AddModelError(string.Empty,
                        "Ya existe un cliente con ese tipo y número de identificación.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Ocurrió un error al actualizar.");
                }

                ViewBag.TiposIdentificacion = ClienteViewModel.TiposIdentificacion;
                return View("Createedit", clientevm);
            }
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.IdCliente == id);

            if (cliente == null) return NotFound();

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();

            try
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty,
                    "No se puede eliminar este cliente porque tiene mascotas o ventas asociadas.");
                return View(cliente);
            }
        }

        private async Task<bool> ClienteExists(int id)
        {
            return await _context.Clientes.AnyAsync(e => e.IdCliente == id);
        }
    }
}
