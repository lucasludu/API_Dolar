using Application.DTOs._tipoDolar.Response;
using Application.Interfaces;
using Application.Specification._tipoCambio;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features._tipoCambio.Command.InsertNewTipoDolarCommands
{
    public class InsertNewTipoDolarCommandHandler : IRequestHandler<InsertNewTipoDolarCommand, TipoDolarResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<InsertNewTipoDolarCommand> _logger;

        public InsertNewTipoDolarCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<InsertNewTipoDolarCommand> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TipoDolarResponse> Handle(InsertNewTipoDolarCommand request, CancellationToken cancellationToken)
        {
            var spec = new TipoDolarByNameSpec(request.Request.Nombre);
            var tipoDolarExistente = await _unitOfWork.RepositoryAsync<TipoDolar>().FirstOrDefaultAsync(spec);
            if (tipoDolarExistente != null)
            {
                _logger.LogWarning("El tipo de dolar ya se encuentra registrado");
                throw new Application.Exceptions.ApiException("El tipo de dólar ya existe.");
            }

            var nuevoTipoDolar = new TipoDolar(request.Request.Nombre);
            var createdTipoDolar = await _unitOfWork.RepositoryAsync<TipoDolar>().AddAsync(nuevoTipoDolar);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (createdTipoDolar != null)
            {
                _logger.LogInformation("Tipo de dólar creado exitosamente.");
                return _mapper.Map<TipoDolarResponse>(createdTipoDolar);
            }
            else
            {
                _logger.LogError("No se pudo crear el tipo de dólar.");
                throw new Application.Exceptions.ApiException("No se pudo crear el tipo de dólar.");
            }
        }
    }

}
