using Application.Constants;
using Application.Exceptions;
using Application.Interfaces;
using Application.Specification._cotizacionDolar;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CotizacionDolarManager : ICotizacionDolarManager
    {
        private readonly IArgentidaDatosService _argentidaDatosService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CotizacionDolarManager> _logger;

        public CotizacionDolarManager(
            IArgentidaDatosService argentidaDatosService,
            IUnitOfWork unitOfWork,
            ILogger<CotizacionDolarManager> logger)
        {
            _argentidaDatosService = argentidaDatosService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<CotizacionDolar> ObtenerYGuardarCotizacionAsync(TipoDolar tipoCotizacion, DateTime fechaPago, CancellationToken cancellationToken)
        {
            // Verificamos estado del servicio
            var conectado = await _argentidaDatosService.GetEstadoApiAsync();
            if (!conectado.Estado.ToLower().Equals(EstadoApiConstants.Correcto.ToLower()))
            {
                _logger.LogWarning("El servicio de cotización de dólar no está disponible");
                throw new ApiException("El servicio de cotización de dólar no está disponible");
            }

            // Obtenemos la cotización del servicio externo
            var cotizacionDolarService = (fechaPago.ToString("yyyy/MM/dd") == DateTime.Now.ToString("yyyy/MM/dd"))
                ? await _argentidaDatosService.GetCotizacionesDolarTodayByTypoAsync(tipoCotizacion.Nombre)
                : await _argentidaDatosService.GetCotizacionPorTipoYFechaAsync(tipoCotizacion.Nombre.ToLower(), fechaPago);

            _logger.LogInformation("Cotización obtenida para tipo {Tipo} y fecha {Fecha}: {@Cotizacion}", tipoCotizacion.Nombre, fechaPago, cotizacionDolarService);

            if (cotizacionDolarService == null)
            {
                _logger.LogInformation("No se encontró una cotización para el tipo {Tipo} y fecha {Fecha}", tipoCotizacion.Nombre, fechaPago);
                throw new ApiException("No se encontró una cotización para el tipo y fecha especificados");
            }

            // Verificamos si existe en la BD, sino la agregamos
            var cotizacionDolarSpec = new CotizacionDolarSpecification(tipoCotizacion.Nombre.ToLower(), fechaPago);
            var cotizacionExistente = await _unitOfWork.RepositoryAsync<CotizacionDolar>().FirstOrDefaultAsync(cotizacionDolarSpec, cancellationToken);
            
            var cotizacionFinal = cotizacionExistente;

            if (cotizacionExistente == null)
            {
                cotizacionFinal = new CotizacionDolar(fechaPago, tipoCotizacion.Id, cotizacionDolarService.Compra, cotizacionDolarService.Venta);
                cotizacionFinal = await _unitOfWork.RepositoryAsync<CotizacionDolar>().AddAsync(cotizacionFinal);

                _logger.LogInformation("Nueva cotización agregada a la base de datos: {@Cotizacion}", cotizacionFinal);
            }
            else
            {
                _logger.LogInformation("Cotización existente encontrada en la base de datos: {@Cotizacion}", cotizacionFinal);
            }

            return cotizacionFinal;
        }
    }
}
