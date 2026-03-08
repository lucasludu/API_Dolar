using Application.DTOs._tipoDolar.Response;
using Application.Interfaces;
using Application.Specification._tipoCambio;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features._tipoCambio.Queries.GetTipoCambioByNameQueries
{
    public class GetTipoCambioByNameQuery : IRequest<TipoDolarResponse>
    {
        public string Name { get; set; }
        public GetTipoCambioByNameQuery(string name)
        {
            Name = name;
        }
    }

    public class GetTipoCambioByNameQueryHandler : IRequestHandler<GetTipoCambioByNameQuery, TipoDolarResponse>
    {
        private readonly IRepositoryAsync<TipoDolar> _tipoDolarRepositoryAsync;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTipoCambioByNameQuery> _logger;

        public GetTipoCambioByNameQueryHandler(IRepositoryAsync<TipoDolar> tipoDolarRepositoryAsync, IMapper mapper, ILogger<GetTipoCambioByNameQuery> logger)
        {
            _tipoDolarRepositoryAsync = tipoDolarRepositoryAsync;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TipoDolarResponse> Handle(GetTipoCambioByNameQuery request, CancellationToken cancellationToken)
        {
            var tipoDolarSpec = new TipoDolarContainNameSpec(request.Name);
            var tipoDolar = await _tipoDolarRepositoryAsync.FirstOrDefaultAsync(tipoDolarSpec, cancellationToken);
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
