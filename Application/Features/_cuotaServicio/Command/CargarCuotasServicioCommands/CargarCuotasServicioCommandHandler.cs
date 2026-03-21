using Application.DTOs._cuotaServicio.Response;
using Application.Exceptions;
using Application.Features._cuotaServicio.Events.CuotaServicioCreadaEvents;
using Application.Interfaces;
using Application.Specification._cuotaServicio;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features._cuotaServicio.Command.CargarCuotasServicioCommands
{
    public class CargarCuotasServicioCommandHandler : IRequestHandler<CargarCuotasServicioCommand, Response<CuotaServicioResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICotizacionDolarManager _cotizacionDolarManager;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<CargarCuotasServicioCommand> _logger;
        private readonly IArgentidaDatosService _argentinaDatosService;

        public CargarCuotasServicioCommandHandler(
                IUnitOfWork unitOfWork,
                ICotizacionDolarManager cotizacionDolarManager,
                IMediator mediator,
                IMapper mapper,
                ILogger<CargarCuotasServicioCommand> logger,
                IArgentidaDatosService argentinaDatosService)
        {
            _unitOfWork = unitOfWork;
            _cotizacionDolarManager = cotizacionDolarManager;
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
            _argentinaDatosService = argentinaDatosService;
        }

        public async Task<Response<CuotaServicioResponse>> Handle(CargarCuotasServicioCommand request, CancellationToken cancellationToken)
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

            #region Obtengo y guardo la cotizacion (Manager)
            var cotizacionFinal = await _cotizacionDolarManager.ObtenerYGuardarCotizacionAsync(request.Request.TipoDolar, request.Request.FechaPago, cancellationToken);
            #endregion

            decimal montoARS = request.Request.MontoARS;
            decimal montoUSD = request.Request.MontoUSD;

            if (request.Request.DeterminaCuotaPor == "USD")
            {
                montoARS = (decimal)(montoUSD * cotizacionFinal.Venta)!;
            }
            else // Default to ARS
            {
                montoUSD = (decimal)(montoARS / cotizacionFinal.Venta)!;
            }

            var cuotaServicio = new CuotaServicio(
                request.Request.NumeroCuota,
                request.Request.FechaPago,
                montoARS,
                montoUSD,
                servicio.Id,
                cotizacionFinal.Id
            );

            cuotaServicio = await _unitOfWork.RepositoryAsync<CuotaServicio>().AddAsync(cuotaServicio);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Publicamos evento de dominio (para separar lógica y cumplir SRP)
            await _mediator.Publish(new CuotaServicioCreadaEvent(cuotaServicio.Id), cancellationToken);

            _logger.LogInformation("Nueva cuota de servicio agregada: {@CuotaServicio}", cuotaServicio);
            var mapped = _mapper.Map<CuotaServicioResponse>(cuotaServicio);
            return new Response<CuotaServicioResponse>(mapped);
        }

        #region MÉTODOS PRIVADOS

        private async Task<bool> VerificarCuotaExistenteAsync(int servicioId, int numeroCuota)
        {
            var spec = new CuotaServicioSpecification(servicioId, numeroCuota);
            var cuotaExistente = await _unitOfWork.RepositoryAsync<CuotaServicio>().FirstOrDefaultAsync(spec);
            return cuotaExistente != null;
        }

        private async Task<Servicio?> ObtenerServicioAsync(int id)
        {
            return await _unitOfWork.RepositoryAsync<Servicio>().GetByIdAsync(id);
        }

        #endregion

    }
}
