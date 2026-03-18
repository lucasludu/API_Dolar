using Application.DTOs._servicio.Response;
using Application.Exceptions;
using Application.Interfaces;
using Application.Specification._servicio;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features._servicio.Command.CreateServicioCommands
{
    public class CreateServicioCommandHandler : IRequestHandler<CreateServicioCommand, Response<ServicioResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateServicioCommand> _logger;

        public CreateServicioCommandHandler(
                IUnitOfWork unitOfWork,
                IMapper mapper,
                ILogger<CreateServicioCommand> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<ServicioResponse>> Handle(CreateServicioCommand request, CancellationToken cancellationToken)
        {
            var servicioSpec = new ServicioByNameSpec(request.Request.Nombre);
            var servicio = await _unitOfWork.RepositoryAsync<Servicio>().FirstOrDefaultAsync(servicioSpec);

            if (servicio != null)
            {
                _logger.LogWarning("El servicio ya existe");
                throw new ApiException("El servicio ya existe.");
            }

            var nuevoServicio = new Servicio(
                request.Request.Nombre,
                request.Request.FechaInicio,
                request.Request.CantidadCuotas,
                request.Request.Activo
            );

            var createdServicio = await _unitOfWork.RepositoryAsync<Servicio>().AddAsync(nuevoServicio);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (createdServicio != null)
            {
                _logger.LogInformation("Servicio creado exitosamente.");
                var mapped = _mapper.Map<ServicioResponse>(createdServicio);
                return new Response<ServicioResponse>(mapped);
            }
            else
            {
                _logger.LogError("No se pudo crear el servicio.");
                return new Response<ServicioResponse>
                {
                    Succeeded = false,
                    Message = "No se pudo crear el servicio."
                };
            }
        }
    }
}
