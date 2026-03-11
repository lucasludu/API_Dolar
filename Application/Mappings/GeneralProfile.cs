using Application.DTOs._cotizaciones.Response;
using Application.DTOs._cuotaServicio.Request;
using Application.DTOs._cuotaServicio.Response;
using Application.DTOs._servicio.Request;
using Application.DTOs._servicio.Response;
using Application.DTOs._tipoDolar.Request;
using Application.DTOs._tipoDolar.Response;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            // SERVICIO
            CreateMap<Servicio, ServicioResponse>();

            // TIPO DE DOLAR
            CreateMap<TipoDolar, TipoDolarResponse>();

            // COTIZACION DOLAR
            CreateMap<CotizacionDolar, CotizacionesResponse>()
                .ForMember(dest => dest.Compra, opt => opt.MapFrom(src => src.Compra))
                .ForMember(dest => dest.Venta, opt => opt.MapFrom(src => src.Venta))
                .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => src.Fecha))
                .ForMember(dest => dest.Casa, opt => opt.MapFrom(src => src.TipoDolar.Nombre));

            // CUOTA SERVICIO
            CreateMap<CuotaServicio, CuotaServicioResponse>()
                .ForMember(dest => dest.Servicio, opt => opt.MapFrom(src => src.Servicio.Nombre))
                .ForMember(dest => dest.DolarVenta, opt => opt.MapFrom(src => src.CotizacionDolar.Venta))
                .ForMember(dest => dest.DolarCompra, opt => opt.MapFrom(src => src.CotizacionDolar.Compra))
                .ForMember(dest => dest.TipoDolar, opt => opt.MapFrom(src => src.CotizacionDolar.TipoDolar.Nombre));
        }
    }
}
