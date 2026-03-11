using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features._cuotaServicio.Command.UpdateCuotaServicioCommands
{
    public class UpdateCuotaServicioCommandHandler : IRequestHandler<UpdateCuotaServicioCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCuotaServicioCommand> _logger;

        public UpdateCuotaServicioCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateCuotaServicioCommand> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateCuotaServicioCommand request, CancellationToken cancellationToken)
        {
            var cuotaServicio = await _unitOfWork.RepositoryAsync<CuotaServicio>().GetByIdAsync(request.Id);
            if (cuotaServicio == null)
            {
                _logger.LogWarning("Cuota de servicio con ID {Id} no encontrada", request.Id);
                throw new ApiException("Cuota de servicio no encontrada.");
            }

            cuotaServicio.Update(
                request.CuotaServicioDto.NumeroCuota,
                request.CuotaServicioDto.FechaPago,
                request.CuotaServicioDto.MontoARS,
                request.CuotaServicioDto.MontoUSD,
                request.CuotaServicioDto.ServicioId,
                request.CuotaServicioDto.CotizacionDolarId
            );

            await _unitOfWork.RepositoryAsync<CuotaServicio>().UpdateAsync(cuotaServicio);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("La cuota del servicio {ServicioId} fue modificada con éxito.", request.CuotaServicioDto.ServicioId);
            return true;
        }
    }
}
