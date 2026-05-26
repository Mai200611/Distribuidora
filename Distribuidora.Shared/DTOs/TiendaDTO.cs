namespace Distribuidora.Shared.DTOs
{
    public class TiendaDTO
    {
        public int Id { get; set; }

        public string NombreTienda { get; set; } = null!;

        public string? EncargadoTienda { get; set; }

        public string? Direccion { get; set; }

        public string? Telefono { get; set; }

        public string? Barrio { get; set; }

        public string? Zona { get; set; }
    }
}