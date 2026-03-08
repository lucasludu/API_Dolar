using Application.Constants;
using Application.DTOs._cuotaServicio.Request;
using Application.DTOs._cuotaServicio.Response;
using Application.Interfaces;
using Application.Specification._cotizacionDolar;
using Application.Specification._cuotaServicio;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features._cuotaServicio.Command.CargarCuotasServicioCommands
{
    public class CargarCuotasServicioCommand : IRequest<Response<CuotaServicioResponse>>
    {
        public CuotaServicioRequest Request { get; set; }

        public CargarCuotasServicioCommand(CuotaServicioRequest request)
        {
            Request = request;
        }
    }

    public class CargarCuotasServicioCommandHandler : IRequestHandler<CargarCuotasServicioCommand, Response<CuotaServicioResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IArgentidaDatosService _argentidaDatosService;
        private readonly IMapper _mapper;
        private readonly ILogger<CargarCuotasServicioCommand> _logger;

        public CargarCuotasServicioCommandHandler(
                IUnitOfWork unitOfWork,
                IArgentidaDatosService argentidaDatosService,
                IMapper mapper,
                ILogger<CargarCuotasServicioCommand> logger)
        {
            _unitOfWork = unitOfWork;
            _argentidaDatosService = argentidaDatosService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<CuotaServicioResponse>> Handle(CargarCuotasServicioCommand request, CancellationToken cancellationToken)
        {
            try
            {
                #region Verificá la cuota
                var cuotaExistente = await this.VerificarCuotaExistenteAsync(request.Request.ServicioId, request.Request.NumeroCuota);
                if (cuotaExistente)
                {
                    _logger.LogWarning("La cuota {NumeroCuota} ya existe para el servicio {ServicioId}", request.Request.NumeroCuota, request.Request.ServicioId);
                    return Response<CuotaServicioResponse>.FailResponse("La cuota ya existe para el servicio especificado");
                }
                #endregion

                #region Obtengo el servicio
                var servicio = await this.ObtenerServicioAsync(request.Request.ServicioId);
                if (servicio == null)
                {
                    _logger.LogWarning("Servicio con ID {ServicioId} no encontrado", request.Request.ServicioId);
                    return Response<CuotaServicioResponse>.FailResponse("Servicio no encontrado");
                }
                #endregion

                #region Obtengo el tipo de dolar
                var tipoCotizacion = await this.ObtenerTipoDolarAsync(request.Request.CotizacionDolarId);
                if (tipoCotizacion == null)
                {
                    _logger.LogWarning("Tipo de cotización con ID {CotizacionDolarId} no encontrado", request.Request.CotizacionDolarId);
                    return Response<CuotaServicioResponse>.FailResponse("Tipo de cotización no encontrado");
                }
                #endregion

                var conectado = await _argentidaDatosService.GetEstadoApiAsync();
                if (conectado.Estado.ToLower().Equals(EstadoApiConstants.Correcto.ToLower()))
                {
                    #region Obtengo la cotización (servicio externo)
                    var cotizacionDolarService = (request.Request.FechaPago.ToString("yyyy/MM/dd") == DateTime.Now.ToString("yyyy/MM/dd"))
                        ? await _argentidaDatosService.GetCotizacionesDolarTodayByTypoAsync(tipoCotizacion.Nombre)
                        : await _argentidaDatosService.GetCotizacionPorTipoYFechaAsync(tipoCotizacion.Nombre.ToLower(), request.Request.FechaPago);

                    _logger.LogInformation("Cotización obtenida para tipo {Tipo} y fecha {Fecha}: {@Cotizacion}", tipoCotizacion.Nombre, request.Request.FechaPago, cotizacionDolarService);

                    if (cotizacionDolarService == null)
                    {
                        _logger.LogInformation("No se encontró una cotización para el tipo {Tipo} y fecha {Fecha}", tipoCotizacion.Nombre, request.Request.FechaPago);
                        return Response<CuotaServicioResponse>.FailResponse("No se encontró una cotización para el tipo y fecha especificados");
                    }
                    #endregion

                    #region Verifico que exista en la BD, si existe tomo la misma sino la agrego
                    var cotizacionExistente = await this.ObtenerCotizacionDolarAsync(tipoCotizacion.Nombre.ToLower(), request.Request.FechaPago, cancellationToken);
                    var cotizacionFinal = cotizacionExistente;

                    if(cotizacionExistente == null)
                    {
                        var cotizacion = _mapper.Map<CotizacionDolar>(cotizacionDolarService);
                        cotizacion.TipoDolarId = tipoCotizacion.Id;
                        cotizacion.TipoDolar = null;
                        cotizacionFinal = await _unitOfWork.CotizacionDolarRepository.AddAsync(cotizacion);

                        _logger.LogInformation("Nueva cotización agregada a la base de datos: {@Cotizacion}", cotizacionFinal);
                    }
                    else
                    {
                        _logger.LogInformation("Cotización existente encontrada en la base de datos: {@Cotizacion}", cotizacionFinal);
                    }
                    #endregion

                    var _cuotaServicio = new CuotaServicioRequest
                    {
                        NumeroCuota = request.Request.NumeroCuota,
                        FechaPago = request.Request.FechaPago,
                        MontoARS = request.Request.MontoARS,
                        ServicioId = servicio.Id,
                        CotizacionDolarId = cotizacionFinal!.Id
                    };
                    var _cuotaServicioMapped = _mapper.Map<CuotaServicio>(_cuotaServicio);
                    _cuotaServicioMapped.MontoUSD = (decimal)(request.Request.MontoARS / cotizacionFinal.Venta)!;
                    var cuotaServicio = await _unitOfWork.CuotaServicioRepository.AddAsync(_cuotaServicioMapped);
                    cuotaServicio.Servicio = servicio;
                    cuotaServicio.CotizacionDolar = cotizacionFinal;
                    cuotaServicio.CotizacionDolar.TipoDolar = tipoCotizacion;

                    _logger.LogInformation("Nueva cuota de servicio agregada: {@CuotaServicio}", cuotaServicio);
                    return Response<CuotaServicioResponse>.SuccessResponse(_mapper.Map<CuotaServicioResponse>(cuotaServicio), "Cuota de servicio cargada exitosamente");
                }
                _logger.LogWarning("El servicio de cotización de dólar no está disponible");
                return Response<CuotaServicioResponse>.FailResponse("El servicio de cotización de dólar no está disponible");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar la cuota de servicio: {Message}", ex.Message);
                return Response<CuotaServicioResponse>.FailResponse($"Error al cargar la cuota de servicio: {ex.Message}");
            }
        }


        private async Task<bool> VerificarCuotaExistenteAsync(int servicioId, int numeroCuota)
        {
            var spec = new CuotaServicioSpecification(servicioId, numeroCuota);
            var cuotaExistente = await _unitOfWork.CuotaServicioRepository.FirstOrDefaultAsync(spec);
            return cuotaExistente != null;
        }

        private async Task<Servicio?> ObtenerServicioAsync(int id)
        {
            var servicio = await _unitOfWork.ServicioRepository.GetByIdAsync(id);
            return servicio;
        }

        private async Task<TipoDolar?> ObtenerTipoDolarAsync(int id)
        {
            var tipoDolar = await _unitOfWork.TipoDolarRepository.GetByIdAsync(id);
            return tipoDolar;
        }

        private async Task<CotizacionDolar?> ObtenerCotizacionDolarAsync(string tipo, DateTime fecha, CancellationToken cancellationToken)
        {
            var cotizacionDolarSpec = new CotizacionDolarSpecification(tipo, fecha);
            var cotizacionDolar = await _unitOfWork.CotizacionDolarRepository.FirstOrDefaultAsync(cotizacionDolarSpec, cancellationToken);
            return cotizacionDolar;
        }

    }
}
