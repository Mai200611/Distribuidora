namespace Distribuidora.Shared.DTOs
{
    public class ProductoDTO
    {
        public int Id { get; set; }

        public string NombreProducto { get; set; } = null!;

        public string? Categoria { get; set; }

        public decimal Precio { get; set; }

        public int Stock { get; set; }

        public string? Marca { get; set; }
    }
}
