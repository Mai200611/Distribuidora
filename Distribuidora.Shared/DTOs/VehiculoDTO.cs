namespace Distribuidora.Shared.DTOs
{
    public class VehiculoDTO
    {
        public int Id { get; set; }

        public string TipoVehiculo { get; set; } = null!;

        public string Placa { get; set; } = null!;

        public string? Marca { get; set; }

        public string? Modelo { get; set; }

        public decimal KilometrajeActual { get; set; }

        public string? EstadoVehiculo { get; set; }
    }
}