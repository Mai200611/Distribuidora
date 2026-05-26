using System.ComponentModel;

namespace Distribuidora.Shared.DTOs.Auth
{
    public class AuthResponseDTO
    {
        public bool Ok { get; set; }

        public string? Token { get; set; }

        public string? Mensaje { get; set; }

    }
}