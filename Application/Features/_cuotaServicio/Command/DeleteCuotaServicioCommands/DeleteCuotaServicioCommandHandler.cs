using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features._cuotaServicio.Command.DeleteCuotaServicioCommands
{
    public class DeleteCuotaServicioCommandHandler : IRequestHandler<DeleteCuotaServicioCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteCuotaServicioCommand> _logger;

        public DeleteCuotaServicioCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteCuotaServicioCommand> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteCuotaServicioCommand request, CancellationToken cancellationToken)
        {
            var cuotaServicio = await _unitOfWork.RepositoryAsync<CuotaServicio>().GetByIdAsync(request.Id);
            if (cuotaServicio == null)
            {
                _logger.LogWarning("La cuota no fue encontrada");
                throw new ApiException("Cuota de servicio no encontrada.");
            }
            await _unitOfWork.RepositoryAsync<CuotaServicio>().DeleteAsync(cuotaServicio);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("La cuota fue eliminada con éxito.");
            return true;
        }
    }
}
