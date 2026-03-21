using Application.Interfaces;
using Application.Specification._servicio;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features._servicio.Command.DeleteServicioCommands
{
    public record DeleteServicioCommandHandler : IRequestHandler<DeleteServicioCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteServicioCommandHandler> _logger;
        public DeleteServicioCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteServicioCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(DeleteServicioCommand request, CancellationToken cancellationToken)
        {
            //var servicio = await _unitOfWork.ReadRepositoryAsync<Servicio>().GetByIdAsync(request.id);

            var servicioSpec = new GetServicioByIdSpec(request.id);
            var servicio = await _unitOfWork.RepositoryAsync<Servicio>().FirstOrDefaultAsync(servicioSpec);

            if (servicio == null)
            {
                _logger.LogWarning("Servicio with id {Id} not found", request.id);
                return new Response<bool>
                {
                    Succeeded = false,
                    Message = $"Servicio with id {request.id} not found",
                    Data = false
                };
            }

            await _unitOfWork.RepositoryAsync<Servicio>().DeleteAsync(servicio);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Servicio with id {Id} deleted successfully", request.id);
            return new Response<bool>
            {
                Succeeded = true,
                Message = $"Servicio with id {request.id} deleted successfully",
                Data = true
            };
        }
    }
}
