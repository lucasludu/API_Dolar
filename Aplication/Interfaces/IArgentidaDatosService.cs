using Application.DTOs._cotizaciones.Response;
using Application.DTOs._feriado.Response;
using Application.DTOs._state.Response;

namespace Application.Interfaces
{
    public interface IArgentidaDatosService
    {
        Task<EstadoResponse> GetEstadoApiAsync();
        Task<List<CotizacionesResponse>> GetCotizacionesDolarAsync();
        Task<CotizacionesResponse?> GetCotizacionPorTipoYFechaAsync(string tipo, DateTime fecha);
        Task<List<FeriadoResponse>> GetFeriadosAsync(int year);

        Task<List<CotizacionesResponse>> GetCotizacionesDolarTodayAsync();
        Task<CotizacionesResponse?> GetCotizacionesDolarTodayByTypoAsync(string tipo);

        Task<List<CotizacionesResponse>> GetOtrasMonedasCotizaciones();
        Task<CotizacionesResponse> GetOtrasMonedasCotizacionesByTypoAsync(string typo);
    }
}
