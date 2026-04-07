using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Veterinaria.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Entities;
using Veterinaria.Web.ViewModels.Dashboard;

namespace Veterinaria.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IApplicationDbContext _context;

        public DashboardController(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new DashboardViewModel
            {
                TotalClientes = await _context.Clientes.CountAsync(),
                TotalMascotas = await _context.Mascota.CountAsync(),
                TotalProductos = await _context.Productos.CountAsync(),
                TotalUsuariosActivos = await _context.Usuarios.CountAsync(u => u.Activo),
                TotalVacunas = await _context.Vacunas.CountAsync()
            };

            return View(vm);
        }
    }
}