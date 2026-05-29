using System.ComponentModel.DataAnnotations;

namespace Distribuidora.Shared.DTOs.Auth
{
    public class ChangePasswordDTO
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña actual")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        public string CurrentPassword { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        public string NewPassword { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Compare("NewPassword", ErrorMessage = "La nueva contraseña y la confirmación no son iguales.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmación nueva contraseña")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        public string Confirm { get; set; } = null!;
    }
}
