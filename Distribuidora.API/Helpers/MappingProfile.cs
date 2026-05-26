using AutoMapper;
using Distribuidora.Shared.DTOs;
using Distribuidora.Shared.Entities;

namespace Distribuidora.API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Empleado, EmpleadoDTO>().ReverseMap();

            CreateMap<Vehiculo, VehiculoDTO>().ReverseMap();

            CreateMap<Zona, ZonaDTO>().ReverseMap();

            CreateMap<Producto, ProductoDTO>().ReverseMap();
        }
    }
}