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
    public class InsertNewTipoDolarCommand : IRequest<Response<TipoDolarResponse>>
    {
        public TipoDolarRequest Request { get; set; }
        public InsertNewTipoDolarCommand(TipoDolarRequest request)
        {
            Request = request;
        }
    }
    public class InsertNewTipoDolarCommandHandler : IRequestHandler<InsertNewTipoDolarCommand, Response<TipoDolarResponse>>
    {
        private readonly IRepositoryAsync<TipoDolar> _tipoDolarRpository;
        private readonly IMapper _mapper;
        private readonly ILogger<InsertNewTipoDolarCommand> _logger;

        public InsertNewTipoDolarCommandHandler(IRepositoryAsync<TipoDolar> tipoDolarRpository, IMapper mapper, ILogger<InsertNewTipoDolarCommand> logger)
        {
            _tipoDolarRpository = tipoDolarRpository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<TipoDolarResponse>> Handle(InsertNewTipoDolarCommand request, CancellationToken cancellationToken)
        {
            var spec = new TipoDolarByNameSpec(request.Request.Nombre);
            var tipoDolarExistente = await _tipoDolarRpository.FirstOrDefaultAsync(spec);
            if (tipoDolarExistente != null)
            {
                _logger.LogWarning("El tipo de dolar ya se encuentra registrado");
                return Response<TipoDolarResponse>.FailResponse("El tipo de dólar ya existe.");
            }

            var nuevoTipoDolar = _mapper.Map<TipoDolar>(request.Request);
            var createdTipoDolar = await _tipoDolarRpository.AddAsync(nuevoTipoDolar);

            if (createdTipoDolar != null)
            {
                _logger.LogInformation("Tipo de dólar creado exitosamente.");
                return Response<TipoDolarResponse>.SuccessResponse(_mapper.Map<TipoDolarResponse>(createdTipoDolar), "Tipo de dólar creado exitosamente.");
            }
            else
            {
                _logger.LogError("No se pudo crear el tipo de dólar.");
                return Response<TipoDolarResponse>.FailResponse("No se pudo crear el tipo de dólar.");
            }
        }
    }
}
