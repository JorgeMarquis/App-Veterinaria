using System.ComponentModel.DataAnnotations;

namespace PRJ_VETERINARIA.ViewModels.Usuarios
{
    public class UsuarioViewModel
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        [StringLength(150, ErrorMessage = "Máximo 150 caracteres.")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(150, ErrorMessage = "Máximo 150 caracteres.")]
        [Display(Name = "Nombre completo")]
        public string? NombreCompleto { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        [StringLength(30)]
        [Display(Name = "Rol")]
        public string? Rol { get; set; }

        [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        [Display(Name = "Especialidad")]
        public string? Especialidad { get; set; }

        [StringLength(50, ErrorMessage = "Máximo 50 caracteres.")]
        [Display(Name = "Número de colegiado")]
        public string? NumeroColegiado { get; set; }

        [StringLength(20, ErrorMessage = "Máximo 20 caracteres.")]
        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        public static readonly List<string> RolesDisponibles = new()
        {
            "Administrador",
            "Veterinario",
            "Recepcionista",
            "Asistente"
        };
    }
}
