using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features._servicio.Command.UpdateServicioCommands
{
    public class UpdateServicioCommandHandler : IRequestHandler<UpdateServicioCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateServicioCommandHandler> _logger;

        public UpdateServicioCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateServicioCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(UpdateServicioCommand request, CancellationToken cancellationToken)
        {
            // 1. Buscamos el servicio original
            var servicioExistente = await _unitOfWork.ReadRepositoryAsync<Servicio>().GetByIdAsync(request.id, cancellationToken);

            if (servicioExistente == null)
            {
                _logger.LogWarning("Servicio with id {Id} not found.", request.id);
                return new Response<bool>(false, "No se encontró el servicio");
            }

            // 2. CORRECCIÓN: Mapear desde el Request HACIA el objeto existente
            // Esto mantiene la referencia que Entity Framework ya está siguiendo
            _mapper.Map(request.Request, servicioExistente);

            // 3. Persistir
            await _unitOfWork.RepositoryAsync<Servicio>().UpdateAsync(servicioExistente, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Servicio with id {Id} updated successfully.", request.id);
            return new Response<bool>(true);
        }
    }
}
