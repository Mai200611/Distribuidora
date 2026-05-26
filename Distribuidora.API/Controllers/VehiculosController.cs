using AutoMapper;
using Distribuidora.API.Data;
using Distribuidora.Shared.DTOs;
using Distribuidora.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Distribuidora.API.Controllers
{
    [ApiController]
    [Route("api/vehiculos")]
    [Authorize(Roles = "Admin,Empleado,Supervisor")]
    public class VehiculosController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        [HttpGet("perfil")]
        public IActionResult Perfil()
        {
            return Ok(new
            {
                Email = User.FindFirst(ClaimTypes.Name)?.Value,
                Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            });
        }

        public VehiculosController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var vehiculos = await _context.Vehiculos.ToListAsync();

            return Ok(_mapper.Map<List<VehiculoDTO>>(vehiculos));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var vehiculo = await _context.Vehiculos
                .FirstOrDefaultAsync(x => x.Id == id);

            if (vehiculo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<VehiculoDTO>(vehiculo));
        }

        [HttpPost]
        public async Task<ActionResult> Post(VehiculoDTO dto)
        {
            var existe = await _context.Vehiculos
                .AnyAsync(x => x.Placa == dto.Placa);

            if (existe)
            {
                return BadRequest("La placa ya existe.");
            }

            var vehiculo = _mapper.Map<Vehiculo>(dto);

            _context.Vehiculos.Add(vehiculo);

            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<VehiculoDTO>(vehiculo));
        }

        [HttpPut]
        public async Task<ActionResult> Put(VehiculoDTO dto)
        {
            var vehiculo = await _context.Vehiculos
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (vehiculo == null)
            {
                return NotFound();
            }

            var placaExiste = await _context.Vehiculos
                .AnyAsync(x => x.Placa == dto.Placa && x.Id != dto.Id);

            if (placaExiste)
            {
                return BadRequest("La placa ya existe.");
            }

            _mapper.Map(dto, vehiculo);

            _context.Update(vehiculo);

            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<VehiculoDTO>(vehiculo));
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var vehiculo = await _context.Vehiculos
                .FirstOrDefaultAsync(x => x.Id == id);

            if (vehiculo == null)
            {
                return NotFound();
            }

            var tieneEmpleados = await _context.Empleados
                .AnyAsync(x => x.VehiculoId == id);

            if (tieneEmpleados)
            {
                return BadRequest(new
                {
                    ok = false,
                    mensaje = "No se puede eliminar el vehículo porque tiene empleados asociados."
                });
            }

            var tieneRegistros = await _context.RegistrosJornada
                .AnyAsync(x => x.VehiculoId == id);

            if (tieneRegistros)
            {
                return BadRequest(new
                {
                    ok = false,
                    mensaje = "No se puede eliminar el vehículo porque tiene registros asociados."
                });
            }

            _context.Vehiculos.Remove(vehiculo);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}