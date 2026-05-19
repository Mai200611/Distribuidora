using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Distribuidora.API.Data;
using Distribuidora.Shared.Entities;

namespace Distribuidora.API.Controllers
{
    [ApiController]
    [Route("api/empleados")]
    public class EmpleadosController : ControllerBase
    {
        private readonly DataContext _context;

        public EmpleadosController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await _context.Empleados
                .Include(x => x.Zona)
                .Include(x => x.Vehiculo)
                .ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var empleado = await _context.Empleados
                .Include(x => x.Zona)
                .Include(x => x.Vehiculo)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (empleado == null)
            {
                return NotFound();
            }

            return Ok(empleado);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Empleado empleado)
        
        {
            var cantidadEmpleados = await _context.Empleados
            .CountAsync(x => x.ZonaId == empleado.ZonaId);

            if (cantidadEmpleados >= 3)
            {
                return BadRequest("La zona ya tiene el máximo de empleados permitidos.");
            }

            _context.Add(empleado);

            await _context.SaveChangesAsync();

            return Ok(empleado);
        }

        [HttpPut]
        public async Task<ActionResult> Put(Empleado empleado)
        {
            var cantidadEmpleados = await _context.Empleados
            .CountAsync(x => x.ZonaId == empleado.ZonaId);

            if (cantidadEmpleados >= 3)
            {
                return BadRequest("La zona ya tiene el máximo de empleados permitidos.");
            }
            _context.Update(empleado);

            await _context.SaveChangesAsync();

            return Ok(empleado);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var filas = await _context.Empleados
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();

            if (filas == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
