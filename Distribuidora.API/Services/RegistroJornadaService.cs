using AutoMapper;
using Distribuidora.API.Data;
using Distribuidora.API.Services.Interfaces;
using Distribuidora.Shared.DTOs;
using Distribuidora.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Distribuidora.API.Services
{
    public class RegistroJornadaService : IRegistroJornadaService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public RegistroJornadaService(
            DataContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(bool Ok, string? Mensaje, RegistroJornadaDTO? Registro)>
            CrearRegistroAsync(RegistroJornadaDTO dto)
        {
            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(x => x.Id == dto.EmpleadoId);

         
            if (empleado == null)
            {
                return (false, "El empleado no existe.", null);
            }

            var vehiculo = await _context.Vehiculos
                .FirstOrDefaultAsync(x => x.Id == dto.VehiculoId);

            if (vehiculo == null)
            {
                return (false, "El vehículo no existe.", null);
            }

            var tienda = await _context.Tiendas
                .FirstOrDefaultAsync(x => x.Id == dto.TiendaId);

            if (tienda == null)
            {
                return (false, "La tienda no existe.", null);
            }

            var existe = await _context.RegistrosJornada
                .AnyAsync(x =>
                    x.EmpleadoId == dto.EmpleadoId &&
                    x.Fecha.Date == dto.Fecha.Date);

            if (existe)
            {
                return (false,
                    "El empleado ya tiene una jornada registrada ese día.",
                    null);
            }

            if (dto.HoraFin <= dto.HoraInicio)
            {
                return (
                    false,
                    "La hora final debe ser mayor a la hora inicial.",
                    null
                );
            }
            
            if (dto.KilometrosRecorridos < 0)
            {
                return (
                    false,
                    "Los kilómetros recorridos no pueden ser negativos.",
                    null
                );
            }

            if (vehiculo == null)
            {
                return (false, "El vehículo no existe.", null);
            }

            if (empleado.VehiculoId != dto.VehiculoId)
            {
                return (
                    false,
                    "El vehículo no pertenece al empleado.",
                    null
                );
            }

            var registro = _mapper.Map<RegistroJornada>(dto);
            
            vehiculo.KilometrajeActual += dto.KilometrosRecorridos;
            
            _context.Add(registro);
           

            await _context.SaveChangesAsync();

            return (
                true,
                null,
                _mapper.Map<RegistroJornadaDTO>(registro)
            );
        }
    }
}