using Distribuidora.Shared.DTOs;

namespace Distribuidora.API.Services.Interfaces
{
    public interface IEmpleadoService
    {
        Task<(bool ok, string? error, EmpleadoDTO? empleado)>
            CrearEmpleadoAsync(EmpleadoDTO dto);
    }
}