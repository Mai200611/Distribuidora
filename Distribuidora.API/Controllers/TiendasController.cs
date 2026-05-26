using Distribuidora.API.Data;
using Distribuidora.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Distribuidora.API.Controllers
{
    [ApiController]
    [Route("api/tiendas")]
    [Authorize(Roles = "Admin,Empleado,Supervisor")]
    public class TiendasController : ControllerBase
    {
        private readonly DataContext _context;
        
        [HttpGet("perfil")]
        public IActionResult Perfil()
        {
            return Ok(new
            {
                Email = User.FindFirst(ClaimTypes.Name)?.Value,
                Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            });
        }

        public TiendasController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await _context.Tiendas.ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var tienda = await _context.Tiendas
                .FirstOrDefaultAsync(x => x.Id == id);

            if (tienda == null)
            {
                return NotFound();
            }

            return Ok(tienda);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Tienda tienda)
        {
            _context.Add(tienda);

            await _context.SaveChangesAsync();

            return Ok(tienda);
        }

        [HttpPut]
        public async Task<ActionResult> Put(Tienda tienda)
        {
            _context.Update(tienda);

            await _context.SaveChangesAsync();

            return Ok(tienda);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var filas = await _context.Tiendas
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