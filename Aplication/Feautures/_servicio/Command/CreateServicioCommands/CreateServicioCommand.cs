using Application.DTOs._servicio.Request;
using Application.DTOs._servicio.Response;
using Application.Interfaces;
using Application.Specification._servicio;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Feautures._servicio.Command.CreateServicioCommands
{
    public class CreateServicioCommand : IRequest<Response<ServicioResponse>>
    {
        public ServicioRequest Request { get; set; }

        public CreateServicioCommand(ServicioRequest request)
        {
            Request = request;
        }
    }

    public class CreateServicioCommandHandler : IRequestHandler<CreateServicioCommand, Response<ServicioResponse>>
    {
        private readonly IRepositoryAsync<Servicio> _servicioRepositoryAsync;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateServicioCommand> _logger;

        public CreateServicioCommandHandler(
                IRepositoryAsync<Servicio> servicioRepositoryAsync,
                IMapper mapper
,
                ILogger<CreateServicioCommand> logger)
        {
            _servicioRepositoryAsync = servicioRepositoryAsync;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<ServicioResponse>> Handle(CreateServicioCommand request, CancellationToken cancellationToken)
        {
            var servicioSpec = new ServicioByNameSpec(request.Request.Nombre);
            var servicio = await _servicioRepositoryAsync.FirstOrDefaultAsync(servicioSpec);

            if (servicio != null)
            {
                _logger.LogWarning("El servicio ya existe");
                return Response<ServicioResponse>.FailResponse("El servicio ya existe.");
            }

            var nuevoServicio = _mapper.Map<Servicio>(request.Request);
            var createdServicio = await _servicioRepositoryAsync.AddAsync(nuevoServicio);

            if (createdServicio != null)
            {
                _logger.LogInformation("Servicio creado exitosamente.");
                return Response<ServicioResponse>.SuccessResponse(_mapper.Map<ServicioResponse>(createdServicio), "Servicio creado exitosamente.");
            }
            else
            {
                _logger.LogError("No se pudo crear el servicio.");
                return Response<ServicioResponse>.FailResponse("No se pudo crear el servicio.");
            }
        }
    }
}
