using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Distribuidora.API.Data;
using Distribuidora.Shared.Entities;

namespace Distribuidora.API.Controllers
{
    [ApiController]
    [Route("api/vehiculos")]
    public class VehiculosController : ControllerBase
    {
        private readonly DataContext _context;

        public VehiculosController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await _context.Vehiculos.ToListAsync());
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

            return Ok(vehiculo);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Vehiculo vehiculo)
        {
            var existe = await _context.Vehiculos
            .AnyAsync(x => x.Placa == vehiculo.Placa);

            if (existe)
            {
                return BadRequest("La placa ya existe.");
            }

            _context.Add(vehiculo);

            await _context.SaveChangesAsync();

            return Ok(vehiculo);
        }

        [HttpPut]
        public async Task<ActionResult> Put(Vehiculo vehiculo)
        {
            var existe = await _context.Vehiculos
            .AnyAsync(x => x.Placa == vehiculo.Placa);

            if (existe)
            {
                return BadRequest("La placa ya existe.");
            }
            
            _context.Update(vehiculo);

            await _context.SaveChangesAsync();

            return Ok(vehiculo);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var filas = await _context.Vehiculos
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
