using Application.DTOs._cuotaServicio.Response;
using Application.Interfaces;
using Application.Specification._cuotaServicio;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Application.Features._cuotaServicio.Queries.GetAllCuotasServicioQueries
{
    public class GetAllCuotasServicioQuery : IRequest<PagedResponse<List<CuotaServicioResponse>>>
    {
        public GetAllCuotasServicioParameters Parameters { get; set; }

        public GetAllCuotasServicioQuery(GetAllCuotasServicioParameters parameters)
        {
            Parameters = parameters;
        }
    }

    public class GetAllCuotasServicioQueryHandler : IRequestHandler<GetAllCuotasServicioQuery, PagedResponse<List<CuotaServicioResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllCuotasServicioQuery> _logger;
        private readonly IMapper _mapper;

        public GetAllCuotasServicioQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllCuotasServicioQuery> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedResponse<List<CuotaServicioResponse>>> Handle(GetAllCuotasServicioQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando consulta de cuotas de servicio con parámetros: {@Parameters}", request.Parameters);

            var cuotaServicioSpec = new CuotaServicioPagedSpecification(request.Parameters);
            var cuotasServicio = await _unitOfWork.CuotaServicioRepository.ListAsync(cuotaServicioSpec);
            var total = await _unitOfWork.CuotaServicioRepository.CountAsync(cuotaServicioSpec);
            var data = _mapper.Map<List<CuotaServicioResponse>>(cuotasServicio);

            _logger.LogInformation("Consulta finalizada. Total registros: {Total}", total);

            return new PagedResponse<List<CuotaServicioResponse>>(data, request.Parameters.PageNumber, request.Parameters.PageSize, total);
        }
    }
}
