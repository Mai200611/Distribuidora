namespace Distribuidora.Shared.DTOs
{
    public class RegistroJornadaDTO
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraFin { get; set; }

        public decimal KilometrosRecorridos { get; set; }

        public decimal VentaTotal { get; set; }

        public string? Observaciones { get; set; }

        public int EmpleadoId { get; set; }

        public int VehiculoId { get; set; }

        public int TiendaId { get; set; }
    }
}
