namespace Distribuidora.Shared.DTOs.Reportes
{
    public class ProductoMasVendidoDTO
    {
        public string Producto { get; set; } = null!;

        public int CantidadVendida { get; set; }

        public decimal TotalVentas { get; set; }
    }
}