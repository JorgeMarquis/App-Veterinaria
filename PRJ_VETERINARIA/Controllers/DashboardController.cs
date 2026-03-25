using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRJ_VETERINARIA.Models;
using PRJ_VETERINARIA.ViewModels.Dashboard;

namespace PRJ_VETERINARIA.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly BDVeterinariaContext _context;

        public DashboardController(BDVeterinariaContext context)
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