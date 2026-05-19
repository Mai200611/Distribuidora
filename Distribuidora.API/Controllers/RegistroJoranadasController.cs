using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Distribuidora.API.Data;
using Distribuidora.Shared.Entities;

namespace Distribuidora.API.Controllers
{
    [ApiController]
    [Route("api/registros")]
    public class RegistroJornadasController : ControllerBase
    {
        private readonly DataContext _context;

        public RegistroJornadasController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await _context.RegistrosJornada
                .Include(x => x.Empleado)
                .Include(x => x.Vehiculo)
                .Include(x => x.Tienda)
                .Include(x => x.DetallesVenta)
                .ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var registro = await _context.RegistrosJornada
                .Include(x => x.Empleado)
                .Include(x => x.Vehiculo)
                .Include(x => x.Tienda)
                .Include(x => x.DetallesVenta)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (registro == null)
            {
                return NotFound();
            }

            return Ok(registro);
        }

        [HttpPost]
        public async Task<ActionResult> Post(RegistroJornada registro)
        {
            _context.Add(registro);

            await _context.SaveChangesAsync();

            return Ok(registro);
        }

        [HttpPut]
        public async Task<ActionResult> Put(RegistroJornada registro)
        {
            _context.Update(registro);

            await _context.SaveChangesAsync();

            return Ok(registro);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var filas = await _context.RegistrosJornada
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
