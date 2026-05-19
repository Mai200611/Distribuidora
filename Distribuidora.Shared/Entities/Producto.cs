using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Distribuidora.Shared.Entities
{
    public class Producto
    {
        public int Id { get; set; }

        [Display(Name = "Nombre Producto")]
        [MaxLength(100)]
        [Required]
        public string NombreProducto { get; set; } = null!;

        [Display(Name = "Categoría")]
        [MaxLength(50)]
        public string? Categoria { get; set; }

        [Display(Name = "Precio")]
        public decimal Precio { get; set; }

        [Display(Name = "Stock")]
        public int Stock { get; set; }

        [Display(Name = "Marca")]
        [MaxLength(50)]
        public string? Marca { get; set; }

        // Relación
        [JsonIgnore]
        public ICollection<DetalleVenta>? DetallesVenta { get; set; }
    }
}
