using System.ComponentModel.DataAnnotations;

namespace Distribuidora.Shared.Entities
{
    public class Zona
    {
        public int Id { get; set; }

        [Display(Name = "Nombre Zona")]
        [MaxLength(100, ErrorMessage = "Máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string NombreZona { get; set; } = null!;

        [Display(Name = "Descripción")]
        [MaxLength(200)]
        public string? Descripcion { get; set; }

        [Display(Name = "Máximo Empleados")]
        public int MaxEmpleados { get; set; } = 3;
    }
}
