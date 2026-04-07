using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Veterinaria.Application.Clientes;
using Veterinaria.Domain.Entities;
using Veterinaria.Web.ViewModels.Clientes;

namespace Veterinaria.Web.Controllers;

public class ClientesController : Controller
{
    private readonly IClienteService _clientes;

    public ClientesController(IClienteService clientes)
    {
        _clientes = clientes;
    }

    public async Task<IActionResult> Index()
    {
        var clientes = await _clientes.ListarActivosAsync();
        return View(clientes);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();

        var cliente = await _clientes.ObtenerPorIdAsync(id.Value);
        if (cliente is null) return NotFound();

        return View(cliente);
    }

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

        var input = ToCreateInput(clientevm);
        var result = await _clientes.CrearAsync(input);

        if (result.Success)
            return RedirectToAction(nameof(Index));

        MapErrorToModelState(result);
        ViewBag.TiposIdentificacion = ClienteViewModel.TiposIdentificacion;
        return View("Createedit", clientevm);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();

        var cliente = await _clientes.ObtenerParaEdicionAsync(id.Value);
        if (cliente is null) return NotFound();

        var vm = ToViewModel(cliente);
        ViewBag.TiposIdentificacion = ClienteViewModel.TiposIdentificacion;
        return View("Createedit", vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ClienteViewModel clientevm)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.TiposIdentificacion = ClienteViewModel.TiposIdentificacion;
            return View("Createedit", clientevm);
        }

        var input = ToUpdateInput(clientevm);
        try
        {
            var result = await _clientes.ActualizarAsync(input);
            if (result.Success)
                return RedirectToAction(nameof(Index));

            if (result.Error == ClienteErrorKind.NotFound)
                return NotFound();

            MapErrorToModelState(result);
            ViewBag.TiposIdentificacion = ClienteViewModel.TiposIdentificacion;
            return View("Createedit", clientevm);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _clientes.ExisteAsync(clientevm.IdCliente)) return NotFound();
            throw;
        }
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();

        var cliente = await _clientes.ObtenerParaEliminacionAsync(id.Value);
        if (cliente is null) return NotFound();

        return View(cliente);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var cliente = await _clientes.ObtenerParaEliminacionAsync(id);
        if (cliente is null) return NotFound();

        var result = await _clientes.EliminarAsync(id);

        if (result.Success)
            return RedirectToAction(nameof(Index));

        ModelState.Clear();
        ModelState.AddModelError(string.Empty,
            "No se puede eliminar este cliente porque tiene mascotas o ventas asociadas.");
        return View(cliente);
    }

    private static ClienteCreateInput ToCreateInput(ClienteViewModel vm) =>
        new(
            vm.TipoIdentificacion!,
            vm.NumIdentificacion!,
            vm.NombreCompleto!,
            vm.TelefonoPrincipal!,
            vm.Direccion!,
            vm.Ciudad!,
            vm.Email,
            vm.TelefonoAlternativo,
            vm.ContactoEmergencia,
            vm.TelefonoEmergencia,
            vm.Observaciones);

    private static ClienteUpdateInput ToUpdateInput(ClienteViewModel vm) =>
        new(
            vm.IdCliente,
            vm.TipoIdentificacion!,
            vm.NumIdentificacion!,
            vm.NombreCompleto!,
            vm.TelefonoPrincipal!,
            vm.Direccion!,
            vm.Ciudad!,
            vm.Email,
            vm.TelefonoAlternativo,
            vm.ContactoEmergencia,
            vm.TelefonoEmergencia,
            vm.Observaciones,
            vm.Activo);

    private static ClienteViewModel ToViewModel(Cliente c) =>
        new()
        {
            IdCliente = c.IdCliente,
            TipoIdentificacion = c.TipoIdentificacion,
            NumIdentificacion = c.NumIdentificacion,
            NombreCompleto = c.NombreCompleto,
            TelefonoPrincipal = c.TelefonoPrincipal,
            Direccion = c.Direccion,
            Ciudad = c.Ciudad,
            Email = c.Email,
            TelefonoAlternativo = c.TelefonoAlternativo,
            ContactoEmergencia = c.ContactoEmergencia,
            TelefonoEmergencia = c.TelefonoEmergencia,
            Observaciones = c.Observaciones,
            Activo = c.Activo,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        };

    private void MapErrorToModelState(ClienteOperationResult result)
    {
        if (result.Error == ClienteErrorKind.DuplicateIdentificacion)
        {
            ModelState.AddModelError(string.Empty,
                "Ya existe un cliente con ese tipo y número de identificación.");
        }
        else if (result.Error == ClienteErrorKind.GenericUpdate)
        {
            ModelState.AddModelError(string.Empty,
                "Ocurrió un error al actualizar.");
        }
        else
        {
            ModelState.AddModelError(string.Empty,
                "Ocurrió un error al guardar. Intente nuevamente.");
        }
    }
}
