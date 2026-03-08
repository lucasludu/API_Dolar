using Application.DTOs._servicio.Response;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Feautures._servicio.Queries.GetAllServiciosQueries
{
    public class GetAllServiciosQuery : IRequest<Response<List<ServicioResponse>>>
    {
    }
    public class GetAllServiciosQueryHandler : IRequestHandler<GetAllServiciosQuery, Response<List<ServicioResponse>>>
    {
        private readonly IRepositoryAsync<Servicio> _servicioRepositoryAsync;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllServiciosQuery> _logger;

        public GetAllServiciosQueryHandler(IRepositoryAsync<Servicio> servicioRepositoryAsync, IMapper mapper, ILogger<GetAllServiciosQuery> logger)
        {
            _servicioRepositoryAsync = servicioRepositoryAsync;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<List<ServicioResponse>>> Handle(GetAllServiciosQuery request, CancellationToken cancellationToken)
        {
            var listaServicios = await _servicioRepositoryAsync.ListAsync(cancellationToken);

            if (listaServicios.Any())
            {
                _logger.LogInformation("Se encontraron {Count} servicios", listaServicios.Count);
                return Response<List<ServicioResponse>>.SuccessResponse(_mapper.Map<List<ServicioResponse>>(listaServicios));
            }
            else
            {
                _logger.LogWarning("No se encontraron servicios");
                return Response<List<ServicioResponse>>.SuccessResponse(null, "No hay servicios");
            }
        }
    }
}
