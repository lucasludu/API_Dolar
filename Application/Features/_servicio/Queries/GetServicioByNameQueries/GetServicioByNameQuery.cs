using Application.DTOs._servicio.Response;
using Application.Interfaces;
using Application.Specification._servicio;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features._servicio.Queries.GetServicioByNameQueries
{
    public class GetServicioByNameQuery : IRequest<ServicioResponse>
    {
        public string Name { get; set; }

        public GetServicioByNameQuery(string name)
        {
            Name = name;
        }
    }

    public class GetServicioByNameQueryHandler : IRequestHandler<GetServicioByNameQuery, ServicioResponse>
    {
        private readonly IRepositoryAsync<Servicio> _servicioRepositoryAsync;
        private readonly IMapper _mapper;
        private readonly ILogger<GetServicioByNameQuery> _logger;

        public GetServicioByNameQueryHandler(IRepositoryAsync<Servicio> servicioRepositoryAsync, IMapper mapper, ILogger<GetServicioByNameQuery> logger)
        {
            _servicioRepositoryAsync = servicioRepositoryAsync;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServicioResponse> Handle(GetServicioByNameQuery request, CancellationToken cancellationToken)
        {
            var servicioSpec = new ServicioContainNameSpec(request.Name);
            var servicio = await _servicioRepositoryAsync.FirstOrDefaultAsync(servicioSpec, cancellationToken);

            if (servicio != null)
            {
                _logger.LogInformation("Servicio encontrado: {ServiceName}", request.Name);
                return _mapper.Map<ServicioResponse>(servicio);
            }
            
            _logger.LogWarning("No se encontro el nombre del servicio: {ServiceName}", request.Name);
            throw new Application.Exceptions.ApiException("No se encontro el nombre del servicio");
        }
    }
}
