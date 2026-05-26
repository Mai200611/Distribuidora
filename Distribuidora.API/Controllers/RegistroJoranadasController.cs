using AutoMapper;
using Distribuidora.API.Data;
using Distribuidora.API.Services.Interfaces;
using Distribuidora.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Distribuidora.API.Controllers
{
    [ApiController]
    [Route("api/registros")]
    [Authorize(Roles = "Admin,Supervisor")]
    public class RegistroJornadasController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IRegistroJornadaService _service;

        [HttpGet("perfil")]
        public IActionResult Perfil()
        {
            return Ok(new
            {
                Email = User.FindFirst(ClaimTypes.Name)?.Value,
                Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            });
        }
        public RegistroJornadasController(
            DataContext context,
            IMapper mapper,
            IRegistroJornadaService service)
        {
            _context = context;
            _mapper = mapper;
            _service = service;
        }

        // GET
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var registros = await _context.RegistrosJornada
                .Include(x => x.Empleado)
                .Include(x => x.Vehiculo)
                .Include(x => x.Tienda)
                .Include(x => x.DetallesVenta)
                .ToListAsync();

            return Ok(_mapper.Map<List<RegistroJornadaDTO>>(registros));
        }

        // GET ID
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

            return Ok(_mapper.Map<RegistroJornadaDTO>(registro));
        }

        // POST
        [HttpPost]
        public async Task<ActionResult> Post(RegistroJornadaDTO dto)
        {
            var resultado = await _service
                .CrearRegistroAsync(dto);

            if (!resultado.Ok)
            {
                return BadRequest(resultado.Mensaje);
            }

            return Ok(resultado.Registro);
        }

        // PUT
        [HttpPut]
        public async Task<ActionResult> Put(RegistroJornadaDTO dto)
        {
            var registro = await _context.RegistrosJornada
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (registro == null)
            {
                return NotFound();
            }

            registro = _mapper.Map(dto, registro);

            _context.Update(registro);

            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<RegistroJornadaDTO>(registro));
        }

        // DELETE
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