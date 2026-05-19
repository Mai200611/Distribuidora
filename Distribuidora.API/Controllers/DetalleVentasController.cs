using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Distribuidora.API.Data;
using Distribuidora.Shared.Entities;

namespace Distribuidora.API.Controllers
{
    [ApiController]
    [Route("api/detallesventa")]
    public class DetalleVentasController : ControllerBase
    {
        private readonly DataContext _context;

        public DetalleVentasController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await _context.DetallesVenta
                .Include(x => x.Producto)
                .Include(x => x.RegistroJornada)
                .ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var detalle = await _context.DetallesVenta
                .Include(x => x.Producto)
                .Include(x => x.RegistroJornada)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (detalle == null)
            {
                return NotFound();
            }

            return Ok(detalle);
        }

        [HttpPost]
        public async Task<ActionResult> Post(DetalleVenta detalle)
        {
            var producto = await _context.Productos
            .FirstOrDefaultAsync(x => x.Id == detalle.ProductoId);

            if (producto == null)
            {
                return BadRequest("El producto no existe.");
            }

            detalle.Subtotal =
            detalle.CantidadVendida * producto.Precio;
            _context.Add(detalle);

            await _context.SaveChangesAsync();

            return Ok(detalle);
        }

        [HttpPut]
        public async Task<ActionResult> Put(DetalleVenta detalle)
        {
            _context.Update(detalle);

            await _context.SaveChangesAsync();

            return Ok(detalle);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var filas = await _context.DetallesVenta
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