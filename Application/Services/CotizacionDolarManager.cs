using Application.Constants;
using Application.DTOs._cotizaciones.Response;
using Application.Exceptions;
using Application.Interfaces;
using Application.Specification._cotizacionDolar;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class CotizacionDolarManager : ICotizacionDolarManager
    {
        private readonly IArgentidaDatosService _argentidaDatosService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CotizacionDolarManager> _logger;

        public CotizacionDolarManager(
            IArgentidaDatosService argentidaDatosService,
            IUnitOfWork unitOfWork,
            ILogger<CotizacionDolarManager> logger,
            IMapper mapper)
        {
            _argentidaDatosService = argentidaDatosService;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<CotizacionesResponse> ObtenerYGuardarCotizacionAsync(string tipoDolar, DateTime fechaPago, CancellationToken cancellationToken)
        {
            // Verificamos estado del servicio
            var conectado = await _argentidaDatosService.GetEstadoApiAsync();
            if (!conectado.Estado.ToLower().Equals(EstadoApiConstants.Correcto.ToLower()))
            {
                _logger.LogWarning("El servicio de cotización de dólar no está disponible");
                throw new ApiException("El servicio de cotización de dólar no está disponible");
            }
            //return null;
            // Obtenemos la cotización del servicio externo
            var cotizacionDolarService = (fechaPago.ToString("yyyy/MM/dd") == DateTime.Now.ToString("yyyy/MM/dd"))
                ? await _argentidaDatosService.GetCotizacionesDolarTodayByTypoAsync(tipoDolar)
                : await _argentidaDatosService.GetCotizacionPorTipoYFechaAsync(tipoDolar.ToLower(), fechaPago);

            _logger.LogInformation("Cotización obtenida para tipo {Tipo} y fecha {Fecha}: {@Cotizacion}", tipoDolar, fechaPago, cotizacionDolarService);

            if (cotizacionDolarService == null)
            {
                _logger.LogInformation("No se encontró una cotización para el tipo {Tipo} y fecha {Fecha}", tipoDolar, fechaPago);
                throw new ApiException("No se encontró una cotización para el tipo y fecha especificados");
            }

            var cotizacionDolarSpec = new CotizacionDolarSpecification(tipoDolar.ToLower(), fechaPago);
            var cotizacionExistente = await _unitOfWork.RepositoryAsync<CotizacionDolar>().FirstOrDefaultAsync(cotizacionDolarSpec, cancellationToken);

            if (cotizacionExistente == null)
            {
                var nuevaCotizacion = _mapper.Map<CotizacionDolar>(cotizacionDolarService);
                await _unitOfWork.RepositoryAsync<CotizacionDolar>().AddAsync(nuevaCotizacion, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return _mapper.Map<CotizacionesResponse>(nuevaCotizacion);
            }
            
            return _mapper.Map<CotizacionesResponse>(cotizacionExistente);
        }
    }
}
