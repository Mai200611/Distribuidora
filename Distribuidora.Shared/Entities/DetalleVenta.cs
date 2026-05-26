using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Distribuidora.Shared.Entities
{
    public class DetalleVenta
    {
        public int Id { get; set; }

        [Display(Name = "Cantidad Vendida")]
        public int CantidadVendida { get; set; }

        [Display(Name = "Subtotal")]
        public decimal Subtotal { get; set; }

        // FK Registro
        public int RegistroJornadaId { get; set; }
        [JsonIgnore]
        public RegistroJornada? RegistroJornada { get; set; }

        // FK Producto
        public int ProductoId { get; set; }
        [JsonIgnore]
        public Producto? Producto { get; set; }
    }
}