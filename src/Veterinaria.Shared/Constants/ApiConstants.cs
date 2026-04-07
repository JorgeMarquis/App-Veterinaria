namespace Veterinaria.Shared.Constants;

/// <summary>
/// Constantes compartidas para la API
/// </summary>
public static class ApiConstants
{
    public const string ApiVersion = "v1";
    public const string ApiName = "Veterinaria API";
    public const string ApiDescription = "API REST para gestión de veterinaria";
    
    public static class ValidationMessages
    {
        public const string FieldRequired = "El campo {0} es requerido.";
        public const string EmailInvalid = "El formato del correo electrónico no es válido.";
        public const string PhoneInvalid = "El formato del teléfono no es válido.";
        public const string StringTooLong = "El campo {0} no puede exceder {1} caracteres.";
        public const string StringTooShort = "El campo {0} debe tener al menos {1} caracteres.";
    }

    public static class HttpMessages
    {
        public const string NotFound = "El recurso solicitado no fue encontrado.";
        public const string BadRequest = "La solicitud contiene datos inválidos.";
        public const string Unauthorized = "No autorizado para acceder a este recurso.";
        public const string Forbidden = "No tiene permisos para acceder a este recurso.";
        public const string InternalError = "Ocurrió un error interno en el servidor.";
    }
}
