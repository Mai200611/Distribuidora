using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Distribuidora.Shared.Entities
{
    public class Vehiculo
    {
        public int Id { get; set; }

        [Display(Name = "Tipo Vehículo")]
        [MaxLength(50)]
        [Required]
        public string TipoVehiculo { get; set; } = null!;

        [Display(Name = "Placa")]
        [MaxLength(10)]
        [Required]
        public string Placa { get; set; } = null!;

        [Display(Name = "Marca")]
        [MaxLength(50)]
        public string? Marca { get; set; }

        [Display(Name = "Modelo")]
        [MaxLength(50)]
        public string? Modelo { get; set; }

        [Display(Name = "Kilometraje")]
        public decimal KilometrajeActual { get; set; }

        [Display(Name = "Estado")]
        [MaxLength(30)]
        public string? EstadoVehiculo { get; set; }

        // Relación
        [JsonIgnore]
        public ICollection<Empleado>? Empleados { get; set; }
    }
}
