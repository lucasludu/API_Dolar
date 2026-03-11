using Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICotizacionDolarManager
    {
        Task<CotizacionDolar> ObtenerYGuardarCotizacionAsync(TipoDolar tipoCotizacion, DateTime fechaPago, CancellationToken cancellationToken);
    }
}
