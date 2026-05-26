namespace Distribuidora.Shared.DTOs.Reportes
{
    public class KilometrosVehiculoDTO
    {
        public string Vehiculo { get; set; } = null!;

        public string Placa { get; set; } = null!;

        public decimal TotalKilometros { get; set; }
    }
}