using Application.DTOs._servicio.Response;
using Application.Interfaces;
using Application.Specification._servicio;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features._servicio.Queries.GetServicioByNameQueries
{
    public class GetServicioByNameQueryHandler : IRequestHandler<GetServicioByNameQuery, ServicioResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetServicioByNameQuery> _logger;

        public GetServicioByNameQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetServicioByNameQuery> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServicioResponse> Handle(GetServicioByNameQuery request, CancellationToken cancellationToken)
        {
            var servicioSpec = new ServicioContainNameSpec(request.Name);
            var servicio = await _unitOfWork.RepositoryAsync<Servicio>().FirstOrDefaultAsync(servicioSpec, cancellationToken);

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
