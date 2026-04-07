using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Veterinaria.Application.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Entities;
using Veterinaria.Web.ViewModels.Usuarios;

namespace Veterinaria.Web.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IApplicationDbContext _context;

        public UsuariosController(IApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuarios
                .OrderBy(u => u.NombreCompleto)
                .ToListAsync();

            return View(usuarios);
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.IdUsuario == id);

            if (usuario == null) return NotFound();

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View(new UsuarioCreateViewModel());
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioCreateViewModel usercreatevm)
        {
            if (!ModelState.IsValid)
                return View(usercreatevm);

            var usuario = new Usuario
            {
                Email = usercreatevm.Email!.Trim().ToLower(),
                NombreCompleto = usercreatevm.NombreCompleto!.Trim(),
                Rol = usercreatevm.Rol!,
                Especialidad = usercreatevm.Especialidad?.Trim(),
                NumeroColegiado = usercreatevm.NumeroColegiado?.Trim(),
                Telefono = usercreatevm.Telefono?.Trim(),
                Activo = true,
                CreatedAt = DateTime.Now,

                // El Controller hace el hash — la vista nunca toca esto
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(usercreatevm.Password)
            };

            try
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UK_Usuario_Email") == true)
                {
                    ModelState.AddModelError(nameof(usercreatevm.Email),
                        "Ya existe un usuario con ese email.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Ocurrió un error al guardar. Intente nuevamente.");
                }

                return View(usercreatevm);
            }
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            // Usamos UsuarioViewModel base — sin password
            var v_uservm = new UsuarioViewModel
            {
                IdUsuario = usuario.IdUsuario,
                Email = usuario.Email,
                NombreCompleto = usuario.NombreCompleto,
                Rol = usuario.Rol,
                Especialidad = usuario.Especialidad,
                NumeroColegiado = usuario.NumeroColegiado,
                Telefono = usuario.Telefono,
                Activo = usuario.Activo
            };

            return View(v_uservm);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UsuarioViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var usuario = await _context.Usuarios.FindAsync(vm.IdUsuario);
            if (usuario == null) return NotFound();

            // Actualizamos solo los campos del ViewModel
            // PasswordHash NO se toca — queda exactamente como estaba
            usuario.Email = vm.Email!.Trim().ToLower();
            usuario.NombreCompleto = vm.NombreCompleto!.Trim();
            usuario.Rol = vm.Rol!;
            usuario.Especialidad = vm.Especialidad?.Trim();
            usuario.NumeroColegiado = vm.NumeroColegiado?.Trim();
            usuario.Telefono = vm.Telefono?.Trim();
            usuario.Activo = vm.Activo;
            usuario.UpdatedAt = DateTime.Now;

            try
            {
                _context.Update(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(vm.IdUsuario)) return NotFound();
                else throw;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UK_Usuario_Email") == true)
                {
                    ModelState.AddModelError(nameof(vm.Email),
                        "Ya existe un usuario con ese email.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Ocurrió un error al actualizar.");
                }

                return View(vm);
            }
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null) return NotFound();

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.IdUsuario == id);

            if (usuario == null) return NotFound();

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            try
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty,
                    "No se puede eliminar este usuario porque tiene registros asociados.");
                return View(usuario);
            }
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.IdUsuario == id);
        }
    }
}
