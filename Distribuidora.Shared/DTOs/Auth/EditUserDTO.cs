using System.ComponentModel.DataAnnotations;

namespace Distribuidora.Shared.DTOs.Auth
{
    public class EditUserDTO
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; } = null!;
        public string? Photo { get; set; }
    }
}
