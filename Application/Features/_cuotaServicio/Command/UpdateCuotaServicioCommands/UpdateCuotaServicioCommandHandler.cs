using Application.DTOs._cuotaServicio.Response;
using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features._cuotaServicio.Command.UpdateCuotaServicioCommands
{
    public class UpdateCuotaServicioCommandHandler : IRequestHandler<UpdateCuotaServicioCommand, Response<CuotaServicioResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICotizacionDolarManager _cotizacionDolarManager;
        private readonly ILogger<UpdateCuotaServicioCommand> _logger;

        public UpdateCuotaServicioCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateCuotaServicioCommand> logger, ICotizacionDolarManager cotizacionDolarManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _cotizacionDolarManager = cotizacionDolarManager;
        }

        public async Task<Response<CuotaServicioResponse>> Handle(UpdateCuotaServicioCommand request, CancellationToken cancellationToken)
        {
            var cuotaServicio = await _unitOfWork.RepositoryAsync<CuotaServicio>().GetByIdAsync(request.Id);
            if (cuotaServicio == null)
            {
                _logger.LogWarning("Cuota de servicio con ID {Id} no encontrada", request.Id);
                throw new ApiException("Cuota de servicio no encontrada.");
            }

            // Usamos el Manager para obtener/guardar la cotización correcta y obtener su ID de base de datos
            var cotizacionFinal = await _cotizacionDolarManager.ObtenerYGuardarCotizacionAsync(request.CuotaServicioDto.TipoDolar, request.CuotaServicioDto.FechaPago, cancellationToken);

            decimal montoARS = request.CuotaServicioDto.MontoARS;
            decimal montoUSD = request.CuotaServicioDto.MontoUSD;

            if (request.CuotaServicioDto.DeterminaCuotaPor == "USD")
            {
                montoARS = (decimal)(montoUSD * cotizacionFinal.Venta)!;
            }
            else // Default to ARS
            {
                montoUSD = (decimal)(montoARS / cotizacionFinal.Venta)!;
            }

            // Si el ServicioId es 0 o no se proporciona, mantenemos el original
            int servicioId = request.CuotaServicioDto.ServicioId == 0 ? cuotaServicio.ServicioId : request.CuotaServicioDto.ServicioId;

            // Si el ServicioId cambió, validamos que el nuevo servicio exista
            if (servicioId != cuotaServicio.ServicioId)
            {
                var servicio = await _unitOfWork.RepositoryAsync<Servicio>().GetByIdAsync(servicioId);
                if (servicio == null)
                {
                    _logger.LogWarning("Servicio con ID {ServicioId} no encontrado al intentar actualizar cuota", servicioId);
                    throw new ApiException("El servicio especificado no existe.");
                }
            }

            cuotaServicio.Update(
                request.CuotaServicioDto.NumeroCuota,
                request.CuotaServicioDto.FechaPago,
                montoARS,
                montoUSD,
                servicioId,
                cotizacionFinal.Id
            );

            await _unitOfWork.RepositoryAsync<CuotaServicio>().UpdateAsync(cuotaServicio);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("La cuota del servicio {ServicioId} fue modificada con éxito.", servicioId);
            return new Response<CuotaServicioResponse>(_mapper.Map<CuotaServicioResponse>(cuotaServicio));
        }
    }
}
