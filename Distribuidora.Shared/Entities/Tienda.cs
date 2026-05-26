using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Distribuidora.Shared.Entities
{
    public class Tienda
    {
        public int Id { get; set; }

        [Display(Name = "Nombre Tienda")]
        [MaxLength(100)]
        [Required]
        public string NombreTienda { get; set; } = null!;

        [Display(Name = "Encargado")]
        [MaxLength(100)]
        public string? EncargadoTienda { get; set; }

        [Display(Name = "Dirección")]
        [MaxLength(150)]
        public string? Direccion { get; set; }

        [Display(Name = "Teléfono")]
        [MaxLength(20)]
        public string? Telefono { get; set; }

        [Display(Name = "Barrio")]
        [MaxLength(50)]
        public string? Barrio { get; set; }

        [Display(Name = "Zona")]
        [MaxLength(50)]
        public string? Zona { get; set; }

        // Relación
        [JsonIgnore]
        public ICollection<RegistroJornada>? RegistrosJornada { get; set; }
    }
}