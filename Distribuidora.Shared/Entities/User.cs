using Microsoft.AspNetCore.Identity;

namespace Distribuidora.Shared.Entities
{
    public class User : IdentityUser
    {
        public string NombreCompleto { get; set; } = null!;
    }
}