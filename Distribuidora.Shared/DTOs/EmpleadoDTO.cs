namespace Distribuidora.Shared.DTOs
{
    public class EmpleadoDTO
    {
        public int Id { get; set; }

        public string NombreCompleto { get; set; } = null!;

        public string Documento { get; set; } = null!;

        public string? Telefono { get; set; }

        public string? Correo { get; set; }

        public DateTime FechaIngreso { get; set; }

        public string? Estado { get; set; }

        public int ZonaId { get; set; }

        public int VehiculoId { get; set; }
    }
}