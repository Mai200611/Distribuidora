using AutoMapper;
using Distribuidora.API.Data;
using Distribuidora.API.Services.Interfaces;
using Distribuidora.Shared.DTOs;
using Distribuidora.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Distribuidora.API.Services
{
    public class DetalleVentaService : IDetalleVentaService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public DetalleVentaService(
            DataContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(bool ok, string? error, DetalleVentaDTO? detalle)>
            CrearDetalleAsync(DetalleVentaDTO dto)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(x => x.Id == dto.ProductoId);

            if (producto == null)
            {
                return (false, "El producto no existe.", null);
            }

            if (dto.CantidadVendida > producto.Stock)
            {
                return (false, "No hay suficiente stock.", null);
            }

            var detalle = _mapper.Map<DetalleVenta>(dto);

            detalle.Subtotal =
                dto.CantidadVendida * producto.Precio;

            producto.Stock -= dto.CantidadVendida;

            _context.Add(detalle);

            await _context.SaveChangesAsync();

            return (
                true,
                null,
                _mapper.Map<DetalleVentaDTO>(detalle)
            );
        }
    }
}
