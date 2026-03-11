using Application.DTOs._cuotaServicio.Request;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Domain.Entities;
using Application.Exceptions;

namespace Application.Features._cuotaServicio.Command.UpdateCuotaServicioCommands
{
    public class UpdateCuotaServicioCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public CuotaServicioRequest CuotaServicioDto { get; set; }
        public UpdateCuotaServicioCommand(int id, CuotaServicioRequest cuotaServicioDto)
        {
            Id = id;
            CuotaServicioDto = cuotaServicioDto;
        }
    }

    public class UpdateCuotaServicioCommandHandler : IRequestHandler<UpdateCuotaServicioCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepositoryAsync<CuotaServicio> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCuotaServicioCommand> _logger;

        public UpdateCuotaServicioCommandHandler(IUnitOfWork unitOfWork, IRepositoryAsync<CuotaServicio> repository, IMapper mapper, ILogger<UpdateCuotaServicioCommand> logger)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateCuotaServicioCommand request, CancellationToken cancellationToken)
        {
            var cuotaServicio = await _repository.GetByIdAsync(request.Id);
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

            await _repository.UpdateAsync(cuotaServicio);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("La cuota del servicio {ServicioId} fue modificada con éxito.", request.CuotaServicioDto.ServicioId);
            return true;
        }
    }
}
