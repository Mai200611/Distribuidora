using Distribuidora.API.Data;
using Distribuidora.Shared.DTOs;
using Distribuidora.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Distribuidora.API.Services
{
    public class EmpleadoService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public EmpleadoService(
            DataContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(bool Ok, string Mensaje, Empleado? Empleado)>
            CrearEmpleadoAsync(EmpleadoDTO dto)
        {
            var zona = await _context.Zonas
                .Include(z => z.Empleados)
                .FirstOrDefaultAsync(z => z.Id == dto.ZonaId);

            if (zona == null)
            {
                return (false, "La zona no existe.", null);
            }

            if (zona.Empleados.Count >= zona.MaxEmpleados)
            {
                return (false,
                    "La zona ya alcanzó el máximo de empleados.",
                    null);
            }

            var vehiculoExiste = await _context.Vehiculos
                .AnyAsync(v => v.Id == dto.VehiculoId);

            if (!vehiculoExiste)
            {
                return (false, "El vehículo no existe.", null);
            }

            var empleado = _mapper.Map<Empleado>(dto);

            _context.Empleados.Add(empleado);

            await _context.SaveChangesAsync();

            return (true, "Empleado creado correctamente.", empleado);
        }

        public async Task<(bool Ok, string Mensaje)>
            ActualizarEmpleadoAsync(EmpleadoDTO dto)
        {
            var empleadoDB = await _context.Empleados
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (empleadoDB == null)
            {
                return (false, "Empleado no encontrado.");
            }

            _mapper.Map(dto, empleadoDB);

            _context.Empleados.Update(empleadoDB);

            await _context.SaveChangesAsync();

            return (true, "Empleado actualizado.");
        }
    }
}