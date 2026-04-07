namespace Veterinaria.Shared.Exceptions;

/// <summary>
/// Excepción lanzada cuando hay errores de validación
/// </summary>
public class ValidationException : Exception
{
    public Dictionary<string, string[]> Errors { get; }

    public ValidationException(string message) : base(message)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(Dictionary<string, string[]> errors)
        : base("Ocurrieron errores de validación.")
    {
        Errors = errors;
    }
}
