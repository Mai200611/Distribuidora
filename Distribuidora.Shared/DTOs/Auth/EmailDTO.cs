using System.ComponentModel.DataAnnotations;

namespace Distribuidora.Shared.DTOs.Auth
{
    public class EmailDTO
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debes ingresar un correo válido.")]
        public string Email { get; set; } = null!;
    }
}
