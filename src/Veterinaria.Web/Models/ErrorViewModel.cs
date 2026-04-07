namespace Veterinaria.Web.Models;

/// <summary>
/// ViewModel para la página de error
/// </summary>
public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
