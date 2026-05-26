using Distribuidora.Shared.DTOs;

namespace Distribuidora.API.Services.Interfaces
{
    public interface IRegistroJornadaService
    {
        Task<(bool Ok, string? Mensaje, RegistroJornadaDTO? Registro)>
            CrearRegistroAsync(RegistroJornadaDTO dto);
    }
}