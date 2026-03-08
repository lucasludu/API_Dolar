using Application.Interfaces;
using Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Feautures._cuotaServicio.Command.DeleteCuotaServicioCommands
{
    public class DeleteCuotaServicioCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }
        public DeleteCuotaServicioCommand(int id)
        {
            Id = id;
        }
    }

    public class DeleteCuotaServicioCommandHandler : IRequestHandler<DeleteCuotaServicioCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteCuotaServicioCommand> _logger;

        public DeleteCuotaServicioCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteCuotaServicioCommand> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(DeleteCuotaServicioCommand request, CancellationToken cancellationToken)
        {
            var cuotaServicio = await _unitOfWork.CuotaServicioRepository.GetByIdAsync(request.Id);
            if (cuotaServicio == null)
            {
                _logger.LogWarning("La cuota no fue encontrada");
                return new Response<bool>(false, "Cuota de servicio no encontrada.");
            }
            await _unitOfWork.CuotaServicioRepository.DeleteAsync(cuotaServicio);

            _logger.LogInformation("La cuota fue eliminada con éxito.");
            return new Response<bool>(true, "Cuota de servicio eliminada correctamente.");
        }
    }
}
