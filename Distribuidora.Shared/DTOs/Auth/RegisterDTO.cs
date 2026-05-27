using System.ComponentModel.DataAnnotations;

namespace Distribuidora.Shared.DTOs.Auth
{
    public class RegisterDTO
    {
        [Required]
        public string NombreCompleto { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Password { get; set; } = null!;
        [Required]
        public string Role { get; set; } = null!;
        public string? Photo { get; set; }
    }
}
