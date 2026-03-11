using Application.DTOs._tipoDolar.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features._tipoCambio.Queries.GetAllTipoDolarQueries
{
    public class GetAllTipoDolarQueryHandler : IRequestHandler<GetAllTipoDolarQuery, IEnumerable<TipoDolarResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllTipoDolarQuery> _logger;

        public GetAllTipoDolarQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllTipoDolarQuery> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<TipoDolarResponse>> Handle(GetAllTipoDolarQuery request, CancellationToken cancellationToken)
        {
            var listTipoDolar = await _unitOfWork.RepositoryAsync<TipoDolar>().ListAsync(cancellationToken);

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
