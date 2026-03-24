using System.ComponentModel.DataAnnotations;

namespace PRJ_VETERINARIA.ViewModels.Usuarios
{
    public class UsuarioCreateViewModel : UsuarioViewModel
    {
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 8,
            ErrorMessage = "La contraseña debe tener entre 8 y 100 caracteres.")]
        [Display(Name = "Contraseña")]
        // hace que el input muestre *** en lugar del texto
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        // Confirmación — debe coincidir con Password
        [Required(ErrorMessage = "Debe confirmar la contraseña.")]
        [Compare(nameof(Password),
            ErrorMessage = "Las contraseñas no coinciden.")]
        [Display(Name = "Confirmar contraseña")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
    }
}
