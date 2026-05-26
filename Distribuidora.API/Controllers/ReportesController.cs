using Distribuidora.API.Data;
using Distribuidora.Shared.DTOs.Reportes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Distribuidora.API.Controllers
{
    [ApiController]
    [Route("api/reportes")]
    [Authorize(Roles = "Admin,Empleado,Supervisor")]
    public class ReportesController : ControllerBase
    {
        private readonly DataContext _context;


        [HttpGet("ventas-por-empleado")]
        public async Task<ActionResult> VentasPorEmpleado()
        {
            var reporte = await _context.RegistrosJornada
                .Include(x => x.Empleado)
                .Include(x => x.DetallesVenta)
                .Select(x => new
                {
                    Empleado = x.Empleado.NombreCompleto,
                    Total = x.DetallesVenta.Sum(d => d.Subtotal)
                })
                .GroupBy(x => x.Empleado)
                .Select(g => new VentasPorEmpleadoDTO
                {
                    Empleado = g.Key,
                    TotalVentas = g.Sum(x => x.Total)
                })
                .OrderByDescending(x => x.TotalVentas)
                .ToListAsync();

            return Ok(reporte);
        }

        [HttpGet("productos-mas-vendidos")]
        public async Task<ActionResult> ProductosMasVendidos()
        {
            var reporte = await _context.DetallesVenta
                .Include(x => x.Producto)
                .GroupBy(x => x.Producto.NombreProducto)
                .Select(g => new ProductoMasVendidoDTO
                {
                    Producto = g.Key,

                    CantidadVendida = g.Sum(x => x.CantidadVendida),

                    TotalVentas = g.Sum(x => x.Subtotal)
                })
                .OrderByDescending(x => x.CantidadVendida)
                .ToListAsync();

            return Ok(reporte);
        }

        [HttpGet("kilometros-vehiculo")]
        public async Task<ActionResult> KilometrosVehiculo()
        {
            var reporte = await _context.RegistrosJornada
                .Include(x => x.Vehiculo)
                .GroupBy(x => new
                {
                    x.Vehiculo.TipoVehiculo,
                    x.Vehiculo.Placa
                })
                .Select(g => new KilometrosVehiculoDTO
                {
                    Vehiculo = g.Key.TipoVehiculo,

                    Placa = g.Key.Placa,

                    TotalKilometros =
                        g.Sum(x => x.KilometrosRecorridos)
                })
                .OrderByDescending(x => x.TotalKilometros)
                .ToListAsync();

            return Ok(reporte);
        }
        
        [HttpGet("ventas-por-fecha")]
        public async Task<ActionResult> VentasPorFecha(DateTime fecha)
        {
            var registros = await _context.RegistrosJornada
                .Include(x => x.DetallesVenta)
                .Where(x => x.Fecha.Date == fecha.Date)
                .ToListAsync();

            if (!registros.Any())
            {
                return NotFound("No hay registros para esa fecha.");
            }

            var reporte = new VentasPorFechaDTO
            {
                Fecha = fecha.Date,

                TotalVentas = registros
                    .SelectMany(x => x.DetallesVenta)
                    .Sum(x => x.Subtotal),

                TotalJornadas = registros.Count,

                ProductosVendidos = registros
                    .SelectMany(x => x.DetallesVenta)
                    .Sum(x => x.CantidadVendida)
            };

            return Ok(reporte);
        }
    }
}