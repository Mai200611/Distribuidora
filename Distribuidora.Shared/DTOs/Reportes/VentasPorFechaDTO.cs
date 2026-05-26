namespace Distribuidora.Shared.DTOs.Reportes
{
    public class VentasPorFechaDTO
    {
        public DateTime Fecha { get; set; }

        public decimal TotalVentas { get; set; }

        public int TotalJornadas { get; set; }

        public int ProductosVendidos { get; set; }
    }
}