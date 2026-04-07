using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Veterinaria.Application.Abstractions;
using Veterinaria.Infrastructure.Persistence;

namespace Veterinaria.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("CadenaVeterinaria");
        services.AddDbContext<BDVeterinariaContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IApplicationDbContext>(sp =>
            sp.GetRequiredService<BDVeterinariaContext>());

        return services;
    }
}
