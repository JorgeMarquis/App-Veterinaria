namespace Veterinaria.Shared.Exceptions;

/// <summary>
/// Excepción lanzada cuando un recurso no es encontrado
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string resourceName, object id) 
        : base($"{resourceName} con ID '{id}' no fue encontrado.")
    {
    }
}
