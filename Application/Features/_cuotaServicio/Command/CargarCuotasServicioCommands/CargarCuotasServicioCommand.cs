using Application.DTOs._cuotaServicio.Request;
using Application.DTOs._cuotaServicio.Response;
using Application.Interfaces;
using Application.Specification._cuotaServicio;
using Application.Specification._cotizacionDolar;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Application.Exceptions;
using Application.Constants;

namespace Application.Features._cuotaServicio.Command.CargarCuotasServicioCommands
{
    public class CargarCuotasServicioCommand : IRequest<CuotaServicioResponse>
    {
        public CuotaServicioRequest Request { get; set; }

        public CargarCuotasServicioCommand(CuotaServicioRequest request)
        {
            Request = request;
        }
    }

    public class CargarCuotasServicioCommandHandler : IRequestHandler<CargarCuotasServicioCommand, CuotaServicioResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepositoryAsync<CuotaServicio> _cuotaServicioRepository;
        private readonly IRepositoryAsync<Servicio> _servicioRepository;
        private readonly IRepositoryAsync<TipoDolar> _tipoDolarRepository;
        private readonly IRepositoryAsync<CotizacionDolar> _cotizacionDolarRepository;
        private readonly IArgentidaDatosService _argentidaDatosService;
        private readonly IMapper _mapper;
        private readonly ILogger<CargarCuotasServicioCommand> _logger;

        public CargarCuotasServicioCommandHandler(
                IUnitOfWork unitOfWork,
                IRepositoryAsync<CuotaServicio> cuotaServicioRepository,
                IRepositoryAsync<Servicio> servicioRepository,
                IRepositoryAsync<TipoDolar> tipoDolarRepository,
                IRepositoryAsync<CotizacionDolar> cotizacionDolarRepository,
                IArgentidaDatosService argentidaDatosService,
                IMapper mapper,
                ILogger<CargarCuotasServicioCommand> logger)
        {
            _unitOfWork = unitOfWork;
            _cuotaServicioRepository = cuotaServicioRepository;
            _servicioRepository = servicioRepository;
            _tipoDolarRepository = tipoDolarRepository;
            _cotizacionDolarRepository = cotizacionDolarRepository;
            _argentidaDatosService = argentidaDatosService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CuotaServicioResponse> Handle(CargarCuotasServicioCommand request, CancellationToken cancellationToken)
        {
            #region Verificá la cuota
            var cuotaExistente = await this.VerificarCuotaExistenteAsync(request.Request.ServicioId, request.Request.NumeroCuota);
            if (cuotaExistente)
            {
                _logger.LogWarning("La cuota {NumeroCuota} ya existe para el servicio {ServicioId}", request.Request.NumeroCuota, request.Request.ServicioId);
                throw new ApiException("La cuota ya existe para el servicio especificado");
            }
            #endregion

            #region Obtengo el servicio
            var servicio = await this.ObtenerServicioAsync(request.Request.ServicioId);
            if (servicio == null)
            {
                _logger.LogWarning("Servicio con ID {ServicioId} no encontrado", request.Request.ServicioId);
                throw new ApiException("Servicio no encontrado");
            }
            #endregion

            #region Obtengo el tipo de dolar
            var tipoCotizacion = await this.ObtenerTipoDolarAsync(request.Request.CotizacionDolarId);
            if (tipoCotizacion == null)
            {
                _logger.LogWarning("Tipo de cotización con ID {CotizacionDolarId} no encontrado", request.Request.CotizacionDolarId);
                throw new ApiException("Tipo de cotización no encontrado");
            }
            #endregion

            var conectado = await _argentidaDatosService.GetEstadoApiAsync();
            if (!conectado.Estado.ToLower().Equals(EstadoApiConstants.Correcto.ToLower()))
            {
                _logger.LogWarning("El servicio de cotización de dólar no está disponible");
                throw new ApiException("El servicio de cotización de dólar no está disponible");
            }

            #region Obtengo la cotización (servicio externo)
            var cotizacionDolarService = (request.Request.FechaPago.ToString("yyyy/MM/dd") == DateTime.Now.ToString("yyyy/MM/dd"))
                ? await _argentidaDatosService.GetCotizacionesDolarTodayByTypoAsync(tipoCotizacion.Nombre)
                : await _argentidaDatosService.GetCotizacionPorTipoYFechaAsync(tipoCotizacion.Nombre.ToLower(), request.Request.FechaPago);

            _logger.LogInformation("Cotización obtenida para tipo {Tipo} y fecha {Fecha}: {@Cotizacion}", tipoCotizacion.Nombre, request.Request.FechaPago, cotizacionDolarService);

            if (cotizacionDolarService == null)
            {
                _logger.LogInformation("No se encontró una cotización para el tipo {Tipo} y fecha {Fecha}", tipoCotizacion.Nombre, request.Request.FechaPago);
                throw new ApiException("No se encontró una cotización para el tipo y fecha especificados");
            }
            #endregion

            #region Verifico que exista en la BD, si existe tomo la misma sino la agrego
            var cotizacionExistente = await this.ObtenerCotizacionDolarAsync(tipoCotizacion.Nombre.ToLower(), request.Request.FechaPago, cancellationToken);
            var cotizacionFinal = cotizacionExistente;

            if (cotizacionExistente == null)
            {
                cotizacionFinal = new CotizacionDolar(request.Request.FechaPago, tipoCotizacion.Id, cotizacionDolarService.Compra, cotizacionDolarService.Venta);
                cotizacionFinal = await _cotizacionDolarRepository.AddAsync(cotizacionFinal);

                _logger.LogInformation("Nueva cotización agregada a la base de datos: {@Cotizacion}", cotizacionFinal);
            }
            else
            {
                _logger.LogInformation("Cotización existente encontrada en la base de datos: {@Cotizacion}", cotizacionFinal);
            }
            #endregion

            decimal montoUsdCalc = (decimal)(request.Request.MontoARS / cotizacionFinal.Venta)!;

            var cuotaServicio = new CuotaServicio(
                request.Request.NumeroCuota,
                request.Request.FechaPago,
                request.Request.MontoARS,
                montoUsdCalc,
                servicio.Id,
                cotizacionFinal.Id
            );

            cuotaServicio = await _cuotaServicioRepository.AddAsync(cuotaServicio);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Nueva cuota de servicio agregada: {@CuotaServicio}", cuotaServicio);
            return _mapper.Map<CuotaServicioResponse>(cuotaServicio);
        }

        private async Task<bool> VerificarCuotaExistenteAsync(int servicioId, int numeroCuota)
        {
            var spec = new CuotaServicioSpecification(servicioId, numeroCuota);
            var cuotaExistente = await _cuotaServicioRepository.FirstOrDefaultAsync(spec);
            return cuotaExistente != null;
        }

        private async Task<Servicio?> ObtenerServicioAsync(int id)
        {
            return await _servicioRepository.GetByIdAsync(id);
        }

        private async Task<TipoDolar?> ObtenerTipoDolarAsync(int id)
        {
            return await _tipoDolarRepository.GetByIdAsync(id);
        }

        private async Task<CotizacionDolar?> ObtenerCotizacionDolarAsync(string tipo, DateTime fecha, CancellationToken cancellationToken)
        {
            var cotizacionDolarSpec = new CotizacionDolarSpecification(tipo, fecha);
            return await _cotizacionDolarRepository.FirstOrDefaultAsync(cotizacionDolarSpec, cancellationToken);
        }
    }
}
