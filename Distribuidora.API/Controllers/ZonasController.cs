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
    [Route("api/zonas")]
    [Authorize(Roles = "Admin,Supervisor")]
    public class ZonasController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok(User.Identity!.Name);
        }

        
        
        [HttpGet("perfil")]
        public IActionResult Perfil()
        {
            return Ok(new
            {
                Email = User.FindFirst(ClaimTypes.Name)?.Value,
                Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            });
        }

        public ZonasController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/zonas
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var zonas = await _context.Zonas.ToListAsync();

            return Ok(_mapper.Map<List<ZonaDTO>>(zonas));
        }

        // GET: api/zonas/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var zona = await _context.Zonas
                .FirstOrDefaultAsync(x => x.Id == id);

            if (zona == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ZonaDTO>(zona));
        }

        // POST: api/zonas
        [HttpPost]
        public async Task<ActionResult> Post(ZonaDTO dto)
        {
            var existe = await _context.Zonas
                .AnyAsync(x => x.NombreZona == dto.NombreZona);

            if (existe)
            {
                return BadRequest("La zona ya existe.");
            }

            var zona = _mapper.Map<Zona>(dto);

            _context.Add(zona);

            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<ZonaDTO>(zona));
        }

        // PUT: api/zonas
        [HttpPut]
        public async Task<ActionResult> Put(ZonaDTO dto)
        {
            var zona = await _context.Zonas
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (zona == null)
            {
                return NotFound();
            }

            var existe = await _context.Zonas
                .AnyAsync(x =>
                    x.NombreZona == dto.NombreZona &&
                    x.Id != dto.Id);

            if (existe)
            {
                return BadRequest("La zona ya existe.");
            }

            zona = _mapper.Map(dto, zona);

            _context.Update(zona);

            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<ZonaDTO>(zona));
        }

        // DELETE: api/zonas/1
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