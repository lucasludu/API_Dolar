using Application.Interfaces;
using Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;
using Domain.Entities;
using Application.Exceptions;

namespace Application.Features._cuotaServicio.Command.DeleteCuotaServicioCommands
{
    public class DeleteCuotaServicioCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public DeleteCuotaServicioCommand(int id)
        {
            Id = id;
        }
    }

    public class DeleteCuotaServicioCommandHandler : IRequestHandler<DeleteCuotaServicioCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepositoryAsync<CuotaServicio> _repository;
        private readonly ILogger<DeleteCuotaServicioCommand> _logger;

        public DeleteCuotaServicioCommandHandler(IUnitOfWork unitOfWork, IRepositoryAsync<CuotaServicio> repository, ILogger<DeleteCuotaServicioCommand> logger)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteCuotaServicioCommand request, CancellationToken cancellationToken)
        {
            var cuotaServicio = await _repository.GetByIdAsync(request.Id);
            if (cuotaServicio == null)
            {
                _logger.LogWarning("La cuota no fue encontrada");
                throw new ApiException("Cuota de servicio no encontrada.");
            }
            await _repository.DeleteAsync(cuotaServicio);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("La cuota fue eliminada con éxito.");
            return true;
        }
    }
}
