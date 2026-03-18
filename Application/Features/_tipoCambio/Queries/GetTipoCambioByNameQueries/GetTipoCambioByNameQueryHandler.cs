using Application.DTOs._tipoDolar.Response;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features._tipoCambio.Queries.GetTipoCambioByNameQueries
{
    public class GetTipoCambioByNameQueryHandler : IRequestHandler<GetTipoCambioByNameQuery, TipoDolarResponse>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<GetTipoCambioByNameQuery> _logger;
        private readonly IArgentidaDatosService _argentidaDatosService;
        public GetTipoCambioByNameQueryHandler(IMapper mapper, ILogger<GetTipoCambioByNameQuery> logger, IArgentidaDatosService argentidaDatosService)
        {
            _mapper = mapper;
            _logger = logger;
            _argentidaDatosService = argentidaDatosService;
        }

        public async Task<TipoDolarResponse> Handle(GetTipoCambioByNameQuery request, CancellationToken cancellationToken)
        {
            var tipoDolar = await _argentidaDatosService.GetCotizacionesDolarTodayByTypoAsync(request.Name);
            if (tipoDolar != null)
            {
                _logger.LogInformation("Tipo de dólar encontrado exitosamente.");
                return _mapper.Map<TipoDolarResponse>(tipoDolar);
            }
            else
            {
                _logger.LogError("No se encontró el tipo de dólar.");
                throw new ApiException("No se encontró el tipo de dólar.");
            }
        }
    }
}
