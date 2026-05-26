namespace Distribuidora.Shared.DTOs
{
    public class DetalleVentaDTO
    {
        public int Id { get; set; }

        public int CantidadVendida { get; set; }

        public decimal Subtotal { get; set; }

        public int RegistroJornadaId { get; set; }

        public int ProductoId { get; set; }
    }
}