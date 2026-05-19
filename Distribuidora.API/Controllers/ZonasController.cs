using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Distribuidora.API.Data;
using Distribuidora.Shared.Entities;

namespace Distribuidora.API.Controllers
{
    [ApiController]
    [Route("api/zonas")]
    public class ZonasController : ControllerBase
    {
        private readonly DataContext _context;

        public ZonasController(DataContext context)
        {
            _context = context;
        }

        // GET
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await _context.Zonas.ToListAsync());
        }

        // GET ID
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var zona = await _context.Zonas
                .FirstOrDefaultAsync(x => x.Id == id);

            if (zona == null)
            {
                return NotFound();
            }

            return Ok(zona);
        }

        // POST
        [HttpPost]
        public async Task<ActionResult> Post(Zona zona)
        {
            _context.Add(zona);

            await _context.SaveChangesAsync();

            return Ok(zona);
        }

        // PUT
        [HttpPut]
        public async Task<ActionResult> Put(Zona zona)
        {
            _context.Update(zona);

            await _context.SaveChangesAsync();

            return Ok(zona);
        }

        // DELETE
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var filas = await _context.Zonas
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
