using Application.DTOs._tipoDolar.Response;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features._tipoCambio.Queries.GetAllTipoDolarQueries
{
    public class GetAllTipoDolarQuery : IRequest<Response<List<TipoDolarResponse>>>
    {

    }
    public class GetAllTipoDolarQueryHandler : IRequestHandler<GetAllTipoDolarQuery, Response<List<TipoDolarResponse>>>
    {
        private readonly IRepositoryAsync<TipoDolar> _tipoDolarRepositoryAsync;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllTipoDolarQuery> _logger;

        public GetAllTipoDolarQueryHandler(IRepositoryAsync<TipoDolar> tipoDolarRepositoryAsync, IMapper mapper, ILogger<GetAllTipoDolarQuery> logger)
        {
            _tipoDolarRepositoryAsync = tipoDolarRepositoryAsync;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<List<TipoDolarResponse>>> Handle(GetAllTipoDolarQuery request, CancellationToken cancellationToken)
        {
            var listTipoDolar = await _tipoDolarRepositoryAsync.ListAsync();
            
            if (listTipoDolar.Any())
            {
                _logger.LogInformation("Lista de tipos de dólar obtenida exitosamente.");
                return Response<List<TipoDolarResponse>>.SuccessResponse(_mapper.Map<List<TipoDolarResponse>>(listTipoDolar), "Lista de tipos de dólar obtenida exitosamente.");
            }
            else
            {
                _logger.LogError("No se encontraron tipos de dólar.");
                return Response<List<TipoDolarResponse>>.FailResponse("No se encontraron tipos de dólar.");
            }
        }
    }
}
