using Microsoft.Extensions.DependencyInjection;
using Veterinaria.Application.Clientes;

namespace Veterinaria.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IClienteService, ClienteService>();
        return services;
    }
}
