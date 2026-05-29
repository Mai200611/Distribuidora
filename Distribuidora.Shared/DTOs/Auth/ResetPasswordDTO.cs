using System.ComponentModel.DataAnnotations;

namespace Distribuidora.Shared.DTOs.Auth
{
    public class ResetPasswordDTO
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debes ingresar un correo válido.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Compare("Password", ErrorMessage = "La nueva contraseña y la confirmación no son iguales.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmación de contraseña")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        public string ConfirmPassword { get; set; } = null!;

        public string Token { get; set; } = null!;
    }
}
