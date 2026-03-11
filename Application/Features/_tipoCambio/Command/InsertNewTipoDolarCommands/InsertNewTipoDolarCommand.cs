using Application.DTOs._tipoDolar.Request;
using Application.DTOs._tipoDolar.Response;
using Application.Interfaces;
using Application.Specification._tipoCambio;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features._tipoCambio.Command.InsertNewTipoDolarCommands
{
    public class InsertNewTipoDolarCommand : IRequest<TipoDolarResponse>
    {
        public TipoDolarRequest Request { get; set; }
        public InsertNewTipoDolarCommand(TipoDolarRequest request)
        {
            Request = request;
        }
    }
    public class InsertNewTipoDolarCommandHandler : IRequestHandler<InsertNewTipoDolarCommand, TipoDolarResponse>
    {
        private readonly IRepositoryAsync<TipoDolar> _tipoDolarRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<InsertNewTipoDolarCommand> _logger;

        public InsertNewTipoDolarCommandHandler(IRepositoryAsync<TipoDolar> tipoDolarRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<InsertNewTipoDolarCommand> logger)
        {
            _tipoDolarRepository = tipoDolarRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TipoDolarResponse> Handle(InsertNewTipoDolarCommand request, CancellationToken cancellationToken)
        {
            var spec = new TipoDolarByNameSpec(request.Request.Nombre);
            var tipoDolarExistente = await _tipoDolarRepository.FirstOrDefaultAsync(spec);
            if (tipoDolarExistente != null)
            {
                _logger.LogWarning("El tipo de dolar ya se encuentra registrado");
                throw new Application.Exceptions.ApiException("El tipo de dólar ya existe.");
            }

            var nuevoTipoDolar = new TipoDolar(request.Request.Nombre);
            var createdTipoDolar = await _tipoDolarRepository.AddAsync(nuevoTipoDolar);
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

