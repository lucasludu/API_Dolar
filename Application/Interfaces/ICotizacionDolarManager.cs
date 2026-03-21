using Application.DTOs._cotizaciones.Response;

namespace Application.Interfaces
{
    public interface ICotizacionDolarManager
    {
        Task<CotizacionesResponse> ObtenerYGuardarCotizacionAsync(string tipoDolar, DateTime fechaPago, CancellationToken cancellationToken);
    }
}
