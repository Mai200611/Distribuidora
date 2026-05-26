using Distribuidora.Shared.DTOs;

namespace Distribuidora.API.Services.Interfaces
{
    public interface IDetalleVentaService
    {
        Task<(bool ok, string? error, DetalleVentaDTO? detalle)>
            CrearDetalleAsync(DetalleVentaDTO dto);
    }
}
