using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Distribuidora.Shared.Entities
{
    public class Empleado
    {
        public int Id { get; set; }

        [Display(Name = "Nombre Completo")]
        [MaxLength(100)]
        [Required]
        public string NombreCompleto { get; set; } = null!;

        [Display(Name = "Documento")]
        [MaxLength(20)]
        [Required]
        public string Documento { get; set; } = null!;

        [Display(Name = "Teléfono")]
        [MaxLength(20)]
        public string? Telefono { get; set; }

        [Display(Name = "Correo")]
        [MaxLength(100)]
        [EmailAddress]
        public string? Correo { get; set; }

        [Display(Name = "Fecha Ingreso")]
        public DateTime FechaIngreso { get; set; }

        [Display(Name = "Estado")]
        [MaxLength(20)]
        public string? Estado { get; set; }

        // FK Zona
        public int ZonaId { get; set; }
        [JsonIgnore]
        public Zona? Zona { get; set; }

        // FK Vehiculo
        public int VehiculoId { get; set; }
        [JsonIgnore]
        public Vehiculo? Vehiculo { get; set; }

        // Relación
        [JsonIgnore]
        public ICollection<RegistroJornada>? RegistrosJornada { get; set; }
    }
}