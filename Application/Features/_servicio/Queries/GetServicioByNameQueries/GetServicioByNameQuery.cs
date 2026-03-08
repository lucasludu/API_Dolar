using Application.DTOs._servicio.Response;
using Application.Interfaces;
using Application.Specification._servicio;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features._servicio.Queries.GetServicioByNameQueries
{
    public class GetServicioByNameQuery : IRequest<Response<ServicioResponse>>
    {
        public string Name { get; set; }

        public GetServicioByNameQuery(string name)
        {
            Name = name;
        }
    }

    public class GetServicioByNameQueryHandler : IRequestHandler<GetServicioByNameQuery, Response<ServicioResponse>>
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


        public async Task<Response<ServicioResponse>> Handle(GetServicioByNameQuery request, CancellationToken cancellationToken)
        {
            var servicioSpec = new ServicioContainNameSpec(request.Name);
            var servicio = await _servicioRepositoryAsync.FirstOrDefaultAsync(servicioSpec, cancellationToken);

            if (servicio != null)
            {
                _logger.LogInformation("Servicio encontrado: {ServiceName}", request.Name);
                return Response<ServicioResponse>.SuccessResponse(_mapper.Map<ServicioResponse>(servicio));
            }
            else
            {
                _logger.LogError("No se encontro el nombre del serivicio: {ServiceName}", request.Name);
                return Response<ServicioResponse>.FailResponse("No se encontro el nombre del serivicio");
            }
        }
    }
}
