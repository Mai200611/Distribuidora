using AutoMapper;
using Distribuidora.API.Data;
using Distribuidora.API.Services;
using Distribuidora.Shared.DTOs;
using Distribuidora.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Distribuidora.API.Controllers
{
    [ApiController]
    [Route("api/empleados")]
    [Authorize(Roles = "Admin,Supervisor")]
    public class EmpleadosController : ControllerBase
    {
        [HttpGet("perfil")]
        public IActionResult Perfil()
        {
            return Ok(new
            {
                Email = User.FindFirst(ClaimTypes.Name)?.Value,
                Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            });
        }

        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly EmpleadoService _service;

        public EmpleadosController(
            DataContext context,
            IMapper mapper,
            EmpleadoService service)
        {
            _context = context;
            _mapper = mapper;
            _service = service;
        }

        // GET
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var empleados = await _context.Empleados
                .Include(x => x.Zona)
                .Include(x => x.Vehiculo)
                .ToListAsync();

            return Ok(_mapper.Map<List<EmpleadoDTO>>(empleados));
        }

        // GET ID
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

            return Ok(_mapper.Map<EmpleadoDTO>(empleado));
        }

        // POST
        [HttpPost]
        public async Task<ActionResult> Post(EmpleadoDTO dto)
        {
            var resultado = await _service
                .CrearEmpleadoAsync(dto);

            if (!resultado.Ok)
            {
                return BadRequest(resultado.Mensaje);
            }

            return Ok(_mapper.Map<EmpleadoDTO>(resultado.Empleado));
        }

        // PUT
        [HttpPut]
        public async Task<ActionResult> Put(EmpleadoDTO dto)
        {
            var resultado = await _service
                .ActualizarEmpleadoAsync(dto);

            if (!resultado.Ok)
            {
                return NotFound(resultado.Mensaje);
            }

            return Ok(resultado.Mensaje);
        }

        // DELETE
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