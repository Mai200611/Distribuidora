using AutoMapper;
using Distribuidora.API.Data;
using Distribuidora.API.Services.Interfaces;
using Distribuidora.Shared.DTOs;
using Distribuidora.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;



namespace Distribuidora.API.Controllers
{
    [ApiController]
    [Route("api/detallesventa")]
    [Authorize(Roles = "Admin,Empleado")]
    public class DetalleVentasController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IDetalleVentaService _service;
        
        [HttpGet("perfil")]
        public IActionResult Perfil()
        {
            return Ok(new
            {
                Email = User.FindFirst(ClaimTypes.Name)?.Value,
                Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            });
        }

        public DetalleVentasController(
         DataContext context,
         IMapper mapper,
         IDetalleVentaService service)
        {
            _context = context;
            _mapper = mapper;
            _service = service;
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
        public async Task<ActionResult> Post(DetalleVentaDTO dto)
        {
            var resultado = await _service
                .CrearDetalleAsync(dto);

            if (!resultado.ok)
            {
                return BadRequest(resultado.error);
            }

            return Ok(resultado.detalle);
        }

        [HttpPut]
        public async Task<ActionResult> Put(DetalleVentaDTO dto)
        {
            var detalleDB = await _context.DetallesVenta
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (detalleDB == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .FirstOrDefaultAsync(x => x.Id == dto.ProductoId);

            if (producto == null)
            {
                return BadRequest("El producto no existe.");
            }

            detalleDB.CantidadVendida = dto.CantidadVendida;
            detalleDB.ProductoId = dto.ProductoId;
            detalleDB.RegistroJornadaId = dto.RegistroJornadaId;

            detalleDB.Subtotal =
                dto.CantidadVendida * producto.Precio;

            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<DetalleVentaDTO>(detalleDB));
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