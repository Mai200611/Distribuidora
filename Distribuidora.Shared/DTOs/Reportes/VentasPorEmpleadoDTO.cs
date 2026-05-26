namespace Distribuidora.Shared.DTOs.Reportes
{
    public class VentasPorEmpleadoDTO
    {
        public string Empleado { get; set; } = null!;

        public decimal TotalVentas { get; set; }
    }
}