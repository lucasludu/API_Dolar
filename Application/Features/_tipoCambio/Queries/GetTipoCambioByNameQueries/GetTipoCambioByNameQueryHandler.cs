using Application.DTOs._tipoDolar.Response;
using Application.Exceptions;
using Application.Interfaces;
using Application.Specification._tipoCambio;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features._tipoCambio.Queries.GetTipoCambioByNameQueries
{
    public class GetTipoCambioByNameQueryHandler : IRequestHandler<GetTipoCambioByNameQuery, TipoDolarResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTipoCambioByNameQuery> _logger;

        public GetTipoCambioByNameQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetTipoCambioByNameQuery> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TipoDolarResponse> Handle(GetTipoCambioByNameQuery request, CancellationToken cancellationToken)
        {
            var tipoDolarSpec = new TipoDolarContainNameSpec(request.Name);
            var tipoDolar = await _unitOfWork.RepositoryAsync<TipoDolar>().FirstOrDefaultAsync(tipoDolarSpec, cancellationToken);
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
