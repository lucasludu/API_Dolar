using Application.DTOs._tipoDolar.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features._tipoCambio.Queries.GetAllTipoDolarQueries
{
    public class GetAllTipoDolarQuery : IRequest<IEnumerable<TipoDolarResponse>>
    {
    }

    public class GetAllTipoDolarQueryHandler : IRequestHandler<GetAllTipoDolarQuery, IEnumerable<TipoDolarResponse>>
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

        public async Task<IEnumerable<TipoDolarResponse>> Handle(GetAllTipoDolarQuery request, CancellationToken cancellationToken)
        {
            var listTipoDolar = await _tipoDolarRepositoryAsync.ListAsync(cancellationToken);

            if (listTipoDolar.Any())
            {
                _logger.LogInformation("Lista de tipos de dólar obtenida exitosamente.");
                return _mapper.Map<IEnumerable<TipoDolarResponse>>(listTipoDolar);
            }
            
            _logger.LogWarning("No se encontraron tipos de dólar.");
            return Enumerable.Empty<TipoDolarResponse>();
        }
    }
}
