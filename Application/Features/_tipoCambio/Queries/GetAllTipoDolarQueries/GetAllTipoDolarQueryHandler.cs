using Application.DTOs._tipoDolar.Response;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features._tipoCambio.Queries.GetAllTipoDolarQueries
{
    public class GetAllTipoDolarQueryHandler : IRequestHandler<GetAllTipoDolarQuery, IEnumerable<TipoDolarResponse>>
    {
        private readonly ILogger<GetAllTipoDolarQuery> _logger;
        private readonly IArgentidaDatosService _argentidaDatosService;
        private readonly IMapper _mapper;

        public GetAllTipoDolarQueryHandler(ILogger<GetAllTipoDolarQuery> logger, IArgentidaDatosService argentidaDatosService, IMapper mapper)
        {
            _logger = logger;
            _argentidaDatosService = argentidaDatosService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TipoDolarResponse>> Handle(GetAllTipoDolarQuery request, CancellationToken cancellationToken)
        {
            var listTipoDolar = await _argentidaDatosService.GetCotizacionesDolarTodayAsync();

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
