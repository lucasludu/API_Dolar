using Application.DTOs._cuotaServicio.Request;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features._cuotaServicio.Command.UpdateCuotaServicioCommands
{
    public class UpdateCuotaServicioCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }
        public CuotaServicioRequest CuotaServicioDto { get; set; }
        public UpdateCuotaServicioCommand(int id, CuotaServicioRequest cuotaServicioDto)
        {
            Id = id;
            CuotaServicioDto = cuotaServicioDto;
        }
    }

    public class UpdateCuotaServicioCommandHandler : IRequestHandler<UpdateCuotaServicioCommand, Response<bool>>
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

        public async Task<Response<bool>> Handle(UpdateCuotaServicioCommand request, CancellationToken cancellationToken)
        {
            var cuotaServicio = await _unitOfWork.CuotaServicioRepository.GetByIdAsync(request.Id);
            if (cuotaServicio == null)
            {
                _logger.LogWarning("Servicio con ID {ServicioId} no encontrado", request.CuotaServicioDto.ServicioId);
                return new Response<bool>(false, "Cuota de servicio no encontrada.");
            }
            _mapper.Map(request.CuotaServicioDto, cuotaServicio);
            await _unitOfWork.CuotaServicioRepository.UpdateAsync(cuotaServicio);
            _logger.LogInformation("La cuota del servicio {ServicioId} fue modificada con éxito.", request.CuotaServicioDto.ServicioId);
            return new Response<bool>(true, "Cuota de servicio actualizada correctamente.");
        }
    }
}
