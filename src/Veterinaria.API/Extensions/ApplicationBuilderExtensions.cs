using Veterinaria.API.Middleware;

namespace Veterinaria.API.Extensions;

/// <summary>
/// Extensiones para la configuración de la aplicación
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Configura los middleware de la API
    /// </summary>
    public static IApplicationBuilder UseApiMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        if (app.ApplicationServices.GetRequiredService<IHostEnvironment>().IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors("AllowAll");
        app.UseAuthorization();

        return app;
    }
}
