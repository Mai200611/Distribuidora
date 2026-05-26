namespace Distribuidora.Shared.DTOs
{
    public class ZonaDTO
    {
        public int Id { get; set; }

        public string NombreZona { get; set; } = null!;

        public string? Descripcion { get; set; }

        public int MaxEmpleados { get; set; }
    }
}
