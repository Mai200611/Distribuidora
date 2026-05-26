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
    [Authorize(Roles = "Admin,Empleado,Supervisor")]
    [ApiController]
    [Route("api/productos")]
    public class ProductosController : ControllerBase
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

        public ProductosController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/productos
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var productos = await _context.Productos.ToListAsync();

            return Ok(_mapper.Map<List<ProductoDTO>>(productos));
        }

        // GET: api/productos/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(x => x.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ProductoDTO>(producto));
        }

        // POST: api/productos
        [HttpPost]
        public async Task<ActionResult> Post(ProductoDTO dto)
        {
            var existe = await _context.Productos
                .AnyAsync(x => x.NombreProducto == dto.NombreProducto);

            if (existe)
            {
                return BadRequest("El producto ya existe.");
            }

            var producto = _mapper.Map<Producto>(dto);

            _context.Add(producto);

            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<ProductoDTO>(producto));
        }

        // PUT: api/productos
        [HttpPut]
        public async Task<ActionResult> Put(ProductoDTO dto)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (producto == null)
            {
                return NotFound();
            }

            var existe = await _context.Productos
                .AnyAsync(x =>
                    x.NombreProducto == dto.NombreProducto &&
                    x.Id != dto.Id);

            if (existe)
            {
                return BadRequest("El producto ya existe.");
            }

            producto = _mapper.Map(dto, producto);

            _context.Update(producto);

            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<ProductoDTO>(producto));
        }

        // DELETE: api/productos/1
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var filas = await _context.Productos
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